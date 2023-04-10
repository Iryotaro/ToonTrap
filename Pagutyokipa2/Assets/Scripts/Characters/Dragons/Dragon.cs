using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using System;
using UniRx;
using UnityEngine;

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
        [SerializeField]
        private Bullet bullet;
        [SerializeField]
        private Transform shotPoint;

        private Player player;

        private Subject<Unit> attackEvent = new Subject<Unit>();

        private void Start()
        {
            JankenableObjectCreateCommand command = new JankenableObjectCreateCommand(new Hp(1), shape, StageManager.activeStage.id);
            Create(command);

            view.SetUp();

            attackEvent
                .ThrottleFirst(TimeSpan.FromSeconds(attackDelay))
                .Subscribe(_ => AttackTrigger());

            events.AttackTriggerEvent
                .Subscribe(x => HandleAttackTrigger(x.id))
                .AddTo(this);

            StageManager.activeStage.SetupStageEvent
                .Subscribe(x => player = x.player)
                .AddTo(this);
        }

        private void Update()
        {
            if (view.skinnedMeshRenderer.isVisible && player != null && Vector2.Distance(transform.position, player.transform.position) <= attackRange)
            {
                attackEvent.OnNext(Unit.Default);
            }
        }

        private void AttackTrigger()
        {
            AttackableObjectCreateCommand command = new AttackableObjectCreateCommand(id, GetData().shape, new Atk(atk));
            AttackTrigger(command);
        }

        private void HandleAttackTrigger(AttackableObjectId id)
        {
            if (player != null) BulletFactory.Create(bullet, id, gameObject, shotPoint.transform.position, player.transform);
            else BulletFactory.Create(bullet, id, gameObject, shotPoint.transform.position, shotPoint.transform.rotation);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
