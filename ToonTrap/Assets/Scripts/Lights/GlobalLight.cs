using UnityEngine;
using Ryocatusn.Util;
using UnityEngine.UI;
using DG.Tweening;

namespace Ryocatusn.Lights
{
    [ExecuteAlways()]
    [RequireComponent(typeof(Image))]
    public class GlobalLight : MonoBehaviour
    {
        public bool on = true;
        [Range(0, 1)]
        public float intensity;
        private Image image;

        private void Start()
        {
            image = GetComponent<Image>();
        }
        private void Update()
        {
            image.color = image.color.ChangeAlpha(intensity);

            if (on) image.enabled = true;
            else image.enabled = false;
        }

        public Tween DoChangeItencity(float endValue, float duration)
        {
            return DOTween.To
                (
                () => intensity,
                x => intensity = x,
                endValue,
                duration
                );
        }
    }
}
