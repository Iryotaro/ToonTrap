using Zenject;
using UnityEngine;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.TileTransforms;
using Photon.Pun;

namespace Ryocatusn.Characters
{
    public class BulletNetworkFactory : IBulletFactory
    {
        [Inject]
        private DiContainer diContainer;

        public Bullet Create(Bullet prefab, AttackableObjectId id, GameObject ownerObject, Vector2 position, Quaternion rotation)
        {
            Bullet bullet = PhotonNetwork.Instantiate(prefab.name, position, rotation).GetComponent<Bullet>();
            diContainer.InjectGameObject(bullet.gameObject);
            bullet.SetUp(id, ownerObject);
            return bullet;
        }
        public Bullet Create(Bullet prefab, AttackableObjectId id, GameObject ownerObject, Vector2 position, Transform target)
        {
            float angle = -90 + Mathf.Atan2(target.position.y - position.y, target.position.x - position.x) * Mathf.Rad2Deg;
            Bullet bullet = PhotonNetwork.Instantiate(prefab.name, position, Quaternion.Euler(0, 0, angle)).GetComponent<Bullet>();
            diContainer.InjectGameObject(bullet.gameObject);
            bullet.SetUp(id, ownerObject);
            return bullet;
        }
        public Bullet Create(Bullet prefab, AttackableObjectId id, GameObject ownerObject, Vector2 position, TileDirection direction)
        {
            Bullet bullet = PhotonNetwork.Instantiate(prefab.name, position, direction.GetRotation()).GetComponent<Bullet>();
            diContainer.InjectGameObject(bullet.gameObject);
            bullet.SetUp(id, ownerObject);
            return bullet;
        }
    }
}
