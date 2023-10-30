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

        private void Start()
        {
            GetComponent<TileTransformTrigger>().OnHitPlayerEvent
                .FirstOrDefault()
                .Subscribe(_ => PowerOutage())
                .AddTo(this);
        }
        
        private void PowerOutage()
        {
            GameContains gameContains = gameManager.gameContains;
            gameContains.lightContains.playerBodyLight.on = false;
            gameContains.lightContains.playerLight.spotLight.on = false;

            DOTween.Sequence()
                .Append(gameContains.lightContains.globalLight.DoChangeItencity(0, 1))
                .SetEase(Ease.InOutCubic)
                .AppendInterval(0.5f)
                .AppendCallback(() =>
                {
                    gameContains.lightContains.playerBodyLight.on = true;
                    gameContains.playerBody.HoldLight();
                })
                .AppendInterval(3)
                .AppendCallback(() =>
                {
                    gameContains.lightContains.playerLight.spotLight.on = true;
                });
        }
    }
}
