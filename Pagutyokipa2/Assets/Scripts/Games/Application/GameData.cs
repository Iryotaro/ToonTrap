using Ryocatusn.Util;
using Ryocatusn.Games.Stages;

namespace Ryocatusn.Games
{
    public class GameData
    {
        public Option<StageId> stageId { get; }
        public StageId[] stageIds { get; }
        public Score score { get; }

        public GameData(Option<StageId> stageId, StageId[] stageIds, Score score)
        {
            this.stageId = stageId;
            this.stageIds = stageIds;
            this.score = score;
        }
    }
}
