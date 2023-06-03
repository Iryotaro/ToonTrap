using Ryocatusn.Games.Stages;
using Ryocatusn.TileTransforms;
using System;
using System.Collections.Generic;
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

        [SerializeField]
        private Tilemap firstRoad;
        [SerializeField]
        private TileTransform startTransform;

        [NonSerialized]
        public List<Tilemap> roads = new List<Tilemap>();

        private BehaviorSubject<GameContains> setupStageEvent = new BehaviorSubject<GameContains>(null);
        private BehaviorSubject<Tilemap[]> addRoadEvent = new BehaviorSubject<Tilemap[]>(default);

        public IObservable<GameContains> SetupStageEvent;
        public IObservable<Tilemap[]> AddRoadEvent => addRoadEvent;

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

            SetupPlayer(gameContains.player);

            setupStageEvent.OnNext(gameContains);
        }

        private void SetupPlayer(Player player)
        {
            player.Init();

            TileTransform playerTransform = player.tileTransform;
            playerTransform.ChangeTilemap(new Tilemap[] { firstRoad }, startTransform.tilePosition.Get().GetWorldPosition());
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

        public void AddRoad(Tilemap tilemap)
        {
            SetupStageEvent
                .Subscribe(gameContains =>
                {
                    roads.Add(tilemap);
                    gameContains.player.tileTransform.AddTilemap(tilemap);
                    addRoadEvent.OnNext(roads.ToArray());
                })
                .AddTo(gameObject);
        }
    }
}
