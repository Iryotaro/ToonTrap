using FTRuntime;
using Ryocatusn.Audio;
using Ryocatusn.Games;
using UniRx;
using UnityEngine;
using Zenject;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(SwfClipController))]
    public class TunnelPaperAnimator : TunnelAnimator
    {
        [SerializeField]
        private int createLocomotiveFrame;
        private SwfClipController swfClipController;

        [SerializeField]
        private SE surpriseSE;

        [Inject]
        private GameManager gameManager;

        private Subject<Unit> playSurpriseSEEvent = new Subject<Unit>();

        private void Awake()
        {
            swfClipController = GetComponent<SwfClipController>();
            swfClipController.clip.OnChangeCurrentFrameEvent += HandleChangeCurrentFrameEvent;

            playSurpriseSEEvent
                .FirstOrDefault()
                .Subscribe(_ => PlaySurpriseSE())
                .AddTo(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            playSurpriseSEEvent.Dispose();
        }

        public override void ChangeRateScale(float rateScale)
        {
            swfClipController = GetComponent<SwfClipController>();
            swfClipController.rateScale = rateScale;
        }

        private void HandleChangeCurrentFrameEvent(SwfClip clip)
        {
            if (clip.currentFrame >= createLocomotiveFrame - 10)
            {
                playSurpriseSEEvent.OnNext(Unit.Default);
            }

            if (clip.currentFrame >= createLocomotiveFrame)
            {
                createLocomotiveEvent.OnNext(Unit.Default);
                swfClipController.clip.OnChangeCurrentFrameEvent -= HandleChangeCurrentFrameEvent;
            }
        }

        private void PlaySurpriseSE()
        {
            SEPlayer sePlayer = new SEPlayer(gameObject, gameManager.gameContains.gameCamera);
            sePlayer.Play(surpriseSE);
        }
    }
}