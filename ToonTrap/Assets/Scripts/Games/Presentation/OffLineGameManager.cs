using Ryocatusn.Conversations;
using Ryocatusn.Games;
using Ryocatusn.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Ryocatusn
{
    public class OffLineGameManager : GameManager
    {
        [SerializeField]
        private Player player;
        [SerializeField]
        private ButtonMappingUI playerButtonMappingUI;
        [SerializeField]
        private GameCamera gameCamera;
        [SerializeField]
        private GameBackground background;
        [SerializeField]
        private Light2D globalLight;
        [SerializeField]
        private AudioSource bgm;
        [SerializeField]
        private Conversation conversation;
        
        private void Start()
        {
            GameContains gameContains = new GameContains(player, playerButtonMappingUI, gameCamera, background, globalLight, bgm, conversation);
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
