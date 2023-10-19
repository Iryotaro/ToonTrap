using Ryocatusn.Conversations;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using Ryocatusn.Characters;

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
        private Light2D globalLight;
        [SerializeField]
        private AudioSource bgm;
        [SerializeField]
        private Conversation conversation;
        [SerializeField]
        private Transition transition;
        
        private void Awake()
        {
            GameContains gameContains = new GameContains(player, playerBody, gameCamera, globalLight, bgm, conversation, transition);
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
