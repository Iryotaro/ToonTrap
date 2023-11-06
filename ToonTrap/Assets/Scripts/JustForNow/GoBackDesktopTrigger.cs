using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class GoBackDesktopTrigger : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<TileTransformTrigger>().OnHitPlayerEvent
                .Subscribe(_ => Quit())
                .AddTo(this);
        }

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
