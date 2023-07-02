using UnityEngine;

namespace Ryocatusn.Util
{
    public static class MyColor
    {
        public static ColorHSV GetColorHSV(this Color color)
        {
            float h;
            float s;
            float v;
            Color.RGBToHSV(color, out h, out s, out v);
            return new ColorHSV(h, s, v);
        }
        public static Color GetColor(this ColorHSV colorHSV)
        {
            return Color.HSVToRGB(colorHSV.h, colorHSV.s, colorHSV.v);
        }
    }
}
