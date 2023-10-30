using Ryocatusn.Conversations;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using Ryocatusn.Lights;

namespace Ryocatusn.Games
{
    public class OffLineGameManager : GameManager
    {
        [SerializeField]
        private Player player;
        [SerializeField]
        private PlayerBody playerBody;
        [SerializeField]
        private GameCamera gameCamera;
        [SerializeField]
        private GlobalLight globalLight;
        [SerializeField]
        private PlayerLight playerLight;
        [SerializeField]
        private PlayerBodyLight playerBodyLight;
        [SerializeField]
        private AudioSource bgm;
        [SerializeField]
        private Conversation conversation;
        [SerializeField]
        private Transition transition;
        
        private void Awake()
        {
            LightContains lightContains = new LightContains(globalLight, playerLight, playerBodyLight);
            GameContains gameContains = new GameContains(player, playerBody, gameCamera, lightContains, bgm, conversation, transition);
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
