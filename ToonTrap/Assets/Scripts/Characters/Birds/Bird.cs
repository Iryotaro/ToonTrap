using Ryocatusn.Games;
using UnityEngine;
using Zenject;

namespace Ryocatusn.Characters
{

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bird : MonoBehaviour
    {
        [SerializeField]
        private float speed = 1;
        private float lastTimeInSideCamera;

        [Inject]
        private GameManager gameManager;

        private void Update()
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);

            if (!gameManager.gameContains.gameCamera.IsOutSideOfCamera(gameObject)) lastTimeInSideCamera = Time.fixedTime;
            if (lastTimeInSideCamera - Time.fixedTime > 15) Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out AttackBehaviour attackBehaviour))
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                Destroy(gameObject, 5);
            }
        }
    }
}
