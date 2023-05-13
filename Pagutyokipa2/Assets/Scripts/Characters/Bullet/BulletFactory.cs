using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.TileTransforms;
using UnityEngine;
using Zenject;

namespace Ryocatusn.Characters
{
    public class BulletFactory : IBulletFactory
    {
        [Inject]
        private DiContainer diContainer;

        public Bullet Create(Bullet prefab, AttackableObjectId id, GameObject ownerObject, Vector2 position, Quaternion rotation)
        {
            Bullet bullet = GameObject.Instantiate(prefab, position, rotation);
            diContainer.InjectGameObject(bullet.gameObject);
            bullet.SetUp(id, ownerObject);
            return bullet;
        }
        public Bullet Create(Bullet prefab, AttackableObjectId id, GameObject ownerObject, Vector2 position, Transform target)
        {
            float angle = -90 + Mathf.Atan2(target.position.y - position.y, target.position.x - position.x) * Mathf.Rad2Deg;
            Bullet bullet = GameObject.Instantiate(prefab, position, Quaternion.Euler(0, 0, angle));
            diContainer.InjectGameObject(bullet.gameObject);
            bullet.SetUp(id, ownerObject);
            return bullet;
        }
        public Bullet Create(Bullet prefab, AttackableObjectId id, GameObject ownerObject, Vector2 position, TileDirection direction)
        {
            Bullet bullet = GameObject.Instantiate(prefab, position, direction.GetRotation());
            diContainer.InjectGameObject(bullet.gameObject);
            bullet.SetUp(id, ownerObject);
            return bullet;
        }
    }
}
