using UniRx;
using UnityEngine;


namespace Ryocatusn
{
    [RequireComponent(typeof(UnityEngine.Rendering.Universal.Light2D))]
    public class PlayerLight : MonoBehaviour
    {
        private Transform playerTransform;
        private UnityEngine.Rendering.Universal.Light2D playerLight;

        private void Start()
        {
            playerLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            playerLight.intensity = 0;

            StageManager.activeStage.SetupStageEvent
                .Subscribe(gameContains =>
                {
                    playerTransform = gameContains.player.transform;
                })
                .AddTo(this);
        }
        private void Update()
        {
            if (playerTransform == null) return;
            transform.position = playerTransform.position;

            StageManager.activeStage.gameContains.Match(gameContains =>
            {
                if (gameContains.globalLight.intensity == 0) playerLight.intensity = 1;
                else playerLight.intensity = 0;
            });
        }
    }
}
