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
    public class Dragon : JankenBehaviour, IReceiveAttack, IForJankenViewEditor
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
        private DragonView dragonView;

        [SerializeField]
        private SE attackSE;

        private Player player;

        private void Start()
        {
            JankenableObjectCreateCommand command = new JankenableObjectCreateCommand(new Hp(1), shape, stageManager.id);
            Create(command);

            stageManager.SetupStageEvent
                .Subscribe(x =>
                {
                    player = x.player;

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

                    SEPlayer sePlayer = new SEPlayer(gameObject);

                    events.AttackTriggerEvent.Subscribe(_ => sePlayer.Play(attackSE)).AddTo(this);

                    this.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        if (dragonView.IsVisible() && player != null && Vector2.Distance(transform.position, player.transform.position) <= attackRange)
                        {
                            dragonView.StartAttackAnimation();
                        }
                    });
                });
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
                    DestroyImmediate(transform.GetChild(i).gameObject);
                }
                GameObject prefab = jankenPrefabs.GetAsset(shape);
                GameObject newGameObject = Instantiate(prefab, gameObject.transform);
                container.InjectGameObject(newGameObject);
                dragonView = newGameObject.GetComponent<DragonView>();
            }
        }
    }
}
