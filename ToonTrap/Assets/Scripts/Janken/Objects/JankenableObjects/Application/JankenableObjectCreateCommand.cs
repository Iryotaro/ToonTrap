using Ryocatusn.Games.Stages;

namespace Ryocatusn.Janken.JankenableObjects
{
    public class JankenableObjectCreateCommand
    {
        public Hp hp { get; }
        public InvincibleTime invincibleTime { get; }
        public Hand.Shape shape { get; }
        public StageId stageId { get; }

        public JankenableObjectCreateCommand(Hp hp, InvincibleTime invincibleTime, Hand.Shape shape, StageId stageId = null)
        {
            this.hp = hp;
            this.invincibleTime = invincibleTime;
            this.shape = shape;
            this.stageId = stageId;
        }
        public JankenableObjectCreateCommand(Hp hp, Hand.Shape shape, StageId stageId = null)
        {
            this.hp = hp;
            invincibleTime = null;
            this.shape = shape;
            this.stageId = stageId;
        }
    }
}
