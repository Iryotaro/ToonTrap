using System;

namespace Ryocatusn.Janken
{
    public class HandId : IEquatable<HandId>
    {
        private string value { get; }

        public HandId(string value)
        {
            this.value = value;
        }

        public bool Equals(HandId other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return Equals(other.value, value);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(obj, this)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals((HandId)obj);
        }
        public override int GetHashCode()
        {
            return value != null ? value.GetHashCode() : 0;
        }
    }
}
