using Microsoft.Extensions.DependencyInjection;
using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Renderer))]
    public class Bullet : MonoBehaviour, IForJankenViewEditor
    {
        private AttackableObjectId id;
        private AttackableObjectApplicationService attackableObjectApplicationService = Installer.installer.serviceProvider.GetService<AttackableObjectApplicationService>();

        [SerializeField]
        private Hand.Shape shape;
        [SerializeField]
        private JankenSprites jankenSprites;
        [SerializeField]
        private float power;

        private Rigidbody2D rigid;

        public void SetUp(AttackableObjectId id)
        {
            this.id = id;

            rigid = GetComponent<Rigidbody2D>();
            Collider2D collider = GetComponent<Collider2D>();
            Renderer renderer = GetComponent<Renderer>();

            collider.OnTriggerEnter2DAsObservable()
                .Where(x => x.TryGetComponent(out IReceiveAttack i))
                .First()
                .Subscribe(x => { if (x.TryGetComponent(out IReceiveAttack i)) OnHit(i); })
                .AddTo(this);

            renderer.OnBecameInvisibleAsObservable()
                .Subscribe(_ => Destroy(gameObject))
                .AddTo(this);
        }
        private void OnDestroy()
        {
            if (id == null) return;
            attackableObjectApplicationService.Delete(id);
        }

        public void FixedUpdate()
        {
            rigid.AddForce(transform.up * power, ForceMode2D.Force);
        }

        private void OnHit(IReceiveAttack receiveAttack)
        {
            attackableObjectApplicationService.Attack(id, receiveAttack);
        }

        public Hand.Shape GetShape()
        {
            if (id == null) return shape;
            return attackableObjectApplicationService.Get(id).shape;
        }
        public void UpdateView(Hand.Shape shape)
        {
            if (jankenSprites.TryGetRenderer(out SpriteRenderer spriteRenderer, this))
            {
                spriteRenderer.sprite = jankenSprites.GetAsset(shape);
            }
        }
    }
}
