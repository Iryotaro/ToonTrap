using UnityEngine;
using Zenject;
using Ryocatusn.Games;
using FTRuntime;
using UnityEngine.Rendering.Universal;
using Ryocatusn.Util;

namespace Ryocatusn
{
    [RequireComponent(typeof(SwfClip))]
    public class SwfClipIlluminated : MonoBehaviour
    {
        [Inject]
        private GameManager gameManager;

        private SwfClip swfClip;
        private Light2D globalLight;

        private ColorHSV defaultColorHSV;

        private void Start()
        {
            swfClip = GetComponent<SwfClip>();
            globalLight = gameManager.gameContains.globalLight;

            defaultColorHSV = swfClip.tint.GetColorHSV();
        }
        private void Update()
        {
            float colorV = GetColorV();
            ColorHSV colorHSV = new ColorHSV(defaultColorHSV.h, defaultColorHSV.s, colorV);
            swfClip.tint = colorHSV.GetColor();
        }

        private float GetColorV()
        {
            float maxV = defaultColorHSV.v;
            float minV = 0;

            return Mathf.Lerp(minV, maxV, globalLight.intensity);
        }
    }
}
