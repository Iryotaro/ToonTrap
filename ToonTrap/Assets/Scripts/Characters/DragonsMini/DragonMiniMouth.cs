using System;
using UniRx;
using UnityEngine;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(Animator))]
    public class DragonMiniMouth : MonoBehaviour
    {
        public Transform shotPoint;

        private Animator animator;

        private Subject<Unit> attackTriggerEvent = new Subject<Unit>();

        public IObservable<Unit> AttackTriggerEvent => attackTriggerEvent;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        private void OnDestroy()
        {
            attackTriggerEvent.Dispose();
        }

        public void StartAttackAnimation()
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Nothing")) return;
            animator.SetTrigger("Attack");
        }
        public void HandleAttackTriggerAnimationEvent()
        {
            attackTriggerEvent.OnNext(Unit.Default);
        }
    }
}
