using Cysharp.Threading.Tasks;
using DG.Tweening;
using FTRuntime;
using Ryocatusn.Audio;
using Ryocatusn.Games;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(SwfClipController))]
    public class PlayerBody : MonoBehaviour
    {
        [SerializeField]
        private Camera cameraFinalResult;
        [SerializeField]
        private Animator leftHandAnimator;

        [SerializeField]
        private SpriteRenderer leftHandSpriteRenderer;
        [SerializeField]
        private JankenSprites leftHands;

        [SerializeField]
        private JankenSwfClipAssets attackAnimations;
        [SerializeField]
        private JankenSwfClipAssets idleAnimations;
        [SerializeField]
        private JankenSwfClipAssets holdLightAnimations;
        [SerializeField]
        private JankenSwfClipAssets idleLightAnimations;

        [SerializeField]
        private SE playerShotSE;

        private JankenableObjectId playerId;
        private Hand.Shape shape;

        private Vector2 defaultPosition;
        private Vector2 offsetWhenLight = new Vector2(-1.86f, -1.58f);
        private State state = State.None;
        private SwfClipController swfClipController;

        [Inject]
        private GameManager gameManager;
        [Inject]
        private JankenableObjectApplicationService jankenableObjectApplicationService;

        private enum State
        {
            None,
            Attack,
            Idle,
            HoldLight,
            IdleLight
        }

        private void Start()
        {
            defaultPosition = transform.position;
            swfClipController = GetComponent<SwfClipController>();

            playerId = gameManager.gameContains.player.id;
            JankenableObjectEvents playerEvents = gameManager.gameContains.player.events;

            playerEvents.ChangeShapeEvent
                .Subscribe(x => leftHandSpriteRenderer.sprite = leftHands.GetAsset(x))
                .AddTo(this);
        }
        public void ShotLeftHand(Player player, Tilemap firstRoad, Vector2 startPosition, Action finish)
        {
            state = State.Attack;
            transform.position = defaultPosition;

            //だいぶコードが汚いけど重要な場所じゃないし良し
            StartCoroutine(ShotLeftHandEnumerator());
            IEnumerator ShotLeftHandEnumerator()
            {
                //敵がプレイヤーに攻撃しないよう距離を開けてる
                player.tileTransform.SetDisable();
                player.transform.position = new Vector2(-1000, -1000);
                player.inputMaster.SetActiveAll(false);
                PlayAnimation(attackAnimations, GetShape(), SwfClipController.LoopModes.Once);
                //左手を打つタイミング
                yield return new WaitUntil(() => swfClipController.clip.currentFrame >= 49);
                new SEPlayer(gameObject).Play(playerShotSE);
                leftHandAnimator.Play("Shot");
                //左手のアニメーションが終わるタイミング
                yield return new WaitForSeconds(1.2f);
                Vector2 playerPosition = gameManager.GetWorldPositionOnGame(leftHandAnimator.transform.position);
                player.transform.position = playerPosition;

                Sequence sequence = DOTween.Sequence();
                sequence
                    .SetLink(player.gameObject)
                    .Append(player.transform.DOMove(startPosition, 1))
                    .Join(player.transform.DORotate(new Vector3(0, 0, 360 * 3), 1, RotateMode.FastBeyond360))
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        player.tileTransform.ChangeTilemap(new Tilemap[] { firstRoad }, startPosition);
                        player.inputMaster.SetActiveAll(true);

                        Idle();

                        finish();
                    });
            }
        }
        public void Idle()
        {
            transform.position = defaultPosition;
            if (state != State.Idle)
            {
                state = State.Idle;
                PlayAnimation(idleAnimations, GetShape(), SwfClipController.LoopModes.Loop);
            }
            else
            {
                PlayAnimation(idleAnimations, GetShape(), SwfClipController.LoopModes.Loop, swfClipController.clip.currentFrame);
            }
        }
        public void HoldLight()
        {
            state = State.HoldLight;
            transform.position = defaultPosition + offsetWhenLight;

            PlayAnimation(holdLightAnimations, GetShape(), SwfClipController.LoopModes.Once)
                .Subscribe(_ => IdleLight())
                .AddTo(this);
        }
        public void IdleLight()
        {
            transform.position = defaultPosition;
            transform.position = defaultPosition + offsetWhenLight;

            if (state != State.IdleLight) 
            {
                state = State.IdleLight;
                PlayAnimation(idleLightAnimations, GetShape(), SwfClipController.LoopModes.Loop);
            }
            else
            {
                PlayAnimation(idleLightAnimations, GetShape(), SwfClipController.LoopModes.Loop, swfClipController.clip.currentFrame);
            }
        }

        private IObservable<Unit> PlayAnimation(JankenSwfClipAssets jankenSwfClipAssets, Hand.Shape shape, SwfClipController.LoopModes loopMode, int frame = 0)
        {
            swfClipController.loopMode = loopMode;
            SwfClipAsset swfClipAsset = jankenSwfClipAssets.GetAsset(shape);
            swfClipController.clip.clip = swfClipAsset;
            swfClipController.clip.currentFrame = frame;
            swfClipController.Play(false);

            Subject<Unit> onCompletEvent = new Subject<Unit>();
            IObservable<Unit> OnCompleteEvent = onCompletEvent.FirstOrDefault();

            swfClipController.OnStopPlayingEvent += Complete;
            void Complete(SwfClipController swfClipController)
            {
                onCompletEvent.OnNext(Unit.Default);
                swfClipController.OnStopPlayingEvent -= Complete;
            }

            return OnCompleteEvent;
        }

        public void UpdateHandShape()
        {
            shape = jankenableObjectApplicationService.Get(playerId).shape;

            if (state == State.Idle) Idle();
            else if (state == State.IdleLight) Idle();
        }

        private Hand.Shape GetShape()
        {
            return shape;
        }
    }
}
