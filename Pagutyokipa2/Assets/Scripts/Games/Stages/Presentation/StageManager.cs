using Microsoft.Extensions.DependencyInjection;
using Ryocatusn.Games.Stages;
using Ryocatusn.TileTransforms;
using Ryocatusn.Util;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Ryocatusn
{
    [DefaultExecutionOrder(-10)]
    public class StageManager : MonoBehaviour
    {
        public StageId id { get; private set; }
        private StageApplicationService stageApplicationService = Installer.installer.serviceProvider.GetService<StageApplicationService>();

        public Option<GameContains> gameContains { get; private set; } = new Option<GameContains>(null);

        public Tilemap[] roads;
        [SerializeField]
        private TileTransform startTransform;

        private BehaviorSubject<GameContains> setupStageEvent = new BehaviorSubject<GameContains>(null);

        public IObservable<GameContains> SetupStageEvent;

        private event Action UnloadEvent;

        public static StageManager activeStage { get; private set; }

        private void Start()
        {
            SetupStageEvent = setupStageEvent.Where(x => x != null).First();

            activeStage = this;

            SceneManager.sceneUnloaded += scene =>
            {
                if (!stageApplicationService.IsEnable(id)) return;
                if (scene.name != stageApplicationService.Get(id).name.value) return;

                UnloadEvent?.Invoke();

                if (UnloadEvent == null) return;
                foreach (Action action in UnloadEvent.GetInvocationList())
                {
                    UnloadEvent -= action;
                }
            };
        }
        private void OnDestroy()
        {
            activeStage = null;
        }

        public void SetupStage(StageId id, GameContains gameContains)
        {
            this.id = id;

            this.gameContains.Set(gameContains);
            SetupPlayer(this.gameContains.Get().player);

            setupStageEvent.OnNext(this.gameContains.Get());
        }

        private void SetupPlayer(Player player)
        {
            player.Init();

            TileTransform playerTransform = player.tileTransform;
            playerTransform.ChangeTilemap(roads, startTransform.tilePosition.Get().GetWorldPosition());
        }

        public void Clear()
        {
            stageApplicationService.Clear(id);
        }
        public void Over()
        {
            if (!stageApplicationService.IsEnable(id)) return;
            stageApplicationService.Over(id);
        }

        public void AddResetHandler(Action handleReset)
        {
            UnloadEvent += handleReset;
        }
    }
}
