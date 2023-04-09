using Cysharp.Threading.Tasks;
using Ryocatusn.Ryoseqs;
using Ryocatusn.TileTransforms;
using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Ryocatusn
{
    public class SequenceWaitByTileTransform : SequenceWaitCommand
    {
        private TileTransform target;
        private TileTransform[] positions;

        public SequenceWaitByTileTransform(TileTransform target, TileTransform position)
        {
            this.target = target;
            this.positions = new TileTransform[] { position };

            handler = new TaskHandler(Finish =>
            {
                Start(Finish).Forget();
            });
        }
        public SequenceWaitByTileTransform(TileTransform target, TileTransform[] positions)
        {
            this.target = target;
            this.positions = positions;

            handler = new TaskHandler(Finish =>
            {
                Start(Finish).Forget();
            });
        }

        private async UniTaskVoid Start(Action onFinish)
        {
            await Wait(onFinish);
        }

        private IEnumerator Wait(Action onFinish)
        {
            yield return new WaitUntil(() =>
            {
                foreach (TileTransform position in positions)
                {
                    bool isHit = IsHit(target, position);
                    if (isHit) return true;
                }
                return false;
            });
            onFinish();
        }

        private bool IsHit(TileTransform target, TileTransform position)
        {
            if (!IsEnableTransform(position) || !IsEnableTransform(target)) return false;

            TilePosition tilePosition = position.tilePosition.Get();
            TilePosition playerTilePosition = target.tilePosition.Get();

            if (tilePosition.IsSamePlace(playerTilePosition)) return true;
            return false;
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
