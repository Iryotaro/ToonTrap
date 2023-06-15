using FTRuntime;
using System;
using UniRx;
using UnityEngine;

namespace Ryocatusn.Characters
{
    public abstract class TunnelAnimator : MonoBehaviour
    {
        protected Subject<Unit> createLocomotiveEvent = new Subject<Unit>();

        public IObservable<Unit> CreateLocomotiveEvent => createLocomotiveEvent;

        public void OnDestroy()
        {
            createLocomotiveEvent.Dispose();
        }

        public abstract void ChangeRateScale(float rateScale);
    }
}
