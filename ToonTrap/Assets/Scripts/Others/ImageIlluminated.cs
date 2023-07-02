using UnityEngine;
using Zenject;
using UnityEngine.UI;
using Ryocatusn.Games;
using UnityEngine.Rendering.Universal;
using Ryocatusn.Util;

namespace Ryocatusn
{
    [RequireComponent(typeof(Image))]
    public class ImageIlluminated : MonoBehaviour
    {
        [Inject]
        private GameManager gameManager;

        private Image image;
        private Light2D globalLight;

        private ColorHSV defaultColorHSV;

        private void Start()
        {
            image = GetComponent<Image>();
            globalLight = gameManager.gameContains.globalLight;

            defaultColorHSV = image.color.GetColorHSV();
        }
        private void Update()
        {
            float colorV = GetColorV();
            ColorHSV colorHSV = new ColorHSV(defaultColorHSV.h, defaultColorHSV.s, colorV);
            image.color = colorHSV.GetColor();
        }

        private float GetColorV()
        {
            float maxV = defaultColorHSV.v;
            float minV = 0;

            return Mathf.Lerp(minV, maxV, globalLight.intensity);
        }
    }
}
