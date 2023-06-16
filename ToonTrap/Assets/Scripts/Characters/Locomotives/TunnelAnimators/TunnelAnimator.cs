using System;
using UniRx;
using UnityEngine;

namespace Ryocatusn.Characters
{
    public abstract class TunnelAnimator : MonoBehaviour
    {
        protected Subject<Unit> createLocomotiveEvent = new Subject<Unit>();

        public IObservable<Unit> CreateLocomotiveEvent => createLocomotiveEvent;

        protected virtual void OnDestroy()
        {
            createLocomotiveEvent.Dispose();
        }

        public abstract void ChangeRateScale(float rateScale);
    }
}
