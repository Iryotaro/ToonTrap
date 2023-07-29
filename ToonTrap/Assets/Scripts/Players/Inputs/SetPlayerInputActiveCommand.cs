namespace Ryocatusn
{
    public class SetPlayerInputActiveCommand
    {
        public bool? move { get; }
        public bool? attack { get; }
        public bool? changeShape { get; }

        public SetPlayerInputActiveCommand(bool? move = null, bool? attack = null, bool? changeShape = null)
        {
            this.move = move;
            this.attack = attack;
            this.changeShape = changeShape;
        }
    }
}
