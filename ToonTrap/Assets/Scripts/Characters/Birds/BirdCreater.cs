using Ryocatusn.Games;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Ryocatusn.Characters
{
    public class BirdCreater : MonoBehaviour
    {
        [SerializeField]
        private Bird bird;
        [SerializeField]
        private float createRate;

        [Inject]
        private GameManager gameManager;
        [Inject]
        private DiContainer diContainer;

        private void Start()
        {
            StartCoroutine(CreateRepeating());
            IEnumerator CreateRepeating()
            {
                while (true)
                {
                    Create();
                    yield return new WaitForSeconds((1 / createRate) + Random.Range(-1 / createRate, 1 / createRate));
                }
            }
        }
        private void Create()
        {
            Camera camera = gameManager.gameContains.gameCamera.camera;
            Vector2 createPosition = camera.ViewportToWorldPoint(new Vector2(-0.3f, Random.Range(0, 1f)));
            Bird newBird = Instantiate(bird, transform);
            diContainer.InjectGameObject(newBird.gameObject);
            newBird.transform.position = createPosition;
        }
    }
}
