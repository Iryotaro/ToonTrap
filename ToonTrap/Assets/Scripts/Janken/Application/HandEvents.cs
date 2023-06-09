using System;

namespace Ryocatusn.Janken
{
    public class HandEvents
    {
        public IObservable<Hand.Shape> ChangeShapeEvent { get; }
        public IObservable<bool> JankenReverseEvent { get; }

        public HandEvents(IObservable<Hand.Shape> ChangeShapeEvent, IObservable<bool> JankenReverseEvent)
        {
            this.ChangeShapeEvent = ChangeShapeEvent;
            this.JankenReverseEvent = JankenReverseEvent;
        }
    }
}
