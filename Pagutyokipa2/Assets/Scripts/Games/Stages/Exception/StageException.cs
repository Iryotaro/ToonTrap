using System;

namespace Ryocatusn.Games.Stages
{
    public class StageException : Exception
    {
        public StageException(string message) : base("StageException : " + message) { }
        public StageException(string message, Exception innerException) : base("StageException : " + message, innerException) { }
    }
}
