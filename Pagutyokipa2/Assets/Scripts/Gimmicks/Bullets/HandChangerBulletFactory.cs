using Microsoft.Extensions.DependencyInjection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ryocatusn.Games.Stages;

namespace Ryocatusn
{
    public class HandChangerBulletFactory
    {
        private HandChangerBullet prefab { get; }
        private (Transform transform, IReceiveAttack receiveAttack) target { get; }
        private Transform ownTransform { get; }

        public HandChangerBulletFactory(HandChangerBullet prefab, (Transform transform, IReceiveAttack receiveAttack) target, Transform ownTransform)
        {
            this.prefab = prefab;
            this.target = target;
            this.ownTransform = ownTransform;
        }
        public HandChangerBullet Create()
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

            HandChangerBullet handChangerBullet = GameObject.Instantiate(prefab, ownTransform.position, Quaternion.identity);
            handChangerBullet.transform.parent = parent.transform;

            handChangerBullet.Set(target);

            return handChangerBullet;
        }
    }
}
