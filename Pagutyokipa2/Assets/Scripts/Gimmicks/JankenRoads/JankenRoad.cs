using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Audio;

namespace Ryocatusn
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(TilemapRenderer))]
    public class JankenRoad : MonoBehaviour
    {
        private JankenableObjectId jankenableObjectId;

        private AttackableObjectCreateCommand attackableObjectCreateCommand;

        private JankenableObjectApplicationService jankenableObjectApplicationService = Installer.installer.serviceProvider.GetService<JankenableObjectApplicationService>();
        private AttackableObjectApplicationService attackableObjectApplicationService = Installer.installer.serviceProvider.GetService<AttackableObjectApplicationService>();

        private List<(IReceiveAttack receiveAttack, GameObject gameObject)> targets = new List<(IReceiveAttack receiveAttack, GameObject gameObject)>();

        public Tilemap tilemap { get; private set; }

        private SEPlayer sePlayer;

        public Hand.Shape shape;
        [SerializeField]
        private HandTiles handTiles;
        [SerializeField]
        private HandTiles crackHandTiles;
        [SerializeField]
        private BreakJankenRoads breakJankenRoads;

        [SerializeField]
        private bool isCrack;

        [SerializeField]
        private SE crackSE;
        [SerializeField]
        private SE breakSE;
        [SerializeField]
        private SE dropDownSE;

        private static bool fallDownPlayer = false;

        private Subject<Unit> attackTriggerEvent = new Subject<Unit>();

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (IsVisiable(tilemap)) enabled = true;
                });

            enabled = false;
        }

        private void Start()
        {
            JankenableObjectCreateCommand jankenableCommand = new JankenableObjectCreateCommand(new Hp(IsCrack() ? 1 : 2), new InvincibleTime(1), shape, StageManager.activeStage.id);
            jankenableObjectId = jankenableObjectApplicationService.Create(jankenableCommand);

            attackableObjectCreateCommand = new AttackableObjectCreateCommand(jankenableObjectId, jankenableObjectApplicationService.Get(jankenableObjectId).shape, new Atk(1));

            JankenableObjectEvents jankenableObjectEvents = jankenableObjectApplicationService.GetEvents(jankenableObjectId);

            jankenableObjectEvents.AttackTriggerEvent
                .Subscribe(x => HandleAttackTrigger(x))
                .AddTo(this);

            jankenableObjectEvents.TakeDamageEvent
                .Subscribe(_ => Crack())
                .AddTo(this);

            jankenableObjectEvents.DieEvent
                .FirstOrDefault()
                .Subscribe(_ => Break())
                .AddTo(this);

            //処理を軽くするため
            attackTriggerEvent
                .ThrottleFirst(TimeSpan.FromSeconds(0.1f))
                .Subscribe(_ =>
                {
                    if (!jankenableObjectApplicationService.IsEnable(jankenableObjectId)) return;
                    jankenableObjectApplicationService.AttackTrigger(jankenableObjectId, attackableObjectCreateCommand, targets.Select(x => x.receiveAttack).ToArray());
                })
                .AddTo(this);

            SetAddTarget();
            SetRemoveTarget();

            StageManager.activeStage.SetupStageEvent
                .Subscribe(x =>
                {
                    x.player.tileTransform.AddTilemap(GetComponent<Tilemap>());
                });

            sePlayer = new SEPlayer(gameObject, GetComponent<Tilemap>());
        }
        private void OnDestroy()
        {
            jankenableObjectApplicationService.Delete(jankenableObjectId);

            attackTriggerEvent.Dispose();
        }

        private void Update()
        {
            if (targets.Count != 0) AttackTrigger();
        }

        private void SetAddTarget()
        {
            gameObject.OnTriggerStay2DAsObservable()
                .Subscribe(x =>
                {
                    if (x == null) return;
                    if (targets.Where(target => target.gameObject.Equals(x.gameObject)).Select(x => x.gameObject).FirstOrDefault() != null) return;
                    if (x.TryGetComponent(out IReceiveAttack receiveAttack))
                    {
                        if (IsOnRoad(x.gameObject)) targets.Add((receiveAttack, x.gameObject));
                    }
                });
        }
        private void SetRemoveTarget()
        {
            this.LateUpdateAsObservable()
                .Subscribe(_ =>
                {
                    targets = targets.Where(x => IsOnRoad(x.gameObject)).ToList();
                });
        }

        private bool IsOnRoad(GameObject receiveAttack)
        {
            if ((GetWorldPosition() - receiveAttack.transform.position).magnitude < 0.3f) return true;
            return false;
        }

        private void AttackTrigger()
        {
            attackTriggerEvent.OnNext(Unit.Default);
        }

        private void HandleAttackTrigger((AttackableObjectId id, IReceiveAttack[] receiveAttacks) x)
        {
            if (!attackableObjectApplicationService.IsEnable(x.id)) return;

            attackableObjectApplicationService.GetEvents(x.id).ReAttackTriggerEvent
            .Subscribe(_ => attackableObjectApplicationService.ReAttack(x.id))
            .AddTo(this);

            foreach (IReceiveAttack receiveAttack in x.receiveAttacks)
            {
                if (!attackableObjectApplicationService.IsEnable(x.id)) return;
                attackableObjectApplicationService.Attack(x.id, receiveAttack);
            }

            attackableObjectApplicationService.Delete(x.id);
        }

        private void Crack()
        {
            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos)) continue;

                tilemap.SetTile(pos, shape switch
                {
                    Hand.Shape.Rock => crackHandTiles.rockTile,
                    Hand.Shape.Scissors => crackHandTiles.scissorsTile,
                    Hand.Shape.Paper => crackHandTiles.paperTile,
                    _ => null
                });
            }

            sePlayer.Play(crackSE);
        }
        public void Break(bool withoutGameOver = false)
        {
            if (StageManager.activeStage.gameContains.Get() == null) return;

            if (sePlayer == null) return;
            sePlayer.Play(breakSE);

            BreakJankenRoad breakJankenRoad = Instantiate(breakJankenRoads.Get(shape));
            breakJankenRoad.transform.position = GetWorldPosition();
            GetComponent<TilemapRenderer>().enabled = false;

            //とりあえず、プレイヤーのことしか考慮しない
            //とりあえず、雑でいい
            Player player = StageManager.activeStage.gameContains.Get().player;

            if (fallDownPlayer)
            {
                Destroy(gameObject);
                return;
            }
            if (withoutGameOver && !IsOnRoad(player.gameObject))
            {
                Destroy(gameObject);
                return;
            }

            fallDownPlayer = true;
            StageManager.activeStage.AddResetHandler(() => fallDownPlayer = false);

            player.GetComponent<Collider2D>().enabled = false;
            player.inputMaster.SetActiveAll(false);
            player.tileTransform.SetDisable();

            Sequence sequence = DOTween.Sequence();
            sequence
                .SetLink(gameObject)
                .Append(player.transform.DOMove(GetWorldPosition(), 0.1f))
                .AppendCallback(() => sePlayer.Play(dropDownSE))
                .Append(player.transform.DORotate(new Vector3(0, 0, 360 * 2), 1, RotateMode.FastBeyond360))
                .Join(player.transform.DOScale(Vector3.zero, 1))
                .AppendCallback(() => Destroy(gameObject))
                .AppendCallback(() => StageManager.activeStage.Over());

            StageManager.activeStage.AddResetHandler(() => player.transform.localScale = Vector3.one);
            StageManager.activeStage.AddResetHandler(() => player.GetComponent<Collider2D>().enabled = true);
        }

        private Vector3 GetWorldPosition()
        {
            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos)) continue;
                Vector3 worldPosition = tilemap.CellToWorld(pos) + Vector3.Scale(tilemap.cellSize, tilemap.transform.lossyScale) / 2;
                return worldPosition;
            }
            return default;
        }

        public Hand.Shape GetHandShape()
        {
            return shape;
        }
        public HandTiles GetHandTiles()
        {
            return handTiles;
        }
        public HandTiles GetCrackHandTiles()
        {
            return crackHandTiles;
        }
        public bool IsCrack()
        {
            return isCrack;
        }

        private bool IsVisiable(Tilemap tilemap)
        {
            Vector3 position = default;
            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos)) continue;
                position = tilemap.CellToWorld(pos) + Vector3.Scale(tilemap.cellSize, tilemap.transform.lossyScale) / 2;
            }

            Camera mainCamera = Camera.main;
            Vector2 screenPoint = mainCamera.WorldToViewportPoint(position);
            if (screenPoint.x >= 0 && screenPoint.x <= 1 &&
                screenPoint.y >= 0 && screenPoint.y <= 1) return true;
            return false;
        }

        [Serializable]
        private class BreakJankenRoads
        {
            public BreakJankenRoad rockGameObject;
            public BreakJankenRoad scissorsGameObject;
            public BreakJankenRoad paperGameObject;

            public BreakJankenRoad Get(Hand.Shape shape)
            {
                return shape switch
                {
                    Hand.Shape.Rock => rockGameObject,
                    Hand.Shape.Scissors => scissorsGameObject,
                    Hand.Shape.Paper => paperGameObject,
                    _ => null
                };
            }
        }
    }
}
