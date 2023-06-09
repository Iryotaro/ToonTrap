using Ryocatusn.Janken.JankenableObjects;

namespace Ryocatusn.Janken.AttackableObjects
{
    public class AttackableObjectData
    {
        public JankenableObjectId ownerId { get; }
        public Hand.Shape shape { get; }
        public HandId handId { get; }
        public Atk atk { get; }
        public bool allowedReAttack { get; }

        public AttackableObjectData(JankenableObjectId ownerId, Hand.Shape shape, HandId handId, Atk atk, bool allowedReAttack)
        {
            this.ownerId = ownerId;
            this.shape = shape;
            this.handId = handId;
            this.atk = atk;
            this.allowedReAttack = allowedReAttack;
        }
    }
}
