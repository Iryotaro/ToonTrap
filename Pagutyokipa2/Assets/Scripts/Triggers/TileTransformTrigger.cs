using Ryocatusn.Games;
using Ryocatusn.TileTransforms;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransform))]
    public class TileTransformTrigger : MonoBehaviour
    {
        [Inject]
        private GameManager gameManager;
        [Inject]
        private StageManager stageManager;

        private TileTransform tileTransform;
        private TileTransform playerTileTransform;

        private bool hit = false;

        private Subject<Unit> onHitPlayerEvent = new Subject<Unit>();
        private Subject<TileDirection.Direction> onTriggerExitPlayerEvent = new Subject<TileDirection.Direction>();

        public IObservable<Unit> OnHitPlayerEvent => onHitPlayerEvent;
        public IObservable<TileDirection.Direction> OnTriggerExitPlayerEvent => onTriggerExitPlayerEvent;

        private void Start()
        {
            tileTransform = GetComponent<TileTransform>();

            tileTransform.ChangeTilemap(RoadManager.instance.GetTilemaps(), transform.position);
            playerTileTransform = gameManager.gameContains.player.tileTransform;

            stageManager.AddRoadEvent
                .Subscribe(x => tileTransform.ChangeTilemap(x, transform.position))
                .AddTo(this);
        }
        private void OnDestroy()
        {
            onHitPlayerEvent.Dispose();
            onTriggerExitPlayerEvent.Dispose();
        }

        private void Update()
        {
            bool hit = IsHit();
            if (hit && !this.hit) onHitPlayerEvent.OnNext(Unit.Default);
            if (!hit && this.hit) OnTriggerExitToPlayer();
            this.hit = hit;
        }

        private bool IsHit()
        {
            if (!IsEnableTransform(tileTransform) || !IsEnableTransform(playerTileTransform)) return false;

            TilePosition tilePosition = tileTransform.tilePosition.Get();
            TilePosition playerTilePosition = playerTileTransform.tilePosition.Get();

            if (tilePosition.IsSamePlace(playerTilePosition)) return true;
            return false;
        }
        private void OnTriggerExitToPlayer()
        {
            if (!IsEnableTransform(tileTransform) || !IsEnableTransform(playerTileTransform)) return;

            foreach (TileDirection.Direction direction in Enum.GetValues(typeof(TileDirection.Direction)))
            {
                TilePosition aroundTilePositoin
                    = tileTransform.tilePosition.Get().GetAroundTilePosition(new TileDirection(direction));

                if (aroundTilePositoin == null) continue;

                if (playerTileTransform.tilePosition.Get().IsSamePlace(aroundTilePositoin))
                {
                    onTriggerExitPlayerEvent.OnNext(direction);
                }
            }
        }

        private bool IsEnableTransform(TileTransform tileTransform)
        {
            if (tileTransform == null) return false;

            TilePosition tilePosition = tileTransform.tilePosition.Get();

            if (tilePosition == null) return false;

            if (tilePosition.IsOutsideRoad()) return false;

            return true;
        }
    }
}
