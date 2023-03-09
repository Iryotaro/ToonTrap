using System;
using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ryocatusn.TileTransforms;
using Ryocatusn.Games.Stages;
using Ryocatusn.Janken.AttackableObjects;

namespace Ryocatusn
{
    public class BulletFactory
    {
        private Bullet prefab { get; }
        private AttackableObjectId id { get; }
        private BulletParameter parameter { get; }
        private Transform ownerTransform { get; }
        private Quaternion rotation { get; }
        private IReceiveAttack target { get; }

        public BulletFactory(Bullet prefab, AttackableObjectId id, BulletParameter parameter, Transform ownerTransform, TileDirection direction, IReceiveAttack target = null)
        {
            Quaternion rotation = direction.value switch
            {
                TileDirection.Direction.Up => Quaternion.Euler(0, 0, 0),
                TileDirection.Direction.Down => Quaternion.Euler(0, 0, 180),
                TileDirection.Direction.Left => Quaternion.Euler(0, 0, 90),
                TileDirection.Direction.Right => Quaternion.Euler(0, 0, -90),
                _ => throw new Exception("想定外の方向です")
            };

            this.prefab = prefab;
            this.id = id;
            this.parameter = parameter;
            this.ownerTransform = ownerTransform;
            this.rotation = rotation;
            this.target = target;
        }
        public BulletFactory(Bullet prefab, AttackableObjectId id, BulletParameter parameter, Transform ownerTransform, Quaternion rotation, IReceiveAttack target = null)
        {
            this.prefab = prefab;
            this.id = id;
            this.parameter = parameter;
            this.ownerTransform = ownerTransform;
            this.rotation = rotation;
            this.target = target;
        }

        public Bullet Create()
        {
            string stageName = null;
            if (StageManager.activeStage != null)
            {
                StageApplicationService stageApplicationService
                    = Installer.installer.serviceProvider.GetService<StageApplicationService>();
                stageName = stageApplicationService.Get(StageManager.activeStage.id).name.value;
            }
            GameObject parent = GameObject.Find("Bullets");
            if (parent == null) parent = new GameObject() { name = "Bullets" };
            if (stageName != null) SceneManager.MoveGameObjectToScene(parent, SceneManager.GetSceneByName(stageName));

            Bullet bullet = GameObject.Instantiate(prefab, ownerTransform.position, rotation);
            bullet.transform.parent = parent.transform;

            bullet.Set(id, ownerTransform, parameter, target);

            return bullet;
        }
    }
}
