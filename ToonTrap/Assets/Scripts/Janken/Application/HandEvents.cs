using System;

namespace Ryocatusn.Janken
{
    public class HandEvents
    {
        public IObservable<Hand.Shape> ChangeShapeEvent { get; }

        public HandEvents(IObservable<Hand.Shape> ChangeShapeEvent)
        {
            this.ChangeShapeEvent = ChangeShapeEvent;
        }
    }
}
