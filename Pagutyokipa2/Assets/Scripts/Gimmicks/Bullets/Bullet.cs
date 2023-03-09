using Microsoft.Extensions.DependencyInjection;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Ryocatusn.Util;
using Ryocatusn.Janken;
using Ryocatusn.Janken.AttackableObjects;
using Ryocatusn.Ryoseqs;
using Ryocatusn.Audio;

namespace Ryocatusn
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        public AttackableObjectId id { get; private set; }
        private AttackableObjectApplicationService attackableObjectApplicationService = Installer.installer.serviceProvider.GetService<AttackableObjectApplicationService>();

        private float speed;
        private float timeToDelete;

        private Option<IReceiveAttack> target { get; } = new Option<IReceiveAttack>(null);

        private Ryoseq moveRyoseq;

        [SerializeField]
        private HandSprites handSprite;

        [SerializeField]
        private SE reAttackSE;
        [SerializeField]
        private SE drawSE;

        public void Set(AttackableObjectId id, Transform ownerTransform, BulletParameter parameter, IReceiveAttack target)
        {
            this.id = id;
            speed = parameter.speed;
            timeToDelete = parameter.timeToDelete;

            this.target.Set(target);

            AttackableObjectEvents events = attackableObjectApplicationService.GetEvents(id);

            events.WinEvent.Subscribe(_ => Delete()).AddTo(this);
            events.DrawEvent.Subscribe(_ => Delete()).AddTo(this);
            events.ReAttackEvent.Subscribe(_ => Delete()).AddTo(this);
            events.OwnerDieEvent.Subscribe(_ => Delete()).AddTo(this);

            moveRyoseq = new Ryoseq();
            ISequence moveSequence = moveRyoseq.Create();

            float startTime = default;
            Vector2 startPosition = default;

            moveSequence
                .AddWhile(new SequenceWhileCommand(finish =>
                {
                    Move();
                    if (attackableObjectApplicationService.Get(id).allowedReAttack) finish();
                }))
                .Connect(new SequenceCommand(_ => { startTime = Time.fixedTime; startPosition = transform.position; }))
                .ConnectWhile(new SequenceWhileCommand(finish =>
                {
                    if (ownerTransform == null) return;
                    if (!Return(startTime, startPosition, ownerTransform)) finish();
                }))
                .Connect(new SequenceCommand(_ => attackableObjectApplicationService.ReAttack(id)));

            moveRyoseq.AddTo(this);

            gameObject.OnTriggerEnter2DAsObservable()
                .Where(x =>
                {
                    if (x.TryGetComponent(out IReceiveAttack receiveAttack))
                    {
                        if (IsAllowedAttack(receiveAttack)) return true;
                        return false;
                    }

                    return false;
                })
                .FirstOrDefault()
                .Subscribe(x =>
                {
                    if (x == null) return;
                    if (x.TryGetComponent(out IReceiveAttack receiveAttack))
                    {
                        if (receiveAttack == null) return;
                        attackableObjectApplicationService.Attack(id, receiveAttack);
                    }
                });

            ChangeSprite(attackableObjectApplicationService.Get(id).handId);

            Invoke(nameof(DeleteByInvoke), timeToDelete);

            SEPlayer sePlayer = new SEPlayer(gameObject);

            events.ReAttackTriggerEvent
                .Subscribe(_ => sePlayer.Play(reAttackSE))
                .AddTo(this);

            events.DrawEvent
                .Subscribe(_ => sePlayer.Play(drawSE))
                .AddTo(this);
        }

        private void ChangeSprite(HandId id)
        {
            HandApplicationService handApplicationService = Installer.installer.serviceProvider.GetService<HandApplicationService>();
            Hand.Shape shape = handApplicationService.Get(id).shape;

            switch (shape)
            {
                case Hand.Shape.Rock:
                    GetComponent<SpriteRenderer>().sprite = handSprite.rockSprite;
                    return;
                case Hand.Shape.Scissors:
                    GetComponent<SpriteRenderer>().sprite = handSprite.scissorsSprite;
                    return;
                case Hand.Shape.Paper:
                    GetComponent<SpriteRenderer>().sprite = handSprite.paperSprite;
                    return;
            }
        }

        private void Move()
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
        private bool Return(float startTime, Vector2 startPosition, Transform destination)
        {
            if (GetTime() > 1) return false;

            float rot = Mathf.Atan2(destination.position.y - transform.position.y, destination.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
            transform.eulerAngles = new Vector3(0, 0, rot);

            transform.position = Vector3.Lerp(startPosition, destination.position, GetTime());
            return true;

            float GetTime()
            {
                return (Time.fixedTime - startTime) * 3;
            }
        }

        private void DeleteByInvoke()
        {
            if (attackableObjectApplicationService.Get(id).allowedReAttack) return;

            Delete();
        }

        private bool IsAllowedAttack(IReceiveAttack receiveAttack)
        {
            if (!attackableObjectApplicationService.IsEnable(id)) return false;
            if (receiveAttack == null) return false;
            if (receiveAttack.id.Equals(attackableObjectApplicationService.Get(id).ownerId)) return false;
            if (target.Get() == null) return true;
            if (target.Get().Equals(receiveAttack)) return true;
            return false;
        }

        private void Update()
        {
            if (!moveRyoseq.IsCompleted()) moveRyoseq.MoveNext();
        }

        private void Delete()
        {
            attackableObjectApplicationService.Delete(id);
            Destroy(gameObject);
        }
    }
}
