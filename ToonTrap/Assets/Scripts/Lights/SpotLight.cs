using UnityEngine;
using Ryocatusn.Util;
using UnityEngine.UI;

namespace Ryocatusn
{
    [ExecuteAlways()]
    public class SpotLight : MonoBehaviour
    {
        [Range(0, 1)]
        public float intencity;
        [Range(0, 1)]
        public float diff;
        [SerializeField]
        private Image outer;
        [SerializeField]
        private Image middle;
        [SerializeField]
        private Image inner;

        private void Update()
        {
            ColorHSV outerColorHSV = outer.color.GetColorHSV();
            outer.color = new ColorHSV(outerColorHSV.h, outerColorHSV.s, intencity - diff).GetColor();

            ColorHSV middleColorHSV = middle.color.GetColorHSV();
            middle.color = new ColorHSV(middleColorHSV.h, middleColorHSV.s, intencity).GetColor();

            ColorHSV innerColorHSV = outer.color.GetColorHSV();
            inner.color = new ColorHSV(innerColorHSV.h, innerColorHSV.s, intencity + diff).GetColor();
        }
    }
}
