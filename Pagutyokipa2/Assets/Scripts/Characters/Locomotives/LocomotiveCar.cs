using UnityEngine;
using Ryocatusn.Janken;
using Ryocatusn.TileTransforms;
using System.Linq;
using Ryocatusn.Janken.AttackableObjects;
using UniRx;
using UniRx.Triggers;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(TileTransform))]
    [RequireComponent(typeof(Collider2D))]
    public class LocomotiveCar : AttackBehaviour, IForJankenViewEditor
    {
        [SerializeField]
        private Hand.Shape shape;
        [SerializeField]
        private JankenPrefabs jankenPrefabs;

        private TileTransform tileTransform;

        public void SetUp(AttackableObjectId id)
        {
            tileTransform = GetComponent<TileTransform>();

            SetId(id);

            this.OnTriggerEnter2DAsObservable()
                .Subscribe(x => { if (x.TryGetComponent(out IReceiveAttack receiveAttack)) Attack(receiveAttack); });
        }

        private void Update()
        {
            transform.rotation = Quaternion.Euler(0, 0, 180 + tileTransform.angle);
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
            else return attackableObjectApplicationService.Get(id).shape;
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
