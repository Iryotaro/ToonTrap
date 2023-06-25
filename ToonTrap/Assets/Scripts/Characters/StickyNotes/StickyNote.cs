using UnityEngine;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Janken.AttackableObjects;
using UniRx;

namespace Ryocatusn.Characters
{
    public class StickyNote : JankenBehaviour
    {
        private Hand.Shape shape = Hand.Shape.Paper;
        [SerializeField, Min(1)]
        private int atk = 1;

        [SerializeField]
        private TileTransformTrigger trigger;
        [SerializeField]
        private StickyNoteSniperScope sniperScope;

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
    }
}
