using DG.Tweening;
using Ryocatusn.Games;
using UniRx;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class PowerOutageTrigger : MonoBehaviour
    {
        [Inject]
        private GameManager gameManager;
        [SerializeField]
        private bool outage = true;

        private void Start()
        {
            GetComponent<TileTransformTrigger>().OnHitPlayerEvent
                .FirstOrDefault()
                .Subscribe(_ =>
                {
                    if (outage) PowerOutage();
                    else ResetLight();
                })
                .AddTo(this);

            gameManager
                .SetStageEvent
                .Subscribe(x =>
                {
                    x.OverEvent
                    .Subscribe(_ => ResetLight())
                    .AddTo(this);

                    x.ClearEvent
                    .Subscribe(_ => ResetLight())
                    .AddTo(this);
                })
                .AddTo(this);
        }

        private void PowerOutage()
        {
            GameContains gameContains = gameManager.gameContains;
            gameContains.lightContains.playerLight.spotLight.on = false;

            gameContains.player.inputMaster.SetActiveAll(false);

            DOTween.Sequence()
                .SetLink(gameContains.lightContains.globalLight.gameObject)
                .Append(gameContains.lightContains.globalLight.DoChangeItencity(0, 1))
                .Join(ChangePitch(gameContains.bgm, 0.6f, 1))
                .SetEase(Ease.InOutCubic)
                .AppendInterval(0.5f)
                .AppendCallback(() =>
                {
                    gameContains.playerBody.HoldLight();
                })
                .AppendInterval(3)
                .OnComplete(() =>
                {
                    gameContains.lightContains.playerLight.spotLight.on = true;
                    gameContains.lightMan.Appear();
                    gameContains.player.inputMaster.SetActiveAll(true);
                });
        }
        private void ResetLight()
        {
            GameContains gameContains = gameManager.gameContains;
            gameContains.lightContains.playerLight.spotLight.on = false;

            gameContains.playerBody.Idle();
            gameContains.lightMan.Disappear();

            gameContains.bgm.pitch = 1;

            gameContains.lightContains.globalLight.DoChangeItencity(1, 1)
                .SetLink(gameContains.lightContains.globalLight.gameObject)
                .SetEase(Ease.InOutCubic);
        }

        private Tween ChangePitch(AudioSource audioSource, float endValue, float duration)
        {
            return DOTween.To
                (
                () => audioSource.pitch,
                x => audioSource.pitch = x,
                endValue,
                duration
                );
        }
    }
}
