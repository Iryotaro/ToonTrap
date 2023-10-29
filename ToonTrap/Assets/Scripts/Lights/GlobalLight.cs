using UnityEngine;
using Ryocatusn.Util;
using UnityEngine.UI;

namespace Ryocatusn.Lights
{
    [ExecuteAlways()]
    [RequireComponent(typeof(Image))]
    public class GlobalLight : MonoBehaviour
    {
        [Range(0, 1)]
        public float intencity;
        private Image image;

        private void Start()
        {
            image = GetComponent<Image>();
        }
        private void Update()
        {
            image.color = image.color.ChangeAlpha(intencity);
        }
    }
}
