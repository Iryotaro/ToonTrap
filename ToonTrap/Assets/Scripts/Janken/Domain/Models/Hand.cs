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

        public IObservable<Shape> ChangeShapeEvent => changeShapeEvent;

        private static Shape[] sequence = new Shape[3] { Shape.Rock, Shape.Scissors, Shape.Paper };
        public static Shape[] rsp { get; } = new Shape[3] { Shape.Rock, Shape.Scissors, Shape.Paper };
        public static Shape[] rps { get; } = new Shape[3] { Shape.Rock, Shape.Paper, Shape.Scissors };

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

        public static void SetSequence(Shape[] sequence)
        {
            Hand.sequence = sequence;
        }
        public static Shape[] GetSequence()
        {
            return sequence;
        }
        public static Shape GetNextShape(Shape shape)
        {
            int index = (Array.IndexOf(sequence, shape) + 1) % 3;
            return sequence[index];
        }
        public static Shape GetRandomShape()
        {
            return (Shape)UnityEngine.Random.Range(0, 3);
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
