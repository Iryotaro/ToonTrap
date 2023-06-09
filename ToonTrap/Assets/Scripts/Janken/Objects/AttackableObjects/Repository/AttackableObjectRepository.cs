using Ryocatusn.Util;
using System.Collections.Generic;

namespace Ryocatusn.Janken.AttackableObjects.Repository
{
    public class AttackableObjectRepository : IAttackableObjectRepository
    {
        private List<AttackableObject> attackableObjects = new List<AttackableObject>();

        public void Save(AttackableObject attackableObject)
        {
            attackableObjects.Add(attackableObject);
        }
        public Option<AttackableObject> Find(AttackableObjectId id)
        {
            return new Option<AttackableObject>(attackableObjects.Find(x => x.id.Equals(id)));
        }
        public void Delete(AttackableObjectId id)
        {
            attackableObjects.RemoveAll(x => x.id.Equals(id));
        }
    }
}
