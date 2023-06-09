using System;
using UniRx;

namespace Ryocatusn.Games.Stages
{
    public class StageEvents
    {
        public IObservable<Unit> ClearEvent { get; }
        public IObservable<Unit> OverEvent { get; }

        public StageEvents(IObservable<Unit> ClearEvent, IObservable<Unit> OverEvent)
        {
            this.ClearEvent = ClearEvent;
            this.OverEvent = OverEvent;
        }
    }
}
