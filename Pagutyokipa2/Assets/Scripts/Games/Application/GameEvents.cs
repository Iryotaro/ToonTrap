using System;
using UniRx;
using Ryocatusn.Util;
using Ryocatusn.Games.Stages;

namespace Ryocatusn.Games
{
    public class GameEvents
    {
        public IObservable<(Option<StageId> prevStageId, StageId nextStageId)> NextSceneEvent { get; }
        public IObservable<StageId> ClearEvent { get; }
        public IObservable<StageId> OverEvent { get; }

        public GameEvents(IObservable<(Option<StageId>, StageId)> NextSceneEvent, IObservable<StageId> ClearEvent, IObservable<StageId> OverEvent)
        {
            this.NextSceneEvent = NextSceneEvent;
            this.ClearEvent = ClearEvent;
            this.OverEvent = OverEvent;
        }
    }
}
