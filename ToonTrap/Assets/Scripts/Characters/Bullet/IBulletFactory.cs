using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.TileTransforms;
using UnityEngine;

namespace Ryocatusn.Characters
{
    public interface IBulletFactory
    {

        public Bullet Create(Bullet prefab, AttackableObjectId id, GameObject ownerObject, Vector2 position, Quaternion rotation);
        public Bullet Create(Bullet prefab, AttackableObjectId id, GameObject ownerObject, Vector2 position, Transform target);
        public Bullet Create(Bullet prefab, AttackableObjectId id, GameObject ownerObject, Vector2 position, TileDirection direction);
    }
}
