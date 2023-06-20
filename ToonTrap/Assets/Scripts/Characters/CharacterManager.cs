using System.Collections.Generic;
using Ryocatusn.Janken.JankenableObjects;

namespace Ryocatusn.Characters
{
    public class CharacterManager
    {
        private List<JankenBehaviour> jankenBehaviours = new List<JankenBehaviour>();

        public void Save(JankenBehaviour jankenBehaviour)
        {
            jankenBehaviours.Add(jankenBehaviour);
        }
        public JankenBehaviour Find(JankenableObjectId id)
        {
            return jankenBehaviours.Find(x => x.id.Equals(id));
        }
        public void Delete(JankenableObjectId id)
        {
            jankenBehaviours.RemoveAll(x => x.id.Equals(id));
        }
    }
}
