using Ryocatusn.Ryoseqs;
using Ryocatusn.Util;
using System;
using UnityEngine;

namespace Ryocatusn
{
    public class StagePresenter : MonoBehaviour
    {
        [SerializeField]
        private EnemiesAndNextRoad[] enemiesAndNextRoads;

        private Option<Ryoseq> stageRyoseq = new Option<Ryoseq>(null);

        private void Start()
        {
            NextTurn(1);
        }

        //Ryoseqの意味があんま無くて良くない
        private void NextTurn(int turn)
        {
            if (enemiesAndNextRoads.Length == 0) return;

            stageRyoseq.Set(new Ryoseq());
            stageRyoseq.Get().AddTo(this);

            ISequence sequence = stageRyoseq.Get().Create();

            sequence
            .ConnectWait(new SequenceWaitByActive(enemiesAndNextRoads[turn - 1].enemies))
            .Connect(new SequenceCommand(_ => AddRoad(enemiesAndNextRoads[turn - 1].roads)))
            .OnCompleted(() =>
            {
                stageRyoseq.Get().Delete();

                if (turn + 1 > enemiesAndNextRoads.Length) return;
                NextTurn(turn + 1);
            });

            stageRyoseq.Get().MoveNext();
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
