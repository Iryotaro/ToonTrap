using FTRuntime;
using Ryocatusn.Audio;
using UniRx;
using UnityEngine;
using Zenject;
using Ryocatusn.Games;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(SwfClipController))]
    public class TunnelScissorsAnimator : TunnelAnimator
    {
        [SerializeField]
        private int createLocomotiveFrame;
        private SwfClipController swfClipController;

        [SerializeField]
        private SE thunderSE;

        [Inject]
        private GameManager gameManager;

        private Subject<Unit> playThunderSEEvent = new Subject<Unit>();

        private void Awake()
        {
            swfClipController = GetComponent<SwfClipController>();
            swfClipController.clip.OnChangeCurrentFrameEvent += HandleChangeCurrentFrameEvent;

            playThunderSEEvent
                .FirstOrDefault()
                .Subscribe(_ => PlayThunderSE())
                .AddTo(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            playThunderSEEvent.Dispose();
        }

        public override void ChangeRateScale(float rateScale)
        {
            swfClipController = GetComponent<SwfClipController>();
            swfClipController.rateScale = rateScale;
        }

        void HandleChangeCurrentFrameEvent(SwfClip clip)
        {
            if (clip.currentFrame >= createLocomotiveFrame - 10)
            {
                playThunderSEEvent.OnNext(Unit.Default);
            }

            if (clip.currentFrame >= createLocomotiveFrame)
            {
                createLocomotiveEvent.OnNext(Unit.Default);
                swfClipController.clip.OnChangeCurrentFrameEvent -= HandleChangeCurrentFrameEvent;
            }
        }

        private void PlayThunderSE()
        {
            SEPlayer sePlayer = new SEPlayer(gameObject, gameManager.gameContains.gameCamera);
            sePlayer.Play(thunderSE);
        }
    }
}
