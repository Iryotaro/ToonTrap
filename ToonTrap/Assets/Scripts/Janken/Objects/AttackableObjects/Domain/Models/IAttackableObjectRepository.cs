using Ryocatusn.Util;

namespace Ryocatusn.Janken.AttackableObjects
{
    public interface IAttackableObjectRepository
    {
        void Save(AttackableObject attackableObject);
        Option<AttackableObject> Find(AttackableObjectId id);
        void Delete(AttackableObjectId id);
    }
}
