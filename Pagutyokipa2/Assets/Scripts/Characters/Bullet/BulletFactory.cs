using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.TileTransforms;
using UnityEngine;

namespace Ryocatusn.Characters
{
    public static class BulletFactory
    {
        public static Bullet Create(Bullet prefab, AttackableObjectId id, GameObject ownerObject, Vector2 position, Quaternion rotation)
        {
            Bullet bullet = GameObject.Instantiate(prefab, position, rotation);
            bullet.SetUp(id, ownerObject);
            return bullet;
        }
        public static Bullet Create(Bullet prefab, AttackableObjectId id, GameObject ownerObject, Vector2 position, Transform target)
        {
            float angle = -90 + Mathf.Atan2(target.position.y - position.y, target.position.x - position.x) * Mathf.Rad2Deg;
            Bullet bullet = GameObject.Instantiate(prefab, position, Quaternion.Euler(0, 0, angle));
            bullet.SetUp(id, ownerObject);
            return bullet;
        }
        public static Bullet Create(Bullet prefab, AttackableObjectId id, GameObject ownerObject, Vector2 position, TileDirection.Direction direction)
        {
            Bullet bullet = GameObject.Instantiate(prefab, position, GetRotation());
            bullet.SetUp(id, ownerObject);
            return bullet;

            Quaternion GetRotation()
            {
                return direction switch
                {
                    TileDirection.Direction.Up => Quaternion.Euler(0, 0, 0),
                    TileDirection.Direction.Down => Quaternion.Euler(0, 0, 180),
                    TileDirection.Direction.Left => Quaternion.Euler(0, 0, 90),
                    TileDirection.Direction.Right => Quaternion.Euler(0, 0, -90),
                    _ => Quaternion.Euler(0, 0, 0)
                };
            }
        }
    }
}
