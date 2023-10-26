using Cysharp.Threading.Tasks;
using Ryocatusn.Games;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    public class AddRoadTrigger : MonoBehaviour
    {
        [Inject]
        private GameManager gameManager;
        [Inject]
        private StageManager stageManager;

        [SerializeField]
        private EnemiesAndNextRoad[] enemiesAndNextRoads;

        private void Start()
        {
            foreach (EnemiesAndNextRoad enemiesAndNextRoad in enemiesAndNextRoads)
            {
                int destroyCount = 0;
                foreach (GameObject enemy in enemiesAndNextRoad.enemies)
                {
                    enemy.OnDestroyAsObservable()
                        .Where(x => gameManager.gameContains.player != null)
                        .Subscribe(_ =>
                        {
                            destroyCount++;
                            if (destroyCount == enemiesAndNextRoad.enemies.Length)
                            {
                                AddRoad(enemiesAndNextRoad.roads);
                            }
                        })
                        .AddTo(this);
                }
            }
        }

        private void AddRoad(Road[] roads)
        {
            if (stageManager == null) return;

            foreach (Road road in roads)
            {
                if (road == null) continue;

                road.Appear();
            }
        }

        [Serializable]
        private class EnemiesAndNextRoad
        {
            public GameObject[] enemies;
            public Road[] roads;
        }
    }
}
