using UnityEngine;
using Zenject;
using Ryocatusn.Games;

namespace Ryocatusn
{
    [RequireComponent(typeof(Canvas))]
    public class StageCanvas : MonoBehaviour
    {
        [Inject]
        private GameManager gameManager;

        private void Awake()
        {
            Camera camera = gameManager.gameContains.gameCamera.camera;
            GetComponent<Canvas>().worldCamera = camera;
        }
    }
}