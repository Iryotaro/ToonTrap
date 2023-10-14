using System;
using UnityEngine;
using UniRx;
using System.Linq;
using System.Collections;

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
        private Frame attackFrame;

        public enum AnimationType
        {
            FirstAppearance,
            Provocation,
            Appear,
            Disappear,
            Attack1
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
                _ => null
            };
        }
    }
}
