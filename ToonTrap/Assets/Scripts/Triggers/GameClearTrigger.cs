using UniRx;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class GameClearTrigger : MonoBehaviour
    {
        [Inject]
        private StageManager stageManager;

        private void Start()
        {
            GetComponent<TileTransformTrigger>().OnHitPlayerEvent
                .FirstOrDefault()
                .Subscribe(_ => stageManager.Clear())
                .AddTo(this);
        }
    }
}
