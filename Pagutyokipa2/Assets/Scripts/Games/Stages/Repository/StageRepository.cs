using Ryocatusn.Util;
using System.Collections.Generic;

namespace Ryocatusn.Games.Stages.Repository
{
    public class StageRepository : IStageRepository
    {
        private List<Stage> stages = new List<Stage>();

        public void Save(Stage stage)
        {
            stages.Add(stage);
        }
        public Option<Stage> Find(StageId id)
        {
            return new Option<Stage>(stages.Find(x => x.id.Equals(id)));
        }
        public void Delete(StageId id)
        {
            stages.RemoveAll(x => x.id.Equals(id));
        }
    }
}
