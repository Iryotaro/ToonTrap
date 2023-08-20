using System;
using UnityEngine;
using UniRx;

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

        public void ShowFirstAppearanceFirstFrame()
        {
            firstAppearance.ShowFirstFrame();
        }
        public void PlayAnimation(AnimationType animationType, Action finishAction = null)
        {
            FrameAnimator frameAnimator = GetAnimator(animationType);
            HideAllFrames();
            frameAnimator.Play();
            if (finishAction != null) frameAnimator.OnCompleted.Subscribe(_ => finishAction()).AddTo(this);
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
