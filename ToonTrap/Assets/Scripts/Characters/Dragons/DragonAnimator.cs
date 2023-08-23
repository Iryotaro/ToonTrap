using System;
using UnityEngine;
using UniRx;
using System.Linq;

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
        private FrameAnimator attack1;

        [SerializeField]
        private Frame attackFrame;

        public enum AnimationType
        {
            FirstAppearance,
            Provocation,
            Appear,
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
        public void PlayAnimations(AnimationType[] animationTypes)
        {
            for (int i = 0; i < animationTypes.Count() - 1; i++)
            {
                FrameAnimator frameAnimator = PlayAnimation(animationTypes[i]);
                AnimationType nextAnimationType = animationTypes[i + 1];
                frameAnimator.CompleteSubjet
                    .Subscribe(_ => PlayAnimation(nextAnimationType))
                    .AddTo(this);
            }
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
                AnimationType.Attack1 => attack1,
                _ => null
            };
        }
    }
}
