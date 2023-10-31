using UnityEngine;

namespace Ryocatusn.Lights
{
    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteLight : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer targetSpriteRenderer;
        [Range(0f, 1f)]
        public float intensity;
        public bool on = true;

        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            Color color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, intensity);

            transform.position = targetSpriteRenderer.transform.position;
            transform.localScale = targetSpriteRenderer.transform.localScale;
            transform.rotation = targetSpriteRenderer.transform.rotation;

            spriteRenderer.sprite = targetSpriteRenderer.sprite;

            if (on)
            {
                spriteRenderer.enabled = targetSpriteRenderer.enabled;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }
}
