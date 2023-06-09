using System;

namespace Ryocatusn.Janken.AttackableObjects
{
    public class AttackableObjectId : IEquatable<AttackableObjectId>
    {
        private string value { get; }

        public AttackableObjectId(string value)
        {
            this.value = value;
        }

        public bool Equals(AttackableObjectId other)
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
            return Equals((AttackableObjectId)obj);
        }
        public override int GetHashCode()
        {
            return value != null ? value.GetHashCode() : 0;
        }
    }
}
