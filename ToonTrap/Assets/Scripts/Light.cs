using Ryocatusn.Games;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    public class Light : MonoBehaviour
    {
        [Inject]
        private GameManager gameManager;

        private void Update()
        {
            Player player = gameManager.gameContains.player;
            Vector2 playerWorldPositionOnFinalResult = gameManager.GetWorldPositoinOnFinalResult(player.transform.position);
            transform.position = playerWorldPositionOnFinalResult;
        }
    }
}
