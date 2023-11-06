using UnityEngine;
using UniRx;
using Zenject;
using Ryocatusn.Games;
using UniRx.Triggers;

namespace Ryocatusn.Characters
{
    public class StickyNoteTrigger : MonoBehaviour
    {
        [SerializeField]
        private StickyNote stickyNote;
        [SerializeField]
        private TileTransformTrigger[] triggers;
        [SerializeField, Range(0, 1)]
        private float viewportX;
        [SerializeField, Range(0, 1)]
        private float viewportY;
        [SerializeField]
        private Road[] addRoads;

        [Inject]
        private GameManager gameManager;
        [Inject]
        private DiContainer diContainer;

        private void Start()
        {
            bool once = true;
            foreach (TileTransformTrigger trigger in triggers)
            {
                trigger.OnHitPlayerEvent
                    .Where(_ => once)
                    .Subscribe(_ =>
                    {
                        once = false;

                        StickyNote stickyNote = Create();

                        stickyNote.OnDestroyAsObservable()
                        .Subscribe(_ => AddRoads(addRoads))
                        .AddTo(this);
                    })
                    .AddTo(this);
            }
        }

        private StickyNote Create()
        {
            GameCamera gameCamera = gameManager.gameContains.gameCamera;
            Vector3 createPosition = gameCamera.camera.ViewportToWorldPoint(new Vector3(0, viewportY));
            createPosition = createPosition - Vector3.left;
            createPosition = new Vector3(createPosition.x, createPosition.y, 0);

            StickyNote newStickyNote = Instantiate(stickyNote, transform);
            newStickyNote.transform.position = createPosition;
            diContainer.InjectGameObject(newStickyNote.gameObject);
            newStickyNote.Setup(new Vector2(viewportX, viewportY));

            return newStickyNote;
        }

        private void AddRoads(Road[] roads)
        {
            foreach (Road road in roads)
            {
                if (road == null) continue;

                road.Appear();
            }
        }
    }
}
