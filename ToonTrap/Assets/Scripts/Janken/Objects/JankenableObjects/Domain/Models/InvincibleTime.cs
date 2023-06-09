using System;

namespace Ryocatusn.Janken.JankenableObjects
{
    public class InvincibleTime : IEquatable<InvincibleTime>
    {
        public float value { get; }

        public InvincibleTime(float value)
        {
            if (value < 0) throw new JankenException("0未満は許可されていません");

            this.value = value;
        }

        public bool Equals(InvincibleTime other)
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
            return Equals((InvincibleTime)obj);
        }
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
