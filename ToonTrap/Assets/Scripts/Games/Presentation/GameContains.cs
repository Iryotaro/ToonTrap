using Ryocatusn.Conversations;
using UnityEngine;
using Ryocatusn.Lights;
using Ryocatusn.Characters;

namespace Ryocatusn
{
    public class GameContains
    {
        public Player player { get; }
        public PlayerBody playerBody { get; }
        public LightMan lightMan { get; }
        public GameCamera gameCamera { get; }
        public LightContains lightContains { get; }
        public AudioSource bgm { get; }
        public Conversation conversation { get; }
        public Transition transition { get; }

        public GameContains
            (
            Player player,
            PlayerBody playerBody,
            LightMan lightMan,
            GameCamera gameCamera,
            LightContains lightContains,
            AudioSource bgm,
            Conversation conversation,
            Transition transition
            )
        {
            this.player = player;
            this.playerBody = playerBody;
            this.lightMan = lightMan;
            this.gameCamera = gameCamera;
            this.lightContains = lightContains;
            this.bgm = bgm;
            this.conversation = conversation;
            this.transition = transition;
        }
    }
}
