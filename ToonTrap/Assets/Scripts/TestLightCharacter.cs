using Ryocatusn.Games;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Ryocatusn.Characters
{
    public class TestLightCharacter : MonoBehaviour
    {
        [SerializeField]
        private Light2D spotLight;

        private Vector2 randomViewport;

        [Inject]
        private GameManager gameManager;

        private void Start()
        {
            StartCoroutine(changeRandomViewport());
            IEnumerator changeRandomViewport()
            {
                while (true)
                {
                    randomViewport = new Vector2(Random.Range(0, 1f), Random.Range(0, 1f));
                    yield return new WaitForSeconds(Random.Range(0.5f, 2));
                }
            }
        }
        private void Update()
        {
            Vector2 target = gameManager.gameContains.gameCamera.camera.ViewportToWorldPoint(randomViewport);
            Vector2 currentVelocity = Vector2.zero;
            Vector2 worldPosition = Vector2.SmoothDamp(spotLight.transform.position, target, ref currentVelocity, 1 / 5f);
            spotLight.transform.position = worldPosition;
        }
    }
}
