using System;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    public class AddRoadTrigger : MonoBehaviour
    {
        [Inject]
        private StageManager stageManager;

        [SerializeField]
        private EnemiesAndNextRoad[] enemiesAndNextRoads;
        
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
