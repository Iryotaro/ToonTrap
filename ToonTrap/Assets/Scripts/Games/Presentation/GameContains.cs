using Ryocatusn.Characters;
using Ryocatusn.Lights;
using UnityEngine;

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
        public WeatherEffects weatherEffects { get; }
        public Transition transition { get; }

        public GameContains
            (
            Player player,
            PlayerBody playerBody,
            LightMan lightMan,
            GameCamera gameCamera,
            LightContains lightContains,
            AudioSource bgm,
            WeatherEffects weatherEffects,
            Transition transition
            )
        {
            this.player = player;
            this.playerBody = playerBody;
            this.lightMan = lightMan;
            this.gameCamera = gameCamera;
            this.lightContains = lightContains;
            this.bgm = bgm;
            this.weatherEffects = weatherEffects;
            this.transition = transition;
        }
    }
}
