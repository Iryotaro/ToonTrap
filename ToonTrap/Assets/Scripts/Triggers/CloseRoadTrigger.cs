using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class CloseRoadTrigger : MonoBehaviour
    {
        [Inject]
        private StageManager stageManager;

        [SerializeField]
        private Road[] roads;

        private void Start()
        {
            GetComponent<TileTransformTrigger>().OnHitPlayerEvent
                .FirstOrDefault()
                .Subscribe(_ => CloseRoads(roads))
                .AddTo(this);
        }

        private void CloseRoads(Road[] roads)
        {
            if (stageManager == null) return;

            foreach (Road road in roads)
            {
                if (road == null) continue;

                road.Disappear();
            }
        }
    }
}
