using Ryocatusn.Audio;
using Ryocatusn.Characters;
using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.TileTransforms;
using Ryocatusn.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(TileTransform))]
    [RequireComponent(typeof(PlayerInputMaster))]
    [RequireComponent(typeof(PlayerJankenChanger))]
    public class Player : JankenBehaviour, IReceiveAttack
    {
        public TileTransform tileTransform { get; private set; }
        private bool move = false;
        private MoveRate moveRate;

        public PlayerInputMaster inputMaster { get; private set; }

        private PlayerJankenChanger jankenChanger;

        public bool isAllowedToReceiveAttack { get; set; } = true;

        [SerializeField, Min(1)]
        private float m_moveRate = 1;
        [SerializeField, Min(1)]
        private int hp = 1;
        [SerializeField, Min(0)]
        private float invincibleTime = 1;
        [SerializeField, Min(1)]
        private int atk = 1;

        [Inject]
        private BulletFactory bulletFactory;
        [SerializeField]
        private Bullet bullet;

        [SerializeField]
        private SE attackSE;
        [SerializeField]
        private SE takeDamageSE;
        [SerializeField]
        private SE changeShapeSE;
        [SerializeField]
        private SE dieSE;

        public void Setup()
        {
            Create(new Hp(hp), new InvincibleTime(invincibleTime), Hand.Shape.Rock);

            tileTransform = GetComponent<TileTransform>();
            tileTransform.ChangeDirection(new TileDirection(TileDirection.Direction.Up));
            moveRate = new MoveRate(m_moveRate);

            if (TryGetComponent(out PlayerUI playerUI)) playerUI.Setup(id);

            inputMaster = GetComponent<PlayerInputMaster>();

            jankenChanger = GetComponent<PlayerJankenChanger>();

            events.AttackTriggerEvent
                .Subscribe(x => HandleAttackTrigger(x.id))
                .AddTo(this);

            events.DieEvent
                .Subscribe(_ => HandleDie())
                .AddTo(this);

            inputMaster.MoveEvent.Subscribe(_ => move = true).AddTo(this);
            inputMaster.CancelMoveEvent.Subscribe(_ => move = false).AddTo(this);
            inputMaster.ChangeDirectionEvent.Subscribe(x => tileTransform.ChangeDirection(x)).AddTo(this);
            inputMaster.AttackEvent.Subscribe(_ => AttackTrigger()).AddTo(this);
            inputMaster.ChangeShapeEvent.Subscribe(_ => ChangeShape()).AddTo(this);

            SEPlayer sePlayer = new SEPlayer(gameObject, gameManager.gameContains.gameCamera);

            events.AttackTriggerEvent.Subscribe(_ => sePlayer.Play(attackSE)).AddTo(this);
            events.TakeDamageEvent.Subscribe(_ => sePlayer.Play(takeDamageSE)).AddTo(this);
            events.ChangeShapeEvent.Subscribe(_ => sePlayer.Play(changeShapeSE)).AddTo(this);
            events.DieEvent.Subscribe(_ => sePlayer.Play(dieSE)).AddTo(this);

            if (TryGetComponent(out PlayerView playerView)) playerView.Setup(this);

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
            IMoveDataCreater moveDataCreater = new MoveTranslate(tileTransform.tilePosition.Get(), tileTransform.tileDirection);
            tileTransform.SetMovement(moveDataCreater, moveRate);
        }

        private void ChangeShape()
        {
            if (jankenableObjectApplicationService.Get(id).shape == jankenChanger.GetShape()) return;
            jankenableObjectApplicationService.ChangeShape(id, jankenChanger.GetShape());
            jankenChanger.ChangePlayerShape(jankenChanger.GetShape());
        }
        private void AttackTrigger()
        {
            AttackableObjectCreateCommand attackableObjectCreateCommand = new AttackableObjectCreateCommand(id, jankenableObjectApplicationService.Get(id).shape, new Atk(atk));
            jankenableObjectApplicationService.AttackTrigger(id, attackableObjectCreateCommand);
        }

        private void HandleAttackTrigger(AttackableObjectId attackableObjectId)
        {
            bulletFactory.Create(bullet, attackableObjectId, gameObject, transform.position, tileTransform.tileDirection);
        }
        private void HandleDie()
        {
            gameManager.nowStageManager.Over();
        }

        public JankenableObjectId GetId()
        {
            return id;
        }
    }
}
