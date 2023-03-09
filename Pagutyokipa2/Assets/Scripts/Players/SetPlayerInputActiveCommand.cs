namespace Ryocatusn
{
    public class SetPlayerInputActiveCommand
    {
        public bool? move { get; }
        public bool? attack { get; }
        public bool? rock { get; }
        public bool? scissors { get; }
        public bool? paper { get; }

        public SetPlayerInputActiveCommand(bool? move = null, bool? attack = null, bool? rock = null, bool? scissors = null, bool? paper = null)
        {
            this.move = move;
            this.attack = attack;
            this.rock = rock;
            this.scissors = scissors;
            this.paper = paper;
        }
    }
}
