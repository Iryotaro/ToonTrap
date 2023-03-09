using UniRx;
using UnityEngine;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class GameClearTrigger : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<TileTransformTrigger>().OnHitPlayerEvent
                .FirstOrDefault()
                .Subscribe(_ => StageManager.activeStage.Clear())
                .AddTo(this);
        }
    }
}
