using Ryocatusn.TileTransforms;
using System;
using UniRx;
using UnityEngine;

namespace Ryocatusn.StageCreaters
{
    public class Area : MonoBehaviour
    {
        public AreaStartJoint startJoint;
        public AreaEndJoint endJoint;

        private Subject<Area> endEvent = new Subject<Area>();
        public IObservable<Area> EndEvent => endEvent;

        private void Start()
        {
            endJoint.tileTransformTrigger
                .OnHitPlayerEvent
                .FirstOrDefault()
                .Subscribe(_ => endEvent.OnNext(this))
                .AddTo(this);
        }
        public void ChangePosition(TilePosition newStartPosition)
        {
            Vector2 nowPosition = startJoint.GetPositoin().GetWorldPosition();
            Vector2 newPosition = newStartPosition.GetWorldPosition();
            Vector2 differencePosition = newPosition - nowPosition;

            transform.position = (Vector2)transform.position + differencePosition;
        }
    }
}
