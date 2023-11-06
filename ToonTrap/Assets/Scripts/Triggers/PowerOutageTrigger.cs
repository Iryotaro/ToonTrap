using UnityEngine;
using UniRx;
using Zenject;
using Ryocatusn.Games;
using DG.Tweening;

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

            gameContains.lightContains.globalLight.DoChangeItencity(1, 1)
                .SetLink(gameContains.lightContains.globalLight.gameObject)
                .SetEase(Ease.InOutCubic);
        }
    }
}
