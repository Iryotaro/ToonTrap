using UnityEngine;
using UniRx;
using Ryocatusn.Characters;
using System.Collections;
using Zenject;
using Ryocatusn.Games;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class PlayCutSceneTrigger : MonoBehaviour
    {
        [SerializeField]
        private VirtualCamera virtualCamera;
        [SerializeField]
        private Tunnel tunnel;
        [SerializeField]
        private ParticleSystem effect;

        private VirtualCamera beginCamera;
        private ParticleSystem newEffect;

        [Inject]
        private GameManager gameManager;

        private void Start()
        {
            beginCamera = VirtualCameraManager.instance.FindEnableCamera().Get();

            newEffect = Instantiate(effect, gameManager.gameContains.gameCamera.transform);
            newEffect.transform.position = new Vector3(18, 0, 10);

            GetComponent<TileTransformTrigger>()
                .OnHitPlayerEvent
                .FirstOrDefault()
                .Subscribe(_ => StartCoroutine(Play()))
                .AddTo(this);
        }
        
        private IEnumerator Play()
        {
            gameManager.gameContains.player.inputMaster.SetActiveAll(false);
            virtualCamera.SetEnableCamera();
            newEffect.Play();
            yield return new WaitForSeconds(1);
            tunnel.Play();
            newEffect.Stop();
            yield return new WaitForSeconds(5);
            if (beginCamera != null) beginCamera.SetEnableCamera();
            gameManager.gameContains.player.inputMaster.SetActiveAll(true);
        }
    }
}
