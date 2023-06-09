using UnityEngine;

namespace Ryocatusn.Games
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField]
        private OffLineGameManager gameManager;
        private GameId id;
        private Player player;

        private void Start()
        {
            id = gameManager.id;
            player = gameManager.gameContains.player;
        }
    }
}
