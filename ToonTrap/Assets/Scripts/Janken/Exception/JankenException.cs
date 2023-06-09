using System;

namespace Ryocatusn.Janken
{
    public class JankenException : Exception
    {
        public JankenException(string message) : base("JankenException : " + message) { }
        public JankenException(string message, Exception innerException) : base("JankenException : " + message, innerException) { }
    }
}
