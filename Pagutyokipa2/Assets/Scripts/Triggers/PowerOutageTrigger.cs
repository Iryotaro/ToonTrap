using System;
using Microsoft.Extensions.DependencyInjection;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Ryocatusn.Games.Stages;
using Ryocatusn.Ryoseqs;
using Ryocatusn.Audio;

namespace Ryocatusn
{
    public class PowerOutageTrigger : MonoBehaviour
    {
        [SerializeField]
        private Trigger[] triggers;
        [SerializeField]
        private float duration = 2;
        [SerializeField]
        private SE flashSE;

        private GameContains gameContains;

        private SEPlayer sePlayer;

        private float defaultIntensity;
        private (Color color1, Color color2) defaultBackgroundColor;
        private float defaultPitch;

        private bool outage = false;

        private void Start()
        {
            StageManager.activeStage.SetupStageEvent
                .Subscribe(gameContains =>
                {
                    this.gameContains = gameContains;

                    StageApplicationService stageApplicationService = Installer.installer.serviceProvider.GetService<StageApplicationService>();
                    StageEvents stageEvents = stageApplicationService.GetEvents(StageManager.activeStage.id);

                    defaultIntensity = gameContains.globalLight.intensity;
                    defaultBackgroundColor = (gameContains.background.image1.color, gameContains.background.image2.color);
                    defaultPitch = gameContains.bgm.pitch;

                    StageManager.activeStage.AddResetHandler(() =>
                    {
                        gameContains.globalLight.intensity = defaultIntensity;
                        gameContains.background.image1.color = defaultBackgroundColor.color1;
                        gameContains.background.image2.color = defaultBackgroundColor.color2;
                        gameContains.bgm.pitch = defaultPitch;
                    });

                    foreach (Trigger trigger in triggers)
                    {
                        trigger.tileTransformTrigger.OnHitPlayerEvent
                            .FirstOrDefault()
                            .Subscribe(_ =>
                            {
                                if (trigger.outage) PowerOutage();
                                else PowerReset();
                            })
                            .AddTo(this);
                    }
                })
                .AddTo(this);

            sePlayer = new SEPlayer(gameObject);

            InvokeRepeating(nameof(Flash), 10, 10);
        }

        private void PowerOutage()
        {
            if (gameContains == null) return;

            outage = true;

            Tween lightTween = CreateLightTween(gameContains, 0);
            Tween backgroundTween = CreateBackgroundTween(gameContains, (Color.black, Color.black));
            Tween bgmTween = CreateBGMTween(gameContains, 0.5f);

            DG.Tweening.Sequence sequence = DOTween.Sequence();
            sequence
            .Append(lightTween)
            .Join(backgroundTween)
            .Join(bgmTween)
            .SetLink(gameObject);
        }
        private void PowerReset()
        {
            if (gameContains == null) return;

            outage = false;

            Tween lightTween = CreateLightTween(gameContains, defaultIntensity);
            Tween backgroundTween = CreateBackgroundTween(gameContains, defaultBackgroundColor);
            Tween bgmTween = CreateBGMTween(gameContains, defaultPitch);

            DG.Tweening.Sequence sequence = DOTween.Sequence();
            sequence
            .Append(lightTween)
            .Join(backgroundTween)
            .Join(bgmTween)
            .SetLink(gameContains.globalLight.gameObject)
            .SetLink(gameContains.background.gameObject)
            .SetLink(gameContains.bgm.gameObject);
        }
        private Tween CreateLightTween(GameContains gameContains, float intensity)
        {
            return DOTween.To(
                () => gameContains.globalLight.intensity,
                x => gameContains.globalLight.intensity = x,
                intensity,
                duration
                )
                .SetLink(gameContains.globalLight.gameObject);
        }
        private Tween CreateBackgroundTween(GameContains gameContains, (Color color1, Color color2) colors)
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence();
            return sequence
                .SetLink(gameContains.background.image1.gameObject)
                .SetLink(gameContains.background.image2.gameObject)
                .Append(gameContains.background.image1.DOColor(colors.color1, duration))
                .Join(gameContains.background.image2.DOColor(colors.color2, duration));
        }
        private Tween CreateBGMTween(GameContains gameContains, float pitch)
        {
            return DOTween.To(
                () => gameContains.bgm.pitch,
                x => gameContains.bgm.pitch = x,
                pitch,
                duration
                )
                .SetLink(gameContains.bgm.gameObject);
        }

        private void Flash()
        {
            if (gameContains == null) return;
            if (!outage) return;

            Ryoseq ryoseq = new Ryoseq();
            ryoseq.AddTo(this);

            ISequence sequence = ryoseq.Create();
            sequence
                .Connect(new SequenceCommand(_ => ChangeGlobalIntensity(0.4f)))
                .Connect(new SequenceCommand(_ => sePlayer.Play(flashSE)))
                .ConnectWait(new SequenceWaitForSeconds(0.5f))
                .Connect(new SequenceCommand(_ => ChangeGlobalIntensity(0)))
                .ConnectWait(new SequenceWaitForSeconds(0.5f))
                .Connect(new SequenceCommand(_ => ChangeGlobalIntensity(0.4f)))
                .Connect(new SequenceCommand(_ => sePlayer.Play(flashSE)))
                .ConnectWait(new SequenceWaitForSeconds(0.5f))
                .Connect(new SequenceCommand(_ => ChangeGlobalIntensity(0)));

            ryoseq.MoveNext();

            void ChangeGlobalIntensity(float intensity)
            {
                gameContains.globalLight.intensity = intensity;
            }
        }

        [Serializable]
        public class Trigger
        {
            public TileTransformTrigger tileTransformTrigger;
            public bool outage;
        }
    }
}
