using System;

namespace Ryocatusn.Games
{
    public class GameException : Exception
    {
        public GameException(string message) : base("GameException : " + message) { }
        public GameException(string message, Exception innerException) : base("GameException : " + message, innerException) { }
    }
}
