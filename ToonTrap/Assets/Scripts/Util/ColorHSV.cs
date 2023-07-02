namespace Ryocatusn.Util
{
    public struct ColorHSV
    {
        public float h { get; }
        public float s { get; }
        public float v { get; }

        public ColorHSV(float h, float s, float v)
        {
            this.h = h;
            this.s = s;
            this.v = v;
        }
    }
}
