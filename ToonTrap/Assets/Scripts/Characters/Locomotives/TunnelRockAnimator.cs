using UniRx;
using UnityEngine;

namespace Ryocatusn.Characters
{
    /// <summary>
    /// Rock用のTunnelAnimator
    /// Rockだけ、なぜかSwfClipControllerで上手くいかなかったため別にしている
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class TunnelRockAnimator : TunnelAnimator
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public override void ChangeRateScale(float rateScale)
        {
            animator.speed = rateScale;
        }

        public void HandleCreateLocomotiveFromAnimation()
        {
            createLocomotiveEvent.OnNext(Unit.Default);
        }
    }
}
