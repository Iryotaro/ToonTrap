namespace Ryocatusn.Lights
{
    public class LightContains
    {
        public GlobalLight globalLight { get; }
        public PlayerLight playerLight { get; }
        public SwfClipLight playerBodyLight { get; }

        public LightContains
            (
            GlobalLight globalLight,
            PlayerLight playerLight,
            SwfClipLight playerBodyLight
            )
        {
            this.globalLight = globalLight;
            this.playerLight = playerLight;
            this.playerBodyLight = playerBodyLight;
        }
    }
}
