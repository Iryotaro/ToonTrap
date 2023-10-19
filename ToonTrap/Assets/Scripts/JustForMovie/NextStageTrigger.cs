using UniRx;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class NextStageTrigger : MonoBehaviour
    {
        [Inject]
        private StageManager stageManager;

        private void Start()
        {
            GetComponent<TileTransformTrigger>().OnHitPlayerEvent
                .FirstOrDefault()
                .Subscribe(_ => Transition.LoadScene("Stage1", "StageDragon_movie", new TransitionSettings(null, null, Janken.Hand.Shape.Scissors)))
                .AddTo(this);
        }
    }
}
