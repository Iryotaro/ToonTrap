using Ryocatusn.Conversations;
using Ryocatusn.UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Ryocatusn
{
    public class GameContains
    {
        public Player player { get; }
        public GameCamera gameCamera { get; }
        public Light2D globalLight { get; }
        public AudioSource bgm { get; }
        public Conversation conversation { get; }

        public GameContains
            (
            Player player,
            GameCamera gameCamera,
            Light2D globalLight,
            AudioSource bgm,
            Conversation conversation
            )
        {
            this.player = player;
            this.gameCamera = gameCamera;
            this.globalLight = globalLight;
            this.bgm = bgm;
            this.conversation = conversation;
        }
    }
}
