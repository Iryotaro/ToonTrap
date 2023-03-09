using System;

namespace Ryocatusn.Games
{
    public class Score : IEquatable<Score>
    {
        public int value { get; }

        public Score(int value)
        {
            if (value < 0) throw new GameException("0未満は許可されていません");

            this.value = value;
        }

        public Score Up(Score up)
        {
            return new Score(value + up.value);
        }
        public Score Down(Score down)
        {
            int newValue = value - down.value;

            return newValue <= 0 ? new Score(0) : new Score(newValue);
        }

        public bool Equals(Score other)
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
            return Equals((Score)obj);
        }
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
