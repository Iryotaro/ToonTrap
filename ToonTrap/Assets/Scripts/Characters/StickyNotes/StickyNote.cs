using UnityEngine;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Janken.AttackableObjects;
using UniRx;
using Ryocatusn.Photographers;
using Zenject;

namespace Ryocatusn.Characters
{
    public class StickyNote : JankenBehaviour, IPhotographerSubject
    {
        private Hand.Shape shape = Hand.Shape.Paper;
        [SerializeField, Min(1)]
        private int atk = 1;

        [SerializeField]
        private TileTransformTrigger trigger;
        [SerializeField]
        private StickyNoteSniperScope sniperScope;

        [Inject]
        private PhotographerSubjectManager photographerSubjectManager;

        public int priority { get; } = 10;
        public int photographerCameraSize { get; } = 3;

        private void Start()
        {
            Create(new Hp(1), shape);

            trigger.OnHitPlayerEvent
                .First()
                .Subscribe(_ => HandleTileTransformTrigger())
                .AddTo(this);

            events.AttackTriggerEvent
                .Subscribe(x => HandleAttackTrigger(x.id, x.receiveAttacks[0]))
                .AddTo(this);

            photographerSubjectManager.Save(this);
        }
        private void OnDestroy()
        {
            photographerSubjectManager.Delete(this);
        }

        private void HandleTileTransformTrigger()
        {
            AttackableObjectCreateCommand command = new AttackableObjectCreateCommand(id, shape, new Atk(atk));
            AttackTrigger(command, gameManager.gameContains.player);
        }
        private void HandleAttackTrigger(AttackableObjectId id, IReceiveAttack receiveAttack)
        {
            sniperScope.Setup(id, receiveAttack);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}
