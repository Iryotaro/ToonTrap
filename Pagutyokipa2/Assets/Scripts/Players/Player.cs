using Ryocatusn.Audio;
using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.TileTransforms;
using Ryocatusn.UI;
using Ryocatusn.Characters;
using UniRx;
using UnityEngine;

namespace Ryocatusn
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(TileTransform))]
    [RequireComponent(typeof(PlayerInputMaster))]
    public class Player : JankenBehaviour, IReceiveAttack
    {
        public TileTransform tileTransform { get; private set; }
        private bool move = false;
        private MoveRate moveRate;

        public PlayerInputMaster inputMaster { get; private set; }

        private SEPlayer sePlayer;

        [SerializeField, Min(1)]
        private float m_moveRate = 1;
        [SerializeField, Min(1)]
        private int hp = 1;
        [SerializeField, Min(0)]
        private float invincibleTime = 1;
        [SerializeField, Min(1)]
        private int atk = 1;
        [SerializeField]
        private GameManager gameManager;

        [SerializeField]
        private JankenSprites jankenSprites;
        [SerializeField]
        private ParticleSystem takeDamageParticle;

        [SerializeField]
        private SE attackSE;
        [SerializeField]
        private SE takeDamageSE;
        [SerializeField]
        private SE changeShapeSE;
        [SerializeField]
        private SE dieSE;

        private void Start()
        {
            JankenableObjectCreateCommand command = new JankenableObjectCreateCommand(new Hp(hp), new InvincibleTime(invincibleTime), Hand.Shape.Rock);
            Create(command);

            tileTransform = GetComponent<TileTransform>();
            tileTransform.ChangeDirection(new TileDirection(TileDirection.Direction.Up));
            moveRate = new MoveRate(m_moveRate);

            if (TryGetComponent(out IJankenableObjectUI jankenableObjectUI)) jankenableObjectUI.Setup(id);

            inputMaster = GetComponent<PlayerInputMaster>();

            events.AttackTriggerEvent
                .Subscribe(x => HandleAttackTrigger(x.id))
                .AddTo(this);

            events.DieEvent
                .Subscribe(_ => HandleDie())
                .AddTo(this);

            inputMaster.MoveEvent.Subscribe(_ => move = true).AddTo(this);
            inputMaster.CancelMoveEvent.Subscribe(_ => move = false).AddTo(this);
            inputMaster.ChangeDirectionEvent.Subscribe(x => tileTransform.ChangeDirection(x));
            inputMaster.AttackEvent.Subscribe(_ => AttackTrigger()).AddTo(this);
            inputMaster.ChangeShapeEvent.Subscribe(x => ChangeShape(x));

            sePlayer = new SEPlayer(gameObject);

            events.AttackTriggerEvent.Subscribe(_ => sePlayer.Play(attackSE));
            events.TakeDamageEvent.Subscribe(_ => sePlayer.Play(takeDamageSE));

            if (TryGetComponent(out PlayerView playerView)) playerView.StartView(this);

            if (gameManager != null) gameManager.SetStageEvent.Subscribe(_ => Init()).AddTo(this);
        }
        private void OnDestroy()
        {
            jankenableObjectApplicationService.Delete(id);
        }

        public void Init()
        {
            jankenableObjectApplicationService.ResetHp(id);
        }

        private void Update()
        {
            if (move) Move();
        }

        private void Move()
        {
            MoveTranslate moveTranslate = new MoveTranslate(tileTransform.tilePosition.Get(), tileTransform.tileDirection);
            tileTransform.SetMovement(moveTranslate, moveRate);
        }
        private void ChangeShape(Hand.Shape shape)
        {
            if (jankenableObjectApplicationService.Get(id).shape == shape) return;
            jankenableObjectApplicationService.ChangeShape(id, shape);
            sePlayer.Play(changeShapeSE);
        }
        private void AttackTrigger()
        {
            AttackableObjectCreateCommand attackableObjectCreateCommand = new AttackableObjectCreateCommand(id, jankenableObjectApplicationService.Get(id).shape, new Atk(atk));
            jankenableObjectApplicationService.AttackTrigger(id, attackableObjectCreateCommand);
        }

        private void HandleAttackTrigger(AttackableObjectId attackableObjectId)
        {
            //球を打つ
        }
        private void HandleDie()
        {
            sePlayer.Play(dieSE);
            StageManager.activeStage.Over();
        }
    }
}
