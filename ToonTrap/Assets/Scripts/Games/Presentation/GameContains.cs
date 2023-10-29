using Ryocatusn.Characters;
using Ryocatusn.Conversations;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Ryocatusn.Lights;

namespace Ryocatusn
{
    public class GameContains
    {
        public Player player { get; }
        public PlayerBody playerBody { get; }
        public GameCamera gameCamera { get; }
        public GlobalLight globalLight { get; }
        public AudioSource bgm { get; }
        public Conversation conversation { get; }
        public Transition transition { get; }

        public GameContains
            (
            Player player,
            PlayerBody playerBody,
            GameCamera gameCamera,
            GlobalLight globalLight,
            AudioSource bgm,
            Conversation conversation,
            Transition transition
            )
        {
            this.player = player;
            this.playerBody = playerBody;
            this.gameCamera = gameCamera;
            this.globalLight = globalLight;
            this.bgm = bgm;
            this.conversation = conversation;
            this.transition = transition;
        }
    }
}
