using Cysharp.Threading.Tasks;
using Ryocatusn.Games;
using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Bullet : AttackBehaviour, IForJankenViewEditor
    {
        [SerializeField]
        private Hand.Shape shape;
        [SerializeField]
        private JankenPrefabs jankenPrefabs;
        [SerializeField]
        private float power;
        [SerializeField]
        private bool attackToOnlyPlayer = false;

        [Inject]
        GameManager gameManager;

        private Rigidbody2D rigid;

        private GameObject ownerObject;

        public void SetUp(AttackableObjectId id, GameObject ownerObject)
        {
            SetId(id, attackToOnlyPlayer);
            this.ownerObject = ownerObject;

            rigid = GetComponent<Rigidbody2D>();
            Collider2D collider = GetComponent<Collider2D>();

            events.WinEvent
                .Subscribe(_ => Destroy(gameObject))
                .AddTo(this);

            events.ReAttackTriggerEvent
                .Subscribe(_ => HandleReAttackTrigger())
                .AddTo(this);

            events.OwnerDieEvent
                .Subscribe(_ => Destroy(gameObject))
                .AddTo(this);

            gameManager.nowStageManager.SetupStageEvent
                .Subscribe(gameContains =>
                {
                    this.OnTriggerEnter2DAsObservable()
                    .Where(x => IsAllowedToAttack(x, gameContains.player))
                    .Take(1)
                    .Subscribe(x => OnHit(x.GetComponent<IReceiveAttack>()));
                });

            rigid.AddForce(transform.up * power, ForceMode2D.Impulse);

            Destroy(gameObject, 5);
        }

        private bool IsAllowedToAttack(Collider2D collider, Player player)
        {
            IReceiveAttack receiveAttack = collider.GetComponentInChildren<IReceiveAttack>();
            if (receiveAttack != null)
            {
                if (receiveAttack.GetId() == null) return false;
                if (receiveAttack.GetId().Equals(Get().ownerId)) return false;
                if (!attackToOnlyPlayer) return true;
                return receiveAttack == (IReceiveAttack)player;
            }
            else
            {
                return false;
            }
        }
        private void OnHit(IReceiveAttack receiveAttack)
        {
            Attack(receiveAttack);
        }

        private void HandleReAttackTrigger()
        {
            StartCoroutine(Move());

            IEnumerator Move()
            {
                float time = Time.fixedTime;
                Vector2 startPosition = transform.position;

                while (GetTime() < 1)
                {
                    yield return new WaitForFixedUpdate();

                    Vector2 endPosition = ownerObject.transform.position;
                    transform.position = Vector2.Lerp(startPosition, endPosition, GetTime());

                    float angle = -90 + Mathf.Atan2(endPosition.y - transform.position.y, endPosition.x - transform.position.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }

                ReAttack();
                Destroy(gameObject);

                float GetTime()
                {
                    //“ñŽŸŠÖ”
                    return Mathf.Pow(Time.fixedTime - time, 2);
                }
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
                    DestroyImmediate(transform.GetChild(i).gameObject);
                }
                GameObject prefab = jankenPrefabs.GetAsset(shape);
                Instantiate(prefab, gameObject.transform);
            }
        }
    }
}
