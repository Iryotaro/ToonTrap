using Ryocatusn.Util;
using System.Collections.Generic;

namespace Ryocatusn.Games.Repository
{
    public class InMemoryGameRepository : IGameRepository
    {
        private List<Game> games = new List<Game>();

        public void Save(Game game)
        {
            games.Add(game);
        }
        public Option<Game> Find(GameId id)
        {
            return new Option<Game>(games.Find(x => x.id.Equals(id)));
        }
        public void Delete(GameId id)
        {
            games.RemoveAll(x => x.id.Equals(id));
        }
    }
}
