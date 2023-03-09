using Ryocatusn.Util;

namespace Ryocatusn.Janken.JankenableObjects
{
    public interface IJankenableObjectRepository
    {
        void Save(JankenableObject jankenableObject);
        Option<JankenableObject> Find(JankenableObjectId id);
        void Delete(JankenableObjectId id);
    }
}
