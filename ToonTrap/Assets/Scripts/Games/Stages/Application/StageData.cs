namespace Ryocatusn.Games.Stages
{
    public class StageData
    {
        public StageName name { get; }
        public int countOfRetry { get; }

        public StageData(StageName name, int countOfRetry)
        {
            this.name = name;
            this.countOfRetry = countOfRetry;
        }
    }
}
