using Ryocatusn.Games.Stages;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Util;
using System;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Ryocatusn.Games
{
    public class GameManager : MonoBehaviour
    {
        public GameId id { get; private set; }
        private Option<StageId> nowStageId = new Option<StageId>(null);
        [Inject]
        private GameApplicationService gameApplicationService { get; }
        [Inject]
        private StageApplicationService stageApplicationService { get; }
        [Inject]
        private JankenableObjectApplicationService jankenableObjectApplicationService { get; }
        protected GameEvents events;

        [Inject]
        private DiContainer diContainer;

        public GameContains gameContains { get; private set; }
        public StageManager nowStageManager { get; private set; }

        [SerializeField]
        private string mainName = "Game";
        [SerializeField]
        private string[] stageNames;

        private Subject<StageManager> setStageEvent = new Subject<StageManager>();

        public IObservable<StageManager> SetStageEvent => setStageEvent;

        protected void Create(GameContains gameContains)
        {
            this.gameContains = gameContains;

            id = gameApplicationService.Create(CreateStage(stageNames));

            events = gameApplicationService.GetEvents(id);

            this.OnDestroyAsObservable()
                .Subscribe(_ => 
                {
                    gameApplicationService.Delete(id);
                    setStageEvent.Dispose();
                });
        }

        protected void Setup()
        {
            diContainer.InjectGameObject(gameContains.player.gameObject);
            gameContains.player.Setup();
            gameApplicationService.Start(id);
        }

        private StageId[] CreateStage(string[] stageNames)
        {
            return stageNames.Select(x => stageApplicationService.Create(new StageName(x))).ToArray();
        }

        protected void MoveToNextStage(Option<StageId> prevStageId, StageId nextStageId)
        {
            nowStageId.Set(nextStageId);
            StageData nextStageData = stageApplicationService.Get(nextStageId);

            prevStageId.Match
                (
                Some: x =>
                {
                    gameContains.player.inputMaster.SetActiveAll(false);

                    StageData prevStageData = prevStageData = stageApplicationService.Get(x);

                    Hand.Shape shape = jankenableObjectApplicationService.Get(gameContains.player.id).shape;
                    Transition.LoadScene(prevStageData.name.value, nextStageData.name.value, new TransitionSettings(gameContains.player.transform, gameContains.gameCamera.camera, shape), () =>
                    {
                        SetupStage(nextStageId, gameContains);
                        gameContains.player.inputMaster.SetActiveAll(true);
                    });
                },
                None: () =>
                {
                    SceneManager.LoadScene(nextStageData.name.value, LoadSceneMode.Additive);
                    SetupStage(nextStageId, gameContains);
                    gameContains.player.inputMaster.SetActiveAll(true);
                });
        }
        protected void MoveToClear(StageId finalStageId)
        {
            MoveToScene(finalStageId, gameContains.player, "Clear");
        }
        protected void MoveToOver(StageId finalStageId)
        {
            MoveToScene(finalStageId, gameContains.player, "Over");
        }
        protected void ResetStage(StageId stageId)
        {
            gameContains.player.inputMaster.SetActiveAll(false);

            StageData stageData = stageApplicationService.Get(stageId);

            SceneManager.UnloadSceneAsync(stageData.name.value);
            SceneManager.LoadScene(stageData.name.value, LoadSceneMode.Additive);

            SetupStage(stageId, gameContains);

            gameContains.player.inputMaster.SetActiveAll(true);
        }

        private void SetupStage(StageId stageId, GameContains gameContains)
        {
            gameContains.player.inputMaster.SetActiveAll(false);

            //LoadScene直後だとGetRootGameObjectsで取得できない
            Observable.NextFrame().Subscribe(x =>
            {
                Scene scene = SceneManager.GetSceneByName(stageApplicationService.Get(stageId).name.value);

                foreach (GameObject rootGameObject in scene.GetRootGameObjects())
                {
                    if (rootGameObject.TryGetComponent(out StageManager stageManager))
                    {
                        nowStageManager = stageManager;
                        stageManager.SetupStage(stageId, gameContains);
                        setStageEvent.OnNext(stageManager);

                        return;
                    }
                }
                throw new GameException("StageSceneはStageManagerをルートオブジェクトにアタッチしてください");
            });
        }
        private void MoveToScene(StageId finalStageId, Player player, string sceneName)
        {
            player.inputMaster.SetActiveAll(false);

            StageData finalStageData = stageApplicationService.Get(finalStageId);

            Hand.Shape shape = jankenableObjectApplicationService.Get(player.id).shape;
            Transition.LoadScene(new string[] { finalStageData.name.value, mainName }, new string[] { sceneName }, new TransitionSettings(player.transform, gameContains.gameCamera.camera, shape));
        }
    }
}
