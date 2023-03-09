using UnityEngine;
using UnityEngine.Rendering.Universal;
using Ryocatusn.UI;
using Ryocatusn.Conversations;

namespace Ryocatusn
{
    public class GameContains
    {
        public Player player { get; }
        public ButtonMappingUI playerButtonMappingUI;
        public GameCamera gameCamera { get; }
        public GameBackground background { get; }
        public Light2D globalLight { get; }
        public AudioSource bgm { get; }
        public Conversation conversation { get; }

        public GameContains
            (
            Player player,
            ButtonMappingUI playerButtonMappingUI,
            GameCamera gameCamera,
            GameBackground background,
            Light2D globalLight, 
            AudioSource bgm,
            Conversation conversation
            )
        {
            this.player = player;
            this.playerButtonMappingUI = playerButtonMappingUI;
            this.gameCamera = gameCamera;
            this.background = background;
            this.globalLight = globalLight;
            this.bgm = bgm;
            this.conversation = conversation;
        }
    }
}
