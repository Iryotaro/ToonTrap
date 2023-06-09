using System;

namespace Ryocatusn.Janken.AttackableObjects
{
    public class Atk : IEquatable<Atk>
    {
        public int value { get; }

        public Atk(int value)
        {
            if (value < 0) throw new JankenException("0未満は許可されていません");

            this.value = value;
        }

        public bool Equals(Atk other)
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
            return Equals((Atk)obj);
        }
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
