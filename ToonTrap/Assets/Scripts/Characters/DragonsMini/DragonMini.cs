using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using UniRx;
using UnityEngine;
using Ryocatusn.Audio;
using Zenject;
using UniRx.Triggers;

namespace Ryocatusn.Characters
{
    public class DragonMini : JankenBehaviour, IReceiveAttack, IForJankenViewEditor
    {
        [SerializeField]
        public Hand.Shape shape;
        [SerializeField, Min(1)]
        private int atk = 1;
        [SerializeField, Min(0)]
        private float attackRange;

        [Inject]
        private StageManager stageManager;
        [Inject]
        private DiContainer container;

        [Inject]
        private BulletFactory bulletFactory;
        [SerializeField]
        private Bullet bullet;

        [SerializeField]
        private JankenPrefabs jankenPrefabs;
        private DragonMiniView dragonView;

        [SerializeField]
        private SE attackSE;

        private Player player;

        public bool isAllowedToReceiveAttack { get; private set; } = true;

        private void Start()
        {
            Create(new Hp(1), shape);

            player = gameManager.gameContains.player;

            dragonView.SetUp();

            dragonView.AttackTriggerEvent
            .Subscribe(_ => AttackTrigger())
            .AddTo(this);

            events.AttackTriggerEvent
            .Subscribe(x => HandleAttackTrigger(x.id))
            .AddTo(this);

            events.DieEvent
            .Subscribe(_ => HandleDie())
            .AddTo(this);

            SEPlayer sePlayer = new SEPlayer(gameObject, gameManager.gameContains.gameCamera);

            events.AttackTriggerEvent.Subscribe(_ => sePlayer.Play(attackSE)).AddTo(this);

            stageManager.SetupStageEvent
                .Subscribe(_ =>
                {
                    this.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        if (dragonView.IsVisible() && player != null && Vector2.Distance(transform.position, player.transform.position) <= attackRange)
                        {
                            dragonView.StartAttackAnimation();
                        }
                    });
                })
                .AddTo(this);
        }

        private void AttackTrigger()
        {
            AttackableObjectCreateCommand command = new AttackableObjectCreateCommand(id, GetData().shape, new Atk(atk));
            AttackTrigger(command);
        }

        private void HandleAttackTrigger(AttackableObjectId id)
        {
            if (player != null) bulletFactory.Create(bullet, id, gameObject, dragonView.shotPoint.transform.position, player.transform);
            else bulletFactory.Create(bullet, id, gameObject, dragonView.shotPoint.transform.position, dragonView.shotPoint.transform.rotation);
        }

        private void HandleDie()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
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
            if (jankenPrefabs.TryGetRenderer(out GameObject gameObject, this))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (Application.isPlaying) Destroy(transform.GetChild(i).gameObject);
                    else DestroyImmediate(transform.GetChild(i).gameObject);
                }
                GameObject prefab = jankenPrefabs.GetAsset(shape);
                GameObject newGameObject = Instantiate(prefab, gameObject.transform);
                if (container != null) container.InjectGameObject(newGameObject);
                dragonView = newGameObject.GetComponent<DragonMiniView>();

                foreach (ReceiveAttackChild receiveAttackChild in GetComponentsInChildren<ReceiveAttackChild>())
                {
                    receiveAttackChild.jankenBehaviour = this;
                }
            }
        }
    }
}
