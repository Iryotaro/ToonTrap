namespace Ryocatusn.Games.Stages
{
    public class StageApplicationService
    {
        private IStageRepository stageRepository;

        public StageApplicationService(IStageRepository stageRepository)
        {
            this.stageRepository = stageRepository;
        }

        public StageId Create(StageName name)
        {
            Stage stage = new Stage(name);
            stageRepository.Save(stage);

            return stage.id;
        }
        public StageData Get(StageId id)
        {
            Stage stage = stageRepository.Find(id).Get();
            if (stage == null) throw new StageException("Stageが見つかりません");

            return new StageData
                (
                stage.name,
                stage.countOfRetry
                );
        }
        public StageEvents GetEvents(StageId id)
        {
            Stage stage = stageRepository.Find(id).Get();
            if (stage == null) throw new StageException("Stageが見つかりません");

            return new StageEvents
                (
                stage.ClearEvent,
                stage.OverEvent
                );
        }
        public void Clear(StageId id)
        {
            stageRepository.Find(id).Match
                (
                Some: x => x.Clear(),
                None: () => throw new StageException("Stageが見つかりません")
                );
        }
        public void Over(StageId id)
        {
            stageRepository.Find(id).Match
                (
                Some: x => x.Over(),
                None: () => throw new StageException("Stageが見つかりません")
                );
        }
        public bool IsEnable(StageId id)
        {
            return stageRepository.Find(id).Get() != null;
        }
        public void Delete(StageId id)
        {
            stageRepository.Find(id).Match
                (
                Some: x =>
                {
                    x.Delete();
                    stageRepository.Delete(id);
                }
                );
        }
    }
}
