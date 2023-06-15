using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.TileTransforms;
using System;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(TileTransform))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class LocomotiveCar : AttackBehaviour, IForJankenViewEditor
    {
        [SerializeField]
        private Hand.Shape shape;
        [SerializeField]
        private JankenPrefabs jankenPrefabs;

        private TileTransform tileTransform;

        private bool finishAttack;

        private Subject<LocomotiveCar> blowAwayEvent = new Subject<LocomotiveCar>();
        public IObservable<LocomotiveCar> BlowAwayEvent => blowAwayEvent;

        public void SetUp(AttackableObjectId id)
        {
            tileTransform = GetComponent<TileTransform>();

            SetId(id, true);

            events.ReAttackTriggerEvent
                .Subscribe(_ => ReAttack())
                .AddTo(this);

            events.ReAttackTriggerEvent
                .Subscribe(_ => blowAwayEvent.OnNext(this))
                .AddTo(this);

            this.OnTriggerEnter2DAsObservable()
                .Where(_ => !finishAttack)
                .Subscribe(x => { if (x.TryGetComponent(out IReceiveAttack receiveAttack)) Attack(receiveAttack); });
        }
        private void OnDestroy()
        {
            blowAwayEvent.Dispose();
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
            tileTransform.movement.Get().CompleteEvent
                .Subscribe(_ => blowAwayEvent.OnNext(this))
                .AddTo(this);
        }
        public void BlowAway()
        {
            finishAttack = true;
            tileTransform.SetDisable();
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            rigid.AddForce(Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)) * (Vector2.up * 15), ForceMode2D.Impulse);

            StartCoroutine(DestroyWhenOutSideOfCamera());

            IEnumerator DestroyWhenOutSideOfCamera()
            {
                yield return new WaitUntil(() => gameManager.gameContains.gameCamera.IsOutSideOfCamera(gameObject));
                Destroy(gameObject);
            }
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
                    Destroy(transform.GetChild(i).gameObject);
                }
                GameObject prefab = jankenPrefabs.GetAsset(shape);
                Instantiate(prefab, gameObject.transform);
            }
        }
    }
}
