using Ryocatusn.Util;

namespace Ryocatusn.Games.Stages
{
    public interface IStageRepository
    {
        void Save(Stage stage);
        Option<Stage> Find(StageId id);
        void Delete(StageId id);
    }
}
