namespace Ryocatusn.Janken.JankenableObjects
{
    public class JankenableObjectData
    {
        public Hand.Shape shape { get; }
        public HandId handId { get; }
        public Hp hp { get; }
        public int winCombo { get; }

        public JankenableObjectData(Hand.Shape shape, HandId handId, Hp hp, int winCombo)
        {
            this.shape = shape;
            this.handId = handId;
            this.hp = hp;
            this.winCombo = winCombo;
        }
    }
}
