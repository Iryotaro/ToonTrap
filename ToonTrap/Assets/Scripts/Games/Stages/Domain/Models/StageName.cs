using System;

namespace Ryocatusn.Games.Stages
{
    public class StageName : IEquatable<StageName>
    {
        public string value { get; }

        public StageName(string value)
        {
            this.value = value;
        }

        public bool Equals(StageName other)
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
            return Equals((StageName)obj);
        }
        public override int GetHashCode()
        {
            return value != null ? value.GetHashCode() : 0;
        }
    }
}
