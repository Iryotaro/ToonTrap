using Ryocatusn.Games;
using Ryocatusn.Janken.JankenableObjects;
using UnityEngine;
using Zenject;

namespace Ryocatusn.Characters
{
    public class StickyNoteSniperScope : MonoBehaviour
    {
        [Inject]
        private GameManager gameManager;

        public void Setup(JankenableObjectId id)
        {
            GameCamera gameCamera = gameManager.gameContains.gameCamera;

            Vector2 randomViewport = new Vector2(Random.Range(0, 1), Random.Range(0, 1));
            Vector2 randomWorldPoint = gameCamera.camera.ViewportToWorldPoint(randomViewport);
        }

        private void ChaseToPlayer()
        {
            Vector2 playerPosition = gameManager.gameContains.player.transform.position;
            Vector2 currentVelocity = Vector2.zero;
            transform.position = Vector2.SmoothDamp(transform.position, playerPosition, ref currentVelocity, 1);
        }
    }
}
