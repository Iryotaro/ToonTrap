using Ryocatusn.Util;

namespace Ryocatusn.Games
{
    public interface IGameRepository
    {
        void Save(Game game);
        Option<Game> Find(GameId id);
        void Delete(GameId id);
    }
}
