using UnityEngine;
using UniRx;
using Ryocatusn.Characters;
using System.Collections;
using Zenject;
using Ryocatusn.Games;
using Ryocatusn.Audio;

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
        [SerializeField]
        private SE se;

        private VirtualCamera beginCamera;
        private ParticleSystem newEffect;

        [Inject]
        private GameManager gameManager;
        [Inject]
        private StageManager stageManager;

        private void Start()
        {
            newEffect = Instantiate(effect, gameManager.gameContains.gameCamera.transform);
            newEffect.transform.position = new Vector3(18, 0, 0);

            GetComponent<TileTransformTrigger>()
                .OnHitPlayerEvent
                .Where(x => stageManager.GetData().countOfRetry == 1)
                .FirstOrDefault()
                .Subscribe(_ => StartCoroutine(Play()))
                .AddTo(this);
        }
        
        private IEnumerator Play()
        {
            beginCamera = VirtualCameraManager.instance.FindEnableCamera().Get();

            gameManager.gameContains.player.inputMaster.SetActiveAll(false);
            virtualCamera.SetEnableCamera();
            newEffect.Play();
            new SEPlayer(gameObject, gameManager.gameContains.gameCamera).Play(se);
            yield return new WaitForSeconds(1);
            tunnel.Play();
            newEffect.Stop();
            Destroy(newEffect);
            yield return new WaitForSeconds(5);
            if (beginCamera != null) beginCamera.SetEnableCamera();
            gameManager.gameContains.player.inputMaster.SetActiveAll(true);
        }
    }
}
