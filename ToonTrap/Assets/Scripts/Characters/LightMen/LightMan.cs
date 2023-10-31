using Ryocatusn.Games;
using Ryocatusn.Lights;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(Animator))]
    public class LightMan : MonoBehaviour
    {
        [SerializeField]
        private SpotLight spotLight;
        [SerializeField]
        private Transform spotLightExtraPosition;

        private Vector2 randomPosition;

        private Animator animator;

        [Inject]
        private GameManager gameManager;

        private void Start()
        {
            animator = GetComponent<Animator>();

            spotLight.on = false;

            StartCoroutine(ChangeRandomPosition());
            IEnumerator ChangeRandomPosition()
            {
                bool setted = false;
                gameManager.SetStageEvent
                    .Subscribe(_ => setted = true)
                    .AddTo(this);

                yield return new WaitUntil(() => setted);

                GameCamera gameCamera = gameManager.gameContains.gameCamera;

                while (true)
                {
                    Vector2 randomWorldPositionOnGame = gameCamera.camera.ViewportToWorldPoint(new Vector2(Random.Range(0, 1f), Random.Range(0, 1f)));
                    randomPosition = gameManager.GetWorldPositoinOnFinalResult(randomWorldPositionOnGame);
                    yield return new WaitForSeconds(2);
                }
            }
        }

        public void Appear()
        {
            animator.SetTrigger("Appear");
            TurnOnLight();
        }

        public void TurnOnLight()
        {
            spotLight.on = true;
        }
        private void Update()
        {
            spotLight.TurnOnExtra(spotLightExtraPosition.position);

            Vector2 currentVelocity = Vector2.zero;
            spotLight.transform.position = Vector2.SmoothDamp(spotLight.transform.position, randomPosition, ref currentVelocity, 0.2f);
        }
    }
}
