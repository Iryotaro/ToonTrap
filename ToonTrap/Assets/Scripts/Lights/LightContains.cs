namespace Ryocatusn.Lights
{
    public class LightContains
    {
        public GlobalLight globalLight { get; }
        public PlayerLight playerLight { get; }

        public LightContains
            (
            GlobalLight globalLight,
            PlayerLight playerLight
            )
        {
            this.globalLight = globalLight;
            this.playerLight = playerLight;
        }
    }
}
