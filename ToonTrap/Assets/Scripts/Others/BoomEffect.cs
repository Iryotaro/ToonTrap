using FTRuntime;
using UnityEngine;

namespace Ryocatusn
{
    [RequireComponent(typeof(SwfClipController))]
    public class BoomEffect : MonoBehaviour
    {
        private SwfClipController swfClipController;

        private void Start()
        {
            swfClipController = GetComponent<SwfClipController>();

            swfClipController.OnRewindPlayingEvent += Delete;
        }

        private void Delete(SwfClipController swfClipController)
        {
            Destroy(gameObject);
            swfClipController.OnRewindPlayingEvent -= Delete;
        }
    }
}
