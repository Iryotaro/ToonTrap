using UnityEngine;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using UniRx;

namespace Ryocatusn.Characters
{
    public class StickyNote : JankenBehaviour
    {
        [SerializeField]
        public Hand.Shape shape;
        [SerializeField, Min(1)]
        private int atk = 1;
        [SerializeField, Min(0)]
        private float attackRange;

        [SerializeField]
        private TileTransformTrigger trigger;

        private void Start()
        {
            Create(new Hp(1), shape);

            trigger.OnHitPlayerEvent
                .First()
                .Subscribe(_ => HandleTileTransformTrigger())
                .AddTo(this);

            events.AttackTriggerEvent
                .Subscribe(x => HandleAttackTrigger())
                .AddTo(this);
        }

        private void HandleTileTransformTrigger()
        {

        }
        private void HandleAttackTrigger()
        {

        }
    }
}
