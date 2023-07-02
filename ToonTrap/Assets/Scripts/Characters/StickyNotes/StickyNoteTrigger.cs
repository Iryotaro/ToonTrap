using UnityEngine;
using UniRx;
using Zenject;
using Ryocatusn.Games;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class StickyNoteTrigger : MonoBehaviour
    {
        [SerializeField]
        private StickyNote stickyNote;
        [SerializeField, Range(0, 1)]
        private float viewportY;

        [Inject]
        private GameManager gameManager;
        [Inject]
        private DiContainer diContainer;

        private void Start()
        {
            TileTransformTrigger trigger = GetComponent<TileTransformTrigger>();

            trigger.OnHitPlayerEvent
                .FirstOrDefault()
                .Subscribe(_ => Create())
                .AddTo(this);
        }

        private void Create()
        {
            GameCamera gameCamera = gameManager.gameContains.gameCamera;
            Vector3 createPosition = gameCamera.camera.ViewportToWorldPoint(new Vector3(0, viewportY));
            createPosition = createPosition - Vector3.left;
            createPosition = new Vector3(createPosition.x, createPosition.y, 0);

            StickyNote newStickyNote = Instantiate(stickyNote, createPosition, Quaternion.identity);
            diContainer.InjectGameObject(newStickyNote.gameObject);
            newStickyNote.Setup();
        }
    }
}
