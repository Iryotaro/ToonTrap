using System.Linq;
using UniRx;
using Ryocatusn.Games.Stages;

namespace Ryocatusn.Games
{
    public class GameApplicationService
    {
        private IGameRepository gameRepository { get; }
        private StageApplicationService stageApplicationService { get; }

        public GameApplicationService(IGameRepository gameRepository, StageApplicationService stageApplicationService)
        {
            this.gameRepository = gameRepository;
            this.stageApplicationService = stageApplicationService;
        }

        public GameId Create(StageId[] stageIds)
        {
            Game game = new Game(stageIds);
            gameRepository.Save(game);

            if (stageIds.Count() != stageIds.Distinct().ToArray().Count()) throw new GameException("同じStageIdは含めません");

            foreach (StageId stageId in stageIds)
            {
                StageEvents events = stageApplicationService.GetEvents(stageId);

                events.ClearEvent.Subscribe(_ => game.NextStage());
                events.OverEvent.Subscribe(_ => game.Over(stageId));
            }
            
            return game.id;
        }
        public void Start(GameId id)
        {
            gameRepository.Find(id).Match
                (
                Some: x => x.Start(),
                None: () => throw new GameException("Gameが見つかりません")
                );
        }
        public GameData GetData(GameId id)
        {
            Game game = gameRepository.Find(id).Get();
            if (game == null) throw new GameException("Gameが見つかりません");

            return new GameData
                (
                game.stageId,
                game.stageIds,
                game.score
                );
        }
        public GameEvents GetEvents(GameId id)
        {
            Game game = gameRepository.Find(id).Get();
            if (game == null) throw new GameException("Gameが見つかりません");

            return new GameEvents
                (
                game.NextStageEvent,
                game.ClearEvent,
                game.OverEvent
                );
        }
        public void Delete(GameId id)
        {
            gameRepository.Find(id).Match
                (
                Some: x =>
                {
                    x.Delete();
                    gameRepository.Delete(id);

                    foreach (StageId stageId in x.stageIds)
                    {
                        stageApplicationService.Delete(stageId);
                    }
                },
                None: () => throw new GameException("Gameが見つかりません")
                );
        }
    }
}
