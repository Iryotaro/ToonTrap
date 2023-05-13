using Ryocatusn.Games.Stages;
using Ryocatusn.TileTransforms;
using Ryocatusn.Util;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Zenject;

namespace Ryocatusn
{
    [DefaultExecutionOrder(-10)]
    public class StageManager : MonoBehaviour
    {
        public StageId id { get; private set; }
        [Inject]
        private StageApplicationService stageApplicationService { get; }

        public Option<GameContains> gameContains { get; private set; } = new Option<GameContains>(null);

        public Tilemap[] roads;
        [SerializeField]
        private TileTransform startTransform;

        private BehaviorSubject<GameContains> setupStageEvent = new BehaviorSubject<GameContains>(null);

        public IObservable<GameContains> SetupStageEvent;

        private void Start()
        {
            SetupStageEvent = setupStageEvent.Where(x => x != null).First();

            SceneManager.sceneUnloaded += scene =>
            {
                if (!stageApplicationService.IsEnable(id)) return;
                if (scene.name != stageApplicationService.Get(id).name.value) return;
            };
        }
        private void OnDestroy()
        {
            setupStageEvent.Dispose();
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
    }
}
