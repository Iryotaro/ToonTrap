using Ryocatusn.Util;
using System.Collections.Generic;

using Photon;

namespace Ryocatusn.Janken.JankenableObjects.Repository
{
    public class JankenableObjectRepository : IJankenableObjectRepository
    {
        private List<JankenableObject> jankenableObjects = new List<JankenableObject>();

        public void Save(JankenableObject jankenableObject)
        {
            jankenableObjects.Add(jankenableObject);
        }
        public Option<JankenableObject> Find(JankenableObjectId id)
        {
            return new Option<JankenableObject>(jankenableObjects.Find(x => x.id.Equals(id)));
        }
        public void Delete(JankenableObjectId id)
        {
            jankenableObjects.RemoveAll(x => x.id.Equals(id));
        }
    }

    public class JankenableObjectNetworkRepository : IJankenableObjectRepository
    {
        private List<JankenableObject> jankenableObjects = new List<JankenableObject>();

        public void Save(JankenableObject jankenableObject)
        {
            jankenableObjects.Add(jankenableObject);
        }
        public Option<JankenableObject> Find(JankenableObjectId id)
        {
            return new Option<JankenableObject>(jankenableObjects.Find(x => x.id.Equals(id)));
        }
        public void Delete(JankenableObjectId id)
        {
            jankenableObjects.RemoveAll(x => x.id.Equals(id));
        }
    }
}
