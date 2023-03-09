using Ryocatusn.Janken.JankenableObjects;

namespace Ryocatusn.Janken.AttackableObjects
{
    public class AttackableObjectCreateCommand
    {
        public JankenableObjectId ownerId { get; }
        public Hand.Shape shape { get; }
        public Atk atk { get; }

        public AttackableObjectCreateCommand(JankenableObjectId ownerId, Hand.Shape shape, Atk atk)
        {
            this.ownerId = ownerId;
            this.shape = shape;
            this.atk = atk;
        }
    }
}
