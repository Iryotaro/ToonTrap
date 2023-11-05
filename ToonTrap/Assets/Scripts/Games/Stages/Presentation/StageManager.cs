using Ryocatusn.Games.Stages;
using Ryocatusn.TileTransforms;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField]
        private StageStartPresenter stageStartPresenter;

        [NonSerialized]
        public List<Tilemap> roads = new List<Tilemap>();

        private BehaviorSubject<GameContains> setupStageEvent = new BehaviorSubject<GameContains>(null);
        private BehaviorSubject<Tilemap[]> changeRoadEvent = new BehaviorSubject<Tilemap[]>(new Tilemap[] {  });
        private Subject<Unit> overEvent = new Subject<Unit>();
        private Subject<Unit> clearEvent = new Subject<Unit>();

        public IObservable<GameContains> SetupStageEvent;
        public IObservable<Tilemap[]> ChangeRoadEvent => changeRoadEvent;
        public IObservable<Unit> OverEvent => overEvent;
        public IObservable<Unit> ClearEvent => clearEvent;

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
            changeRoadEvent.Dispose();
            overEvent.Dispose();
            clearEvent.Dispose();
        }

        public void SetupStage(StageId id, GameContains gameContains)
        {
            this.id = id;

            Action finish = () => { setupStageEvent.OnNext(gameContains); };

            Vector2 startPosition = startTransform.tilePosition.Get().GetWorldPosition();
            stageStartPresenter.Play(startPosition, firstRoad, finish);

            gameContains.player.isAllowedToReceiveAttack = false;
            gameContains.player.inputMaster.SetActiveAll(false);
            SetupStageEvent
                .Subscribe(_ =>
                {
                    gameContains.player.isAllowedToReceiveAttack = true;
                    gameContains.player.inputMaster.SetActiveAll(true);
                })
                .AddTo(this);
        }

        public StageData GetData()
        {
            return stageApplicationService.Get(id);
        }

        public void Clear()
        {
            stageApplicationService.Clear(id);
            clearEvent.OnNext(Unit.Default);
        }
        public void Over()
        {
            if (!stageApplicationService.IsEnable(id)) return;
            stageApplicationService.Over(id);
            overEvent.OnNext(Unit.Default);
        }

        public void AddRoad(Tilemap tilemap)
        {
            SetupStageEvent
                .Subscribe(gameContains =>
                {
                    roads.Add(tilemap);
                    gameContains.player.tileTransform.AddTilemap(tilemap);
                    changeRoadEvent.OnNext(roads.ToArray());
                })
                .AddTo(this);
        }
        public void RemoveRoad(Tilemap tilemap)
        {
            SetupStageEvent
                .Subscribe(gameContains =>
                {
                    roads.Remove(tilemap);
                    gameContains.player.tileTransform.ChangeTilemap(roads.ToArray(), gameContains.player.transform.position);
                    changeRoadEvent.OnNext(roads.ToArray());
                })
                .AddTo(this);
        }
    }
}
