using System;
using UnityEngine;

namespace Ryocatusn
{
    public class AddRoadTrigger : MonoBehaviour
    {
        [SerializeField]
        private EnemiesAndNextRoad[] enemiesAndNextRoads;

        private void Update()
        {

        }

        private void AddRoad(Road[] roads)
        {
            if (StageManager.activeStage == null) return;

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
