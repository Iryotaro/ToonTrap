using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Ryocatusn.Games.Stages;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Audio;

namespace Ryocatusn
{
    public class HandChangerShotEnemy : SimpleEnemy
    {
        [SerializeField]
        private float attackRange = 5;
        [SerializeField, Min(0.1f)]
        private float attackRate = 1;
        [SerializeField]
        private HandChangerBullet handChangerBullet;
        [SerializeField]
        private SE shotSE;

        private Player player;

        Subject<Unit> attackSubject = new Subject<Unit>();

        protected override void Started()
        {
            StageManager.activeStage.SetupStageEvent
                .Subscribe(gameContains => player = gameContains.player)
                .AddTo(this);

            attackSubject
                .ThrottleFirst(TimeSpan.FromSeconds(1 / attackRate))
                .Subscribe(_ => HandleAttackTrigger());
        }
        private void OnDestroy()
        {
            attackSubject.Dispose();
        }

        protected override JankenableObjectId Create(Hp hp, Hand.Shape shape, StageId stageId)
        {
            JankenableObjectCreateCommand command = new JankenableObjectCreateCommand(hp, shape, stageId);
            return jankenableObjectApplicationService.Create(command);
        }

        private void Update()
        {
            if (player == null) return;
            if (Vector2.Distance(player.transform.position, transform.position) < attackRange) AttackTrigger();
        }

        private void AttackTrigger()
        {
            attackSubject.OnNext(Unit.Default);
        }
        private void HandleAttackTrigger()
        {
            HandChangerBulletFactory handChangerBulletFactory = new HandChangerBulletFactory(handChangerBullet, (player.transform, player), transform);
            handChangerBulletFactory.Create();

            sePlayer.Play(shotSE);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
