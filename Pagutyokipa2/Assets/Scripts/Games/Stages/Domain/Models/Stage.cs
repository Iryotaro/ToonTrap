using System;
using UniRx;

namespace Ryocatusn.Games.Stages
{
    public class Stage : IEquatable<Stage>
    {
        public StageId id { get; }
        public StageName name { get; }
        public int countOfRetry { get; private set; } = 1;

        private Subject<Unit> clearEvent = new Subject<Unit>();
        private Subject<Unit> overEvent = new Subject<Unit>();

        public IObservable<Unit> ClearEvent => clearEvent;
        public IObservable<Unit> OverEvent => overEvent;

        public Stage(StageName name)
        {
            id = new StageId(Guid.NewGuid().ToString());
            this.name = name;
        }

        public void Clear()
        {
            clearEvent.OnNext(Unit.Default);
        }
        public void Over()
        {
            overEvent.OnNext(Unit.Default);
            countOfRetry++;
        }

        public void Delete()
        {
            clearEvent.Dispose();
            overEvent.Dispose();
        }

        public bool Equals(Stage other)
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
            return Equals((Stage)obj);
        }
        public override int GetHashCode()
        {
            return id != null ? id.GetHashCode() : 0;
        }
    }
}
