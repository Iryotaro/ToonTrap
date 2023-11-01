using FTRuntime;
using UnityEngine;

namespace Ryocatusn.Lights
{
    [ExecuteAlways]
    [RequireComponent(typeof(SwfClipController))]
    [RequireComponent(typeof(MeshRenderer))]
    public class SwfClipLight : MonoBehaviour
    {
        [SerializeField]
        private SwfClipController targetSwfClipController;
        [Range(0f, 1f)]
        public float intensity;
        public bool on = true;

        private SwfClipController swfClipController;
        private MeshRenderer meshRenderer;

        private void Start()
        {
            swfClipController = GetComponent<SwfClipController>();
            meshRenderer = GetComponent<MeshRenderer>();
        }
        private void LateUpdate()
        {
            Color color = swfClipController.clip.tint;
            swfClipController.clip.tint = new Color(color.r, color.g, color.b, intensity);

            transform.position = targetSwfClipController.transform.position;
            transform.localScale = targetSwfClipController.transform.localScale;
            transform.rotation = targetSwfClipController.transform.rotation;

            if (on)
            {
                swfClipController.clip.clip = targetSwfClipController.clip.clip;
                swfClipController.clip.currentFrame = targetSwfClipController.clip.currentFrame;
            }
            else
            {
                meshRenderer.enabled = false;
            }
        }
    }
}
