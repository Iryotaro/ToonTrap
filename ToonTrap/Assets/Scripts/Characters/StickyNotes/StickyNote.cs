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
    public class StickyNote : JankenBehaviour, IPhotographerSubject
    {
        private Hand.Shape shape = Hand.Shape.Paper;
        [SerializeField, Min(1)]
        private int atk = 1;
        private bool finishToShot;
        private Animator animator;

        [SerializeField]
        private StickyNoteSniperScope sniperScope;

        [Inject]
        private PhotographerSubjectManager photographerSubjectManager;

        public int priority { get; } = 10;
        public int photographerCameraSize { get; } = 3;
        public Subject<Unit> showOnPhotographerEvent { get; }

        public void Setup()
        {
            animator = GetComponent<Animator>();

            Create(new Hp(1), shape);

            events.AttackTriggerEvent
                .Subscribe(x => HandleAttackTrigger(x.id, x.receiveAttacks[0]))
                .AddTo(this);

            sniperScope.ShotSubject
                .Subscribe(_ => HandleShot())
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
        private void HandleShot()
        {
            animator.SetTrigger("Attack");
            finishToShot = true;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        private void Update()
        {
            Move();
            if (IsAllowedToDelete()) Destroy(gameManager);
        }
        private void Move()
        {
            transform.Translate(3 * Time.deltaTime, 0, 0);
        }
        private bool IsAllowedToDelete()
        {
            if (!finishToShot) return false;
            return gameManager.gameContains.gameCamera.IsOutSideOfCamera(gameObject);
        }
    }
}
