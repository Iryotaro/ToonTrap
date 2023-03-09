using System.Collections.Generic;
using Ryocatusn.Util;

namespace Ryocatusn.Janken.Repository
{
    public class HandRepository : IHandRepository
    {
        private List<Hand> hands = new List<Hand>();

        public void Save(Hand hand)
        {
            hands.Add(hand);
        }
        public Option<Hand> Find(HandId id)
        {
            return new Option<Hand>(hands.Find(x => x.id.Equals(id)));
        }
        public void Delete(HandId id)
        {
            hands.RemoveAll(x => x.id.Equals(id));
        }
    }
}
