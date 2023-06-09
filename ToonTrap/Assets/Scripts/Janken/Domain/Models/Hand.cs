using System;
using UniRx;

namespace Ryocatusn.Janken
{
    public class Hand : IEquatable<Hand>
    {
        public HandId id { get; }
        public Shape shape { get; private set; }

        public static bool reverse { private set; get; } = false;

        private Subject<Shape> changeShapeEvent = new Subject<Shape>();
        private static Subject<bool> jankenReverseEvent = new Subject<bool>();

        public IObservable<Shape> ChangeShapeEvent => changeShapeEvent;
        public static IObservable<bool> JankenReverseEvent => jankenReverseEvent;

        public enum Shape
        {
            Rock,
            Scissors,
            Paper
        }

        public Hand(Shape shape)
        {
            id = new HandId(Guid.NewGuid().ToString());
            this.shape = shape;
        }
        public void ChangeShape(Shape shape)
        {
            this.shape = shape;

            changeShapeEvent.OnNext(shape);
        }

        public void Delete()
        {
            changeShapeEvent.Dispose();
        }

        public static void JankenReverse(bool reverse)
        {
            if (Hand.reverse == reverse) return;

            Hand.reverse = reverse;
            jankenReverseEvent.OnNext(reverse);
        }

        public bool Equals(Hand other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return Equals(id, other.id);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(obj, this)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals((Hand)obj);
        }
        public override int GetHashCode()
        {
            return id != null ? id.GetHashCode() : 0;
        }
    }
}
