using FTRuntime;
using UniRx;
using UnityEngine;

namespace Ryocatusn.Characters
{
    /// <summary>
    /// ScissorsとPaper用のTunnelAnimator
    /// Rockだけ、なぜかSwfClipControllerで上手くいかなかったため別にしている
    /// </summary>
    [RequireComponent(typeof(SwfClipController))]
    public class TunnelScissorsOrPaperAnimator : TunnelAnimator
    {
        [SerializeField]
        private int createLocomotiveFrame;
        private SwfClipController swfClipController;

        private void Awake()
        {
            swfClipController = GetComponent<SwfClipController>();
            swfClipController.clip.OnChangeCurrentFrameEvent += HandleChangeCurrentFrameEvent;
        }

        public override void ChangeRateScale(float rateScale)
        {
            swfClipController = GetComponent<SwfClipController>();
            swfClipController.rateScale = rateScale;
        }

        void HandleChangeCurrentFrameEvent(SwfClip clip)
        {
            if (clip.currentFrame >= createLocomotiveFrame)
            {
                createLocomotiveEvent.OnNext(Unit.Default);
                swfClipController.clip.OnChangeCurrentFrameEvent -= HandleChangeCurrentFrameEvent;
            }
        }
    }
}
