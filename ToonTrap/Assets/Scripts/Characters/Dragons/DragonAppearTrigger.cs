using UnityEngine;
using UniRx;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class DragonAppearTrigger : MonoBehaviour
    {
        [SerializeField]
        private Dragon dragon;

        private void Start()
        {
            TileTransformTrigger trigger = GetComponent<TileTransformTrigger>();

            trigger.OnHitPlayerEvent
                .First()
                .Subscribe(_ => dragon.Appear())
                .AddTo(this);
        }
    }
}
