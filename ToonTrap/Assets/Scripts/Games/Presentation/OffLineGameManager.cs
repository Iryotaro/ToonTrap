using Cysharp.Threading.Tasks;
using Ryocatusn.Characters;
using Ryocatusn.Lights;
using UniRx;
using UnityEngine;

namespace Ryocatusn.Games
{
    public class OffLineGameManager : GameManager
    {
        [SerializeField]
        private Player player;
        [SerializeField]
        private PlayerBody playerBody;
        [SerializeField]
        private LightMan lightMan;
        [SerializeField]
        private GameCamera gameCamera;
        [SerializeField]
        private GlobalLight globalLight;
        [SerializeField]
        private PlayerLight playerLight;
        [SerializeField]
        private AudioSource bgm;
        [SerializeField]
        private WeatherEffects weatherEffects;
        [SerializeField]
        private Transition transition;

        private void Awake()
        {
            LightContains lightContains = new LightContains(globalLight, playerLight);
            GameContains gameContains = new GameContains(player, playerBody, lightMan, gameCamera, lightContains, bgm, weatherEffects, transition);
            Create(gameContains);

            events.NextSceneEvent
                .Subscribe(x => MoveToNextStage(x.prevStageId, x.nextStageId))
                .AddTo(this);

            events.ClearEvent
                .Subscribe(x => MoveToClear(x))
                .AddTo(this);

            events.OverEvent
                .Subscribe(x => ResetStage(x))
                .AddTo(this);

            Setup();
        }
    }
}
