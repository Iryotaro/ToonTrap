using FTRuntime;
using UnityEngine;

namespace Ryocatusn
{
    [RequireComponent(typeof(SwfClipController))]
    public class Effect : MonoBehaviour
    {
        private SwfClipController swfClipController;

        private void Start()
        {
            swfClipController = GetComponent<SwfClipController>();

            swfClipController.OnRewindPlayingEvent += Delete;
        }
        private void OnDestroy()
        {
            swfClipController.OnRewindPlayingEvent -= Delete;
        }

        private void Delete(SwfClipController controller)
        {
            Destroy(gameObject);
        }
    }
}
