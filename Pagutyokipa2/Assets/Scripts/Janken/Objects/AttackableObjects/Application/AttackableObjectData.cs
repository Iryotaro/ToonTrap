using Ryocatusn.Janken.JankenableObjects;

namespace Ryocatusn.Janken.AttackableObjects
{
    public class AttackableObjectData
    {
        public JankenableObjectId ownerId { get; }
        public HandId handId { get; }
        public Atk atk { get; }
        public bool allowedReAttack { get; }

        public AttackableObjectData(JankenableObjectId ownerId, HandId handId, Atk atk, bool allowedReAttack)
        {
            this.ownerId = ownerId;
            this.handId = handId;
            this.atk = atk;
            this.allowedReAttack = allowedReAttack;
        }
    }
}
