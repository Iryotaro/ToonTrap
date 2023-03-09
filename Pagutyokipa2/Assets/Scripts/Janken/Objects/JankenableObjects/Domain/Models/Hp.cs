using System;
using Ryocatusn.Janken.AttackableObjects;

namespace Ryocatusn.Janken.JankenableObjects
{
    public class Hp : IEquatable<Hp>
    {
        public int value { get; }
        public int maxValue { get; }

        public Hp(int value, int? maxValue = null)
        {
            if (value < 0) throw new JankenException("0未満は許可されていません");
            if (maxValue != null && maxValue < 0) throw new JankenException("0未満は許可されていません");

            this.value = value;
            this.maxValue = maxValue ?? value;
        }

        public Hp Decrease(Atk atk)
        {
            int newValue = value - atk.value;

            return newValue <= 0 ? new Hp(0, maxValue) : new Hp(newValue, maxValue);
        }
        public Hp Increase(Hp addHp)
        {
            int newValue = value + addHp.value;

            return newValue > maxValue ? new Hp(maxValue, maxValue) : new Hp(newValue, maxValue);
        }
        public Hp Reset()
        {
            return new Hp(maxValue);
        }

        public bool Equals(Hp other)
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
            return Equals((Hp)obj);
        }
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
