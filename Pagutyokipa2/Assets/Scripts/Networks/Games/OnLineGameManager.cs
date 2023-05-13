using UnityEngine;
using Ryocatusn.UI;
using UnityEngine.Rendering.Universal;
using Ryocatusn.Conversations;
using UniRx;
using Ryocatusn.Util;
using Ryocatusn.Games.Stages;

namespace Ryocatusn.Games
{
    public class OnLineGameManager : GameManager
    {
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

        public void StartGame(Player player)
        {
            GameContains gameContains = new GameContains(player, playerButtonMappingUI, gameCamera, background, globalLight, bgm, conversation);
            Create(gameContains);

            events.NextSceneEvent
                .Subscribe(x => HandleNextSceneEvent(x.prevStageId, x.nextStageId))
                .AddTo(this);

            events.ClearEvent
                .Subscribe(x => HandleClearEvent(x))
                .AddTo(this);

            events.OverEvent
                .Subscribe(x => HandleOverEvent(x))
                .AddTo(this);

            Setup();
        }

        private void HandleNextSceneEvent(Option<StageId> prevStageId, StageId nextStageId)
        {
            MoveToNextStage(prevStageId, nextStageId);
        }
        private void HandleClearEvent(StageId finalStageId)
        {

            MoveToClear(finalStageId);
        }
        private void HandleOverEvent(StageId finalStageId)
        {

            MoveToOver(finalStageId);
        }
    }
}
