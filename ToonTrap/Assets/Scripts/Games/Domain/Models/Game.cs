using Ryocatusn.Games.Stages;
using Ryocatusn.Util;
using System;
using UniRx;

namespace Ryocatusn.Games
{
    public class Game : IEquatable<Game>
    {
        public GameId id { get; }
        public Option<StageId> stageId = new Option<StageId>(null);
        public StageId[] stageIds { get; private set; }
        public Score score { get; private set; } = new Score(0);

        private Subject<(Option<StageId> prevStageId, StageId nextStageId)> setStageEvent = new Subject<(Option<StageId>, StageId)>();
        private Subject<StageId> clearEvent = new Subject<StageId>();
        private Subject<StageId> overEvent = new Subject<StageId>();

        public IObservable<(Option<StageId> prevStageId, StageId nextStageId)> NextStageEvent => setStageEvent;
        public IObservable<StageId> ClearEvent => clearEvent;
        public IObservable<StageId> OverEvent => overEvent;

        public Game(StageId[] stageIds)
        {
            id = new GameId(Guid.NewGuid().ToString());
            this.stageIds = stageIds;
        }

        public void Start()
        {
            stageId.Match
                (
                Some: _ => throw new GameException("既にStartされています"),
                None: () => stageId.Set(stageIds[0])
                );

            setStageEvent.OnNext((new Option<StageId>(null), stageId.Get()));
        }
        public void NextStage()
        {
            StageId prevStageId = null;
            stageId.Match
                (
                Some: x =>
                {
                    prevStageId = x;
                    stageId.SetInArrayIndex(stageIds, Array.IndexOf(stageIds, x) + 1);
                });

            stageId.Match
                (
                Some: x => setStageEvent.OnNext((new Option<StageId>(prevStageId), x)),
                None: () => clearEvent.OnNext(prevStageId)
                );
        }
        public void Over(StageId stageId)
        {
            overEvent.OnNext(stageId);
        }

        public void UpScore(Score up)
        {
            score = score.Up(up);
        }
        public void DownScore(Score down)
        {
            score = score.Down(down);
        }

        public void Delete()
        {
            setStageEvent.Dispose();
            clearEvent.Dispose();
            overEvent.Dispose();
        }

        public bool Equals(Game other)
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
            return Equals((Game)obj);
        }
        public override int GetHashCode()
        {
            return id != null ? id.GetHashCode() : 0;
        }
    }
}
