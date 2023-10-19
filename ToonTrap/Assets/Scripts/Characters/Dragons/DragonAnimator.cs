using System;
using UnityEngine;
using UniRx;
using Ryocatusn.Games;

namespace Ryocatusn.Characters
{
    public class DragonAnimator : MonoBehaviour
    {
        [SerializeField]
        private FrameAnimator firstAppearance;
        [SerializeField]
        private FrameAnimator provocation;
        [SerializeField]
        private FrameAnimator appear;
        [SerializeField]
        private FrameAnimator disappear;
        [SerializeField]
        private FrameAnimator attack1;
        [SerializeField]
        private FrameAnimator damage;

        [SerializeField]
        private Frame attackFrame;

        private Subject<Unit> attackSubject = new Subject<Unit>();

        public IObservable<Unit> AttackSubject => attackSubject;

        public enum AnimationType
        {
            FirstAppearance,
            Provocation,
            Appear,
            Disappear,
            Attack1,
            Damage
        }

        private void Start()
        {
            attackFrame.ShowSubject
                .Subscribe(_ => attackSubject.OnNext(Unit.Default))
                .AddTo(this);
        }
        private void OnDestroy()
        {
            attackSubject.Dispose();
        }

        public void ShowAppearanceFirstFrame()
        {
            HideAllFrames();
            FrameAnimator frameAnimator = GetAnimator(AnimationType.FirstAppearance);
            frameAnimator.ShowSpecificFrame(0);
        }
        public FrameAnimator PlayAnimation(AnimationType animationType)
        {
            FrameAnimator frameAnimator = GetAnimator(animationType);
            HideAllFrames();
            frameAnimator.Play();
            return frameAnimator;
        }
        public void PlayAnimations(AnimationType[] animationTypes, int index = 0)
        {
            if (animationTypes.Length <= index) return;
            
            AnimationType animationType = animationTypes[index];
            FrameAnimator animator = PlayAnimation(animationType);
            animator.CompleteSubjet
                .Subscribe(_ => PlayAnimations(animationTypes, index + 1))
                .AddTo(this);
        }
        private void HideAllFrames()
        {
            foreach (AnimationType type in Enum.GetValues(typeof(AnimationType)))
            {
                GetAnimator(type).HideAllFrames();
            }
        }
        private FrameAnimator GetAnimator(AnimationType animationType)
        {
            return animationType switch
            {
                AnimationType.FirstAppearance => firstAppearance,
                AnimationType.Provocation => provocation,
                AnimationType.Appear => appear,
                AnimationType.Disappear => disappear,
                AnimationType.Attack1 => attack1,
                AnimationType.Damage => damage,
                _ => null
            };
        }
    }
}
