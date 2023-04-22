using UnityEngine;
using Ryocatusn.Janken;
using Ryocatusn.TileTransforms;
using Ryocatusn.Janken.JankenableObjects;
using System.Linq;
using UnityEngine.Tilemaps;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(TileTransform))]
    public class LocomotiveCar : JankenBehaviour, IForJankenViewEditor
    {
        [SerializeField]
        private Hand.Shape shape;
        [SerializeField]
        private JankenPrefabs jankenPrefabs;

        private TileTransform tileTransform;

        public void SetUp(Hand.Shape shape)
        {
            tileTransform = GetComponent<TileTransform>();

            JankenableObjectCreateCommand command = new JankenableObjectCreateCommand(new Hp(1), shape);
            Create(command);
        }

        private void Update()
        {
            transform.rotation = GetRotation();
            Quaternion GetRotation()
            {
                return tileTransform.tileDirection.value switch
                {
                    TileDirection.Direction.Up => Quaternion.Euler(0, 0, 0),
                    TileDirection.Direction.Down => Quaternion.Euler(0, 0, 180),
                    TileDirection.Direction.Left => Quaternion.Euler(0, 0, 90),
                    TileDirection.Direction.Right => Quaternion.Euler(0, 0, -90),
                    _ => Quaternion.Euler(0, 0, 0)
                };
            }
        }

        public void Move(Railway railway, float moveRate)
        {
            tileTransform.ChangeTilemap(railway.tilemaps, transform.position);
            MoveAStar moveDataCreater = new MoveAStar(railway.startPosition.position, railway.endPosition.position, railway.tilemaps.ToList());
            tileTransform.SetMovement(moveDataCreater, new MoveRate(moveRate));
        }

        public Hand.Shape GetShape()
        {
            if (id == null) return shape;
            else return jankenableObjectApplicationService.Get(id).shape;
        }
        public void UpdateView(Hand.Shape shape)
        {
            if (jankenPrefabs.TryGetRenderer(out GameObject gameObject, this))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                }
                GameObject prefab = jankenPrefabs.GetAsset(shape);
                Instantiate(prefab, gameObject.transform);
            }
        }
    }
}
