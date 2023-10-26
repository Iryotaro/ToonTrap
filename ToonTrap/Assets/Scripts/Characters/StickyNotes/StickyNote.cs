using UnityEngine;
using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using Ryocatusn.Janken.AttackableObjects;
using UniRx;
using Ryocatusn.Photographers;
using Zenject;

namespace Ryocatusn.Characters
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class StickyNote : JankenBehaviour, IPhotographerSubject
    {
        private Hand.Shape shape = Hand.Shape.Paper;
        [SerializeField]
        private float speed;
        [SerializeField, Min(1)]
        private int atk = 1;
        private bool finishToAttack;
        private Animator animator;
        private Rigidbody2D rigid;

        private Vector2 viewport;

        [SerializeField]
        private StickyNoteSniperScope sniperScope;

        [Inject]
        private PhotographerSubjectManager photographerSubjectManager;

        public int priority { get; } = 10;
        public int photographerCameraSize { get; } = 3;
        public Subject<Unit> showOnPhotographerEvent { get; }

        public void Setup(Vector2 viewport)
        {
            this.viewport = viewport;

            animator = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();

            Create(new Hp(1), shape);

            events.AttackTriggerEvent
                .Subscribe(x => HandleAttackTrigger(x.id, x.receiveAttacks[0]))
                .AddTo(this);

            sniperScope.ShotEvent
                .Subscribe(_ => animator.SetTrigger("Attack"))
                .AddTo(this);

            sniperScope.HitEvent
                .Subscribe(_ => finishToAttack = true)
                .AddTo(this);

            photographerSubjectManager.Save(this);

            AttackableObjectCreateCommand command = new AttackableObjectCreateCommand(id, shape, new Atk(atk));
            AttackTrigger(command, gameManager.gameContains.player);
        }
        private void OnDestroy()
        {
            photographerSubjectManager.Delete(this);
        }

        private void HandleAttackTrigger(AttackableObjectId id, IReceiveAttack receiveAttack)
        {
            sniperScope.Setup(id, receiveAttack);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        private void Update()
        {
            if (finishToAttack) MoveUp();
            Move();
            if (IsAllowedToDelete())
            {
                //エフェクトを見せるためにDestroyを遅らせてる
                Destroy(sniperScope.gameObject, 4);
                Destroy(gameObject);
            }
        }

        private void MoveUp()
        {
            rigid.AddForce(Vector2.up * 3);
        }
        private void Move()
        {
            Camera cam = gameManager.gameContains.gameCamera.camera;
            Vector3 target = cam.ViewportToWorldPoint(new Vector2(viewport.x, viewport.y));
            target = new Vector3(target.x, target.y, 0);
            Vector3 currentVelocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, target, ref currentVelocity, 1 / speed);
        }

        private bool IsAllowedToDelete()
        {
            if (!finishToAttack) return false;
            return gameManager.gameContains.gameCamera.IsOutSideOfCamera(gameObject);
        }
    }
}
