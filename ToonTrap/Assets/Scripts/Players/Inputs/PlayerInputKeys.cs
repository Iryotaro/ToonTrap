using UnityEngine.InputSystem;

namespace Ryocatusn
{
    public class PlayerInputKeys
    {
        public string moveUpPath { get; }
        public string moveDownPath { get; }
        public string moveLeftPath { get; }
        public string moveRightPath { get; }

        public Key[] rockKeys { get; }
        public Key[] scissorsKeys { get; }

        public PlayerInputKeys(string moveUpPath, string moveDownPath, string moveLeftPath, string moveRightPath, Key[] rockKeys, Key[] scissorsKeys)
        {
            this.moveUpPath = moveUpPath;
            this.moveDownPath = moveDownPath;
            this.moveLeftPath = moveLeftPath;
            this.moveRightPath = moveRightPath;

            this.rockKeys = rockKeys;
            this.scissorsKeys = scissorsKeys;
        }
    }
}
