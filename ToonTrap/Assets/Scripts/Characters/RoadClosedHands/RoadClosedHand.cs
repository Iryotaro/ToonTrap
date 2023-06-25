using UnityEngine;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Janken.AttackableObjects;
using UniRx;
using UniRx.Triggers;
using Ryocatusn.TileTransforms;
using Zenject;

namespace Ryocatusn.Characters
{
    public class RoadClosedHand : JankenBehaviour, IForJankenViewEditor, IReceiveAttack
    {
        [SerializeField]
        public Hand.Shape shape;
        [SerializeField, Min(1)]
        private int atk = 1;
        [SerializeField]
        private TileTransformTrigger trigger;
        [SerializeField]
        private SpriteRenderer spriteRendererMain;
        [SerializeField]
        private SpriteRenderer spriteRendererAdditional;
        [SerializeField]
        private JankenSprites jankenSpritesMain;
        [SerializeField]
        private JankenSprites jankenSpritesAdditional;

        [Inject]
        private AttackableObjectApplicationService attackableObjectApplicationService;

        public bool isAllowedToReceiveAttack { get; } = true;

        private void Start()
        {
            Create(new Hp(1), shape);

            events.AttackTriggerEvent
                 .Subscribe(x => HandleAttackTrigger(x.id, x.receiveAttacks))
                 .AddTo(this);

            events.DieEvent
                .Subscribe(_ => Destroy(gameObject))
                .AddTo(this);

            events.AttackerLoseEvent
                .Subscribe(_ => Destroy(gameManager))
                .AddTo(this);

            events.AttackerWinEvent
                .Select(x => characterManager.Find(x) as Player)
                .Where(x => x != null)
                .Subscribe(x => BouncePlayer(x))
                .AddTo(this);

            events.AttackerDrawEvent
                .Select(x => characterManager.Find(x) as Player)
                .Where(x => x != null)
                .Subscribe(x => BouncePlayer(x))
                .AddTo(this);

            this.OnTriggerEnter2DAsObservable()
                .Subscribe(x =>
                {
                    if (x.TryGetComponent(out IReceiveAttack receiveAttack))
                    {
                        AttackableObjectCreateCommand command = new AttackableObjectCreateCommand(id, shape, new Atk(atk));
                        AttackTrigger(command, receiveAttack);
                    }
                });
        }

        private void HandleAttackTrigger(AttackableObjectId id, IReceiveAttack[] receiveAttacks)
        {
            foreach (IReceiveAttack receiveAttack in receiveAttacks)
            {
                attackableObjectApplicationService.Attack(id, receiveAttack);
            }
        }

        private void BouncePlayer(Player player)
        {
            if (player.tileTransform.movement.Get() == null) return;

            TilePosition tilePosition = player.tileTransform.tilePosition.Get();
            TileDirection tileDirection = new TileDirection(player.tileTransform.angle + 180);
            IMoveDataCreater moveDataCreater = new MoveStraightLine(tilePosition, tileDirection, 1);
            player.tileTransform.SetMovement(moveDataCreater, new MoveRate(4), TileTransform.SetMovementMode.Force);
        }

        public JankenableObjectId GetId()
        {
            return id;
        }

        public Hand.Shape GetShape()
        {
            return shape;
        }

        public void UpdateView(Hand.Shape shape)
        {
            spriteRendererMain.sprite = jankenSpritesMain.GetAsset(shape);
            spriteRendererAdditional.sprite = jankenSpritesAdditional.GetAsset(shape);
        }
    }
}
