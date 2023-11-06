using System;
using UniRx;
using UnityEngine;

namespace Ryocatusn.Characters
{
    public class Frame : MonoBehaviour
    {
        public float interval;

        private Subject<Unit> showSubject = new Subject<Unit>();
        public IObservable<Unit> ShowSubject => showSubject;

        public void OnDestroy()
        {
            showSubject.Dispose();
        }

        public void Show()
        {
            showSubject.OnNext(Unit.Default);
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
