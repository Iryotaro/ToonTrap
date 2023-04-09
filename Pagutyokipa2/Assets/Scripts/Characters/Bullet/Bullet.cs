using Microsoft.Extensions.DependencyInjection;
using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Bullet : MonoBehaviour, IForJankenViewEditor
    {
        private AttackableObjectId id;
        private AttackableObjectApplicationService attackableObjectApplicationService = Installer.installer.serviceProvider.GetService<AttackableObjectApplicationService>();

        [SerializeField]
        private Hand.Shape shape;
        [SerializeField]
        private JankenPrefabs jankenPrefabs;
        [SerializeField]
        private float power;

        private Rigidbody2D rigid;

        public void SetUp(AttackableObjectId id)
        {
            this.id = id;

            rigid = GetComponent<Rigidbody2D>();
            Collider2D collider = GetComponent<Collider2D>();

            StageManager.activeStage.SetupStageEvent
                .Subscribe(gameContains =>
                {
                    this.OnTriggerEnter2DAsObservable()
                        .Where(x =>
                        {
                            if (x.TryGetComponent(out IReceiveAttack i))
                            {
                                return i == (IReceiveAttack)gameContains.player;
                            }
                            return false;
                        })
                        .Take(1)
                        .Subscribe(x => OnHit(gameContains.player));
                });

            rigid.AddForce(transform.up * power, ForceMode2D.Impulse);

            Destroy(gameObject, 5);
        }
        private void OnDestroy()
        {
            if (id == null) return;
            attackableObjectApplicationService.Delete(id);
        }

        private void OnHit(IReceiveAttack receiveAttack)
        {
            attackableObjectApplicationService.Attack(id, receiveAttack);
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
