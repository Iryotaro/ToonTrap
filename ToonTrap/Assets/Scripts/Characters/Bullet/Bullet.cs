using Cysharp.Threading.Tasks;
using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Ryocatusn.Audio;

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
        private bool m_attackToOnlyPlayer = false;
        [SerializeField]
        private SE reAttackSE;

        private GameObject ownerObject;
        private bool isAllowedToDestroy = true;

        public void SetUp(AttackableObjectId id, GameObject ownerObject)
        {
            SetId(id, m_attackToOnlyPlayer);
            this.ownerObject = ownerObject;

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

            this.OnTriggerEnter2DAsObservable()
            .Where(x => IsAllowedToAttack(x, gameManager.gameContains.player))
            .Take(1)
            .Subscribe(x => OnHit(x.GetComponent<IReceiveAttack>()));

            GetComponent<Rigidbody2D>().AddForce(transform.up * power, ForceMode2D.Impulse);

            StartCoroutine(DestroyWhenOutSideOfCamera());

            IEnumerator DestroyWhenOutSideOfCamera()
            {
                yield return new WaitUntil(() => IsAllowedToDestroy());
                Destroy(gameObject);

                bool IsAllowedToDestroy()
                {
                    if (!isAllowedToDestroy) return false;
                    if (!gameManager.gameContains.gameCamera.IsOutSideOfCamera(gameObject)) return false;
                    return true;
                }
            }
        }

        private bool IsAllowedToAttack(Collider2D collider, Player player)
        {
            IReceiveAttack receiveAttack = collider.GetComponentInChildren<IReceiveAttack>();
            if (receiveAttack == null) return false;
            if (receiveAttack.GetId() == null) return false;
            if (receiveAttack.GetId().Equals(Get().ownerId)) return false;
            if (!m_attackToOnlyPlayer) return true;
            return receiveAttack == (IReceiveAttack)player;
        }
        private void OnHit(IReceiveAttack receiveAttack)
        {
            Attack(receiveAttack);
        }

        private void HandleReAttackTrigger()
        {
            isAllowedToDestroy = false;

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
                    //二次関数
                    return Mathf.Pow(Time.fixedTime - time, 2);
                }
            }

            new SEPlayer(gameObject, gameManager.gameContains.gameCamera).Play(reAttackSE);
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
                    if (Application.isPlaying) Destroy(transform.GetChild(i).gameObject);
                    else DestroyImmediate(transform.GetChild(i).gameObject);
                }
                GameObject prefab = jankenPrefabs.GetAsset(shape);
                Instantiate(prefab, gameObject.transform);
            }
        }
    }
}
