using UniRx;
using UnityEngine;
using DG.Tweening;
using Ryocatusn.Janken;
using Ryocatusn.Conversations;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class JankenReverseTrigger : MonoBehaviour
    {
        [SerializeField]
        private bool reverse;

        private void Start()
        {
            StageManager.activeStage.SetupStageEvent
                .Subscribe(gameContains =>
                {
                    GetComponent<TileTransformTrigger>().OnHitPlayerEvent
                        .FirstOrDefault()
                        .Subscribe(_ =>
                        {
                            Hand.JankenReverse(reverse);

                            if (reverse) gameContains.conversation.ShowMessage(new Message("負けなければいけない...", -1, 10, 2.5f));
                            else gameContains.conversation.ShowMessage(new Message("勝たなければいけない...", -1, 10, 2.5f));

                            Sequence sequence = DOTween.Sequence();
                            sequence
                            .AppendCallback(() => gameContains.gameCamera.colorAdjustments.hueShift.value = 180)
                            .AppendInterval(0.2f)
                            .AppendCallback(() => gameContains.gameCamera.colorAdjustments.hueShift.value = 0)
                            .AppendInterval(0.2f)
                            .AppendCallback(() => gameContains.gameCamera.colorAdjustments.hueShift.value = 180)
                            .AppendInterval(0.2f)
                            .AppendCallback(() => gameContains.gameCamera.colorAdjustments.hueShift.value = 0)
                            .AppendInterval(0.2f)
                            .AppendCallback(() => gameContains.gameCamera.colorAdjustments.hueShift.value = 180)
                            .AppendInterval(0.2f)
                            .AppendCallback(() => gameContains.gameCamera.colorAdjustments.hueShift.value = 0);
                        })
                        .AddTo(this);

                    StageManager.activeStage.AddResetHandler(() => Hand.JankenReverse(false));
                    gameContains.gameCamera.colorAdjustments.hueShift.value = 0;
                })
                .AddTo(this);
        }
    }
}
