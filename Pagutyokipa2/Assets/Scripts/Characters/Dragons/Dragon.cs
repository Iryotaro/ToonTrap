using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using System;
using UniRx;
using UnityEngine;
using Ryocatusn.Audio;
using Zenject;
using UniRx.Triggers;

namespace Ryocatusn.Characters
{
    public class Dragon : JankenBehaviour, IReceiveAttack
    {
        [SerializeField]
        public Hand.Shape shape;
        [SerializeField]
        private DragonView view;
        [SerializeField, Min(1)]
        private int atk = 1;
        [SerializeField, Min(0)]
        private float attackRange;
        [SerializeField, Min(1)]
        private float attackDelay = 1;

        [Inject]
        private StageManager stageManager;

        [Inject]
        private BulletFactory bulletFactory;
        [SerializeField]
        private Bullet bullet;
        [SerializeField]
        private Transform shotPoint;

        [SerializeField]
        private SE attackSE;

        private Player player;
        private Subject<Unit> attackEvent = new Subject<Unit>();

        private void Start()
        {
            JankenableObjectCreateCommand command = new JankenableObjectCreateCommand(new Hp(1), shape, stageManager.id);
            Create(command);

            stageManager.SetupStageEvent
                .Subscribe(x =>
                {
                    player = x.player;

                    view.SetUp();

                    attackEvent
                        .ThrottleFirst(TimeSpan.FromSeconds(attackDelay))
                        .Subscribe(_ => AttackTrigger());

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
                        if (view.skinnedMeshRenderer.isVisible && player != null && Vector2.Distance(transform.position, player.transform.position) <= attackRange)
                        {
                            attackEvent.OnNext(Unit.Default);
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
            if (player != null) bulletFactory.Create(bullet, id, gameObject, shotPoint.transform.position, player.transform);
            else bulletFactory.Create(bullet, id, gameObject, shotPoint.transform.position, shotPoint.transform.rotation);
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
    }
}
