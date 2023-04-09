using Microsoft.Extensions.DependencyInjection;
using Ryocatusn.Conversations;
using Ryocatusn.Games;
using Ryocatusn.Games.Stages;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Ryoseqs;
using Ryocatusn.UI;
using Ryocatusn.Util;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Ryocatusn
{
    public class GameManager : MonoBehaviour
    {
        public GameId id { get; private set; }
        private Option<StageId> nowStageId = new Option<StageId>(null);
        private GameApplicationService gameApplicationService = Installer.installer.serviceProvider.GetService<GameApplicationService>();
        private StageApplicationService stageApplicationService = Installer.installer.serviceProvider.GetService<StageApplicationService>();
        private JankenableObjectApplicationService jankenableObjectApplicationService = Installer.installer.serviceProvider.GetService<JankenableObjectApplicationService>();

        private InputMaster input;

        [SerializeField]
        private Player player;
        [SerializeField]
        private ButtonMappingUI buttonMappingUI;
        [SerializeField]
        private GameCamera gameCamera;
        [SerializeField]
        private Light2D globalLight;
        [SerializeField]
        private GameBackground background;
        [SerializeField]
        private AudioSource bgm;
        [SerializeField]
        private Conversation conversation;
        [SerializeField]
        private string[] stageNames;

        public GameContains gameContains { get; private set; }

        private Subject<StageManager> setStageEvent = new Subject<StageManager>();

        public IObservable<StageManager> SetStageEvent => setStageEvent;

        private void Start()
        {

#if UNITY_EDITOR
            PlayerPrefs.DeleteAll();
#endif

            gameContains = new GameContains(player, buttonMappingUI, gameCamera, background, globalLight, bgm, conversation);

            id = gameApplicationService.Create(CreateStage(GetStageNames()));

            GameEvents events = gameApplicationService.GetEvents(id);

            events.NextSceneEvent
                .Subscribe(x => HandleNextStage(x.prevStageId, x.nextStageId))
                .AddTo(this);

            events.ClearEvent
                .Subscribe(x => HandleClear(x))
                .AddTo(this);

            events.OverEvent
                .Subscribe(x => HandleOver(x))
                .AddTo(this);

            gameApplicationService.Start(id);

            DG.Tweening.DOTween.SetTweensCapacity(800, 200);
        }
        private void OnDestroy()
        {
            gameApplicationService.Delete(id);
            setStageEvent.Dispose();
        }

        private StageId[] CreateStage(string[] stageNames)
        {
            return stageNames.Select(x => stageApplicationService.Create(new StageName(x))).ToArray();
        }

        private void HandleNextStage(Option<StageId> prevStageId, StageId nextStageId)
        {
            nowStageId.Set(nextStageId);
            StageData nextStageData = stageApplicationService.Get(nextStageId);

            SaveStageName(nextStageData.name);

            prevStageId.Match
                (
                Some: x =>
                {
                    player.inputMaster.SetActiveAll(false);

                    StageData prevStageData = prevStageData = stageApplicationService.Get(x);

                    Hand.Shape shape = jankenableObjectApplicationService.Get(player.id).shape;
                    Transition.LoadScene(prevStageData.name.value, nextStageData.name.value, new TransitionSettings(player.transform, gameCamera.camera, shape), () =>
                    {
                        SetupStage(nextStageId, gameContains);
                        player.inputMaster.SetActiveAll(true);
                    });
                },
                None: () =>
                {
                    SceneManager.LoadScene(nextStageData.name.value, LoadSceneMode.Additive);
                    SetupStage(nextStageId, gameContains);
                    player.inputMaster.SetActiveAll(true);
                });
        }
        private void HandleClear(StageId finalStageId)
        {
            player.inputMaster.SetActiveAll(false);

            StageData finalStageData = stageApplicationService.Get(finalStageId);

            SaveStageName(new StageName(stageNames[0]));

            Hand.Shape shape = jankenableObjectApplicationService.Get(player.id).shape;
            Transition.LoadScene(new string[] { finalStageData.name.value, "Game" }, new string[] { "Clear" }, new TransitionSettings(player.transform, gameCamera.camera, shape));
        }
        private void HandleOver(StageId stageId)
        {
            Ryoseq ryoseq = new Ryoseq();
            ISequence sequence = ryoseq.Create();

            sequence
                .Connect(new SequenceCommand(_ => player.inputMaster.SetActiveAll(false)))
                .ConnectWait(new SequenceWaitForSeconds(3))
                .Connect(new SequenceCommand(_ =>
                {
                    StageData stageData = stageApplicationService.Get(stageId);

                    SceneManager.UnloadSceneAsync(stageData.name.value);
                    SceneManager.LoadScene(stageData.name.value, LoadSceneMode.Additive);

                    SetupStage(stageId, gameContains);

                    player.inputMaster.SetActiveAll(true);
                }))
                .OnCompleted(() => ryoseq.Kill(sequence));

            ryoseq.MoveNext();
        }

        private void SetupStage(StageId stageId, GameContains gameContains)
        {
            //LoadScene直後だとGetRootGameObjectsで取得できない
            Observable.NextFrame().Subscribe(x =>
            {
                Scene scene = SceneManager.GetSceneByName(stageApplicationService.Get(stageId).name.value);

                foreach (GameObject rootGameObject in scene.GetRootGameObjects())
                {
                    if (rootGameObject.TryGetComponent(out StageManager stageManager))
                    {
                        stageManager.SetupStage(stageId, gameContains);
                        setStageEvent.OnNext(stageManager);

                        return;
                    }
                }
                throw new GameException("StageSceneはStageManagerをルートオブジェクトにアタッチしてください");
            });
        }

        private string[] GetStageNames()
        {
            string nowStageName = PlayerPrefs.GetString("SaveStage", stageNames[0]);
            return stageNames.Where(x => Array.IndexOf(stageNames, nowStageName) <= Array.IndexOf(stageNames, x)).ToArray();
        }
        private void SaveStageName(StageName stageName)
        {
            if (stageName.value == "Settings") return;
            PlayerPrefs.SetString("SaveStage", stageName.value);
            PlayerPrefs.Save();
        }

        private void Awake()
        {
            input = new InputMaster();
        }
        private void OnEnable()
        {
            input.Game.Enable();
        }
        private void OnDisable()
        {
            input.Game.Disable();
        }
    }
}
