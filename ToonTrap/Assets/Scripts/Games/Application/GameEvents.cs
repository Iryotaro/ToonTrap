using Ryocatusn.Games.Stages;
using Ryocatusn.Util;
using System;

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
