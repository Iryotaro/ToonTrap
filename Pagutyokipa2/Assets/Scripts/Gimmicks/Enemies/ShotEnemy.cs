using UniRx;
using UnityEngine;
using Ryocatusn.Games.Stages;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Audio;

namespace Ryocatusn
{
    [RequireComponent(typeof(Collider2D))]
    public class ShotEnemy : SimpleEnemy
    {
        [SerializeField]
        private float attackRange = 5;
        [SerializeField]
        private BulletParameter bulletParameter;
        [SerializeField, Min(0.1f)]
        private float attackRate = 1;
        [SerializeField, Min(1)]
        private int atk = 1;
        [SerializeField]
        private Transform shotPoint;
        [SerializeField]
        private Bullet bullet;
        [SerializeField]
        private SE shotSE;
        [SerializeField]
        private Animator animator;

        private IReceiveAttack target;

        private Subject<Unit> attack = new Subject<Unit>();

        protected override void Started()
        {
            StageManager.activeStage.SetupStageEvent
                .Subscribe(x => target = x.player)
                .AddTo(this);

            attack
                .ThrottleFirst(System.TimeSpan.FromSeconds(attackRate))
                .Subscribe(_ =>
                {
                    if (!jankenableObjectApplicationService.IsEnable(id)) return;
                    AttackableObjectCreateCommand command = new AttackableObjectCreateCommand(id, jankenableObjectApplicationService.Get(id).shape, new Atk(atk));
                    jankenableObjectApplicationService.AttackTrigger(id, command);
                })
                .AddTo(this);

            animator = GetComponent<Animator>();
        }
        protected override void OnDestroyed()
        {
            attack.Dispose();
        }

        protected override JankenableObjectId Create(Hp hp, Hand.Shape shape, StageId stageId)
        {
            JankenableObjectCreateCommand command = new JankenableObjectCreateCommand(hp, shape, stageId);
            return jankenableObjectApplicationService.Create(command);
        }
        protected override void SetEventHandlers(JankenableObjectEvents events)
        {
            base.SetEventHandlers(events);

            events.AttackTriggerEvent.Subscribe(x => HandleAttackTrigger(x.id)).AddTo(this);
        }

        private void Update()
        {
            StageManager.activeStage.gameContains.Match(x => 
            {
                if (Vector2.Distance(transform.position, x.player.transform.position) < attackRange) Attack();
            });
        }
        
        private void Attack()
        {
            attack.OnNext(Unit.Default);
        }
        private void HandleAttackTrigger(AttackableObjectId attackableObjectId)
        {
            animator.SetTrigger("Shot");
            BulletFactory bulletFactory = new BulletFactory(bullet, attackableObjectId, bulletParameter, shotPoint, shotPoint.rotation, target);
            bulletFactory.Create();

            sePlayer.Play(shotSE);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
