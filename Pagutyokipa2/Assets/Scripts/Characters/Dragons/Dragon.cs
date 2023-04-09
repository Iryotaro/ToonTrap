using Ryocatusn.Audio;
using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using UniRx;
using UnityEngine;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class Dragon : JankenBehaviour, IReceiveAttack
    {
        [SerializeField]
        public Hand.Shape shape;
        [SerializeField]
        private int atk;
        [SerializeField]
        private Bullet bullet;
        [SerializeField]
        private Transform shotPoint;

        private SkinnedMeshRenderer skinnedMeshRenderer;

        private Player player;

        private void Start()
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

            JankenableObjectCreateCommand command = new JankenableObjectCreateCommand(new Hp(1), shape, StageManager.activeStage.id);
            Create(command);

            SEPlayer sePlayer = new SEPlayer(gameObject, skinnedMeshRenderer);

            events.AttackTriggerEvent
                .Subscribe(x => HandleAttackTrigger(x.id, x.receiveAttacks))
                .AddTo(this);

            events.DieEvent
                .Subscribe(_ => Destroy(gameObject))
                .AddTo(this);

            StageManager.activeStage.SetupStageEvent
                .Subscribe(x => player = x.player)
                .AddTo(this);

            Invoke(nameof(At), 4);
        }

        private void At()
        {
            AttackableObjectCreateCommand command = new AttackableObjectCreateCommand(id, GetData().shape, new Atk(atk));
            AttackTrigger(command);
        }

        private void HandleAttackTrigger(AttackableObjectId id, IReceiveAttack[] receiveAttacks)
        {
            BulletFactory.Create(bullet, id, shotPoint.transform.position, player.transform);
        }
    }
}
