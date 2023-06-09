using Ryocatusn.Util;

namespace Ryocatusn.Janken
{
    public interface IHandRepository
    {
        void Save(Hand hand);
        Option<Hand> Find(HandId id);
        void Delete(HandId id);
    }
}
