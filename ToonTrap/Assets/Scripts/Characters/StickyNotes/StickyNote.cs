using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Janken.JankenableObjects;
using UniRx;
using UnityEngine;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(StickyNoteView))]
    public class StickyNote : JankenBehaviour
    {
        private Hand.Shape shape = Hand.Shape.Paper;
        [SerializeField, Min(1)]
        private int atk = 1;

        private StickyNoteView stickyNoteView;

        [SerializeField]
        private StickyNoteSniperScope sniperScope;

        public void Setup(Vector2 viewport)
        {
            Create(new Hp(1), shape);

            events.AttackTriggerEvent
                .Subscribe(x => HandleAttackTrigger(x.id, x.receiveAttacks[0]))
                .AddTo(this);

            AttackableObjectCreateCommand command = new AttackableObjectCreateCommand(id, shape, new Atk(atk));
            AttackTrigger(command, gameManager.gameContains.player);

            stickyNoteView = GetComponent<StickyNoteView>();
            stickyNoteView.Setup(events, viewport, sniperScope);
        }

        private void HandleAttackTrigger(AttackableObjectId id, IReceiveAttack receiveAttack)
        {
            sniperScope.Setup(id, receiveAttack);
        }
    }
}
