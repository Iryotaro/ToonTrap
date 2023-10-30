using FTRuntime;
using UnityEngine;

namespace Ryocatusn.Lights
{
    [ExecuteAlways]
    [RequireComponent(typeof(SwfClipController))]
    [RequireComponent(typeof(MeshRenderer))]
    public class PlayerBodyLight : MonoBehaviour
    {
        [SerializeField]
        private SwfClipController playerBodySwfClipController;
        public bool on = true;

        private SwfClipController swfClipController;
        private MeshRenderer meshRenderer;

        private void Start()
        {
            swfClipController = GetComponent<SwfClipController>();
            meshRenderer = GetComponent<MeshRenderer>();
        }
        private void Update()
        {
            transform.position = playerBodySwfClipController.transform.position;
            transform.localScale = playerBodySwfClipController.transform.localScale;

            if (on)
            {
                meshRenderer.enabled = true;
                swfClipController.clip.clip = playerBodySwfClipController.clip.clip;
                swfClipController.clip.currentFrame = playerBodySwfClipController.clip.currentFrame;
            }
            else
            {
                meshRenderer.enabled = false;
            }
        }
    }
}
