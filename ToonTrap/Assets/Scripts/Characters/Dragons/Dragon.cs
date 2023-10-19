using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using UnityEngine;
using UniRx;
using System;
using Ryocatusn.Janken.AttackableObjects;
using Zenject;

namespace Ryocatusn.Characters
{
    public class Dragon : JankenBehaviour, IForJankenViewEditor, IReceiveAttack
    {
        [SerializeField]
        private int hp;
        [SerializeField]
        private JankenDragonAnimators jankenDragonAnimators;
        [NonSerialized]
        public DragonAnimator dragonAnimator;
        [SerializeField]
        private AppearType appearType;
        [SerializeField]
        private Bullet bullet;
        [SerializeField]
        private Transform shotPoint;

        [Inject]
        private BulletFactory bulletFactory;

        public enum AppearType
        {
            FirstAppearance,
            Appear
        }

        public bool isAllowedToReceiveAttack { get; } = true;

        private void Start()
        {
            //Hand.Shape shape = Hand.GetRandomShape();
            //Create(new Hp(hp), shape);
            Create(new Hp(hp), Hand.Shape.Paper);

            gameManager.SetStageEvent
                .Subscribe(_ => { if (appearType == AppearType.FirstAppearance) dragonAnimator.ShowAppearanceFirstFrame(); })
                .AddTo(this);

            events.AttackTriggerEvent
                .Subscribe(x => Shot(x.id))
                .AddTo(this);
        }

        public void Appear()
        {
            if (dragonAnimator == null) return;

            if (appearType == AppearType.Appear) dragonAnimator.PlayAnimation(DragonAnimator.AnimationType.Appear);
            else dragonAnimator.PlayAnimations(new DragonAnimator.AnimationType[3] 
            {
                DragonAnimator.AnimationType.FirstAppearance,
                DragonAnimator.AnimationType.Provocation,
                DragonAnimator.AnimationType.Disappear,
            });
        }
        
        private void Attack()
        {
            AttackableObjectCreateCommand command = new AttackableObjectCreateCommand(id, GetShape(), new Atk(1));
            AttackTrigger(command);
        }
        private void Shot(AttackableObjectId id)
        {
            bulletFactory.Create(bullet, id, gameObject, shotPoint.position, gameManager.gameContains.player.transform);
        }

        public JankenableObjectId GetId()
        {
            return id;
        }

        public Hand.Shape GetShape()
        {
            if (id == null) return Hand.Shape.Paper;
            else return jankenableObjectApplicationService.Get(id).shape;
        }

        public void UpdateView(Hand.Shape shape)
        {
            if (jankenDragonAnimators.TryGetRenderer(out GameObject gameObject, this))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    GameObject childObject = transform.GetChild(i).gameObject;

                    if (!childObject.TryGetComponent(out DragonAnimator anime)) continue;

                    if (Application.isPlaying) Destroy(childObject);
                    else DestroyImmediate(childObject);
                }
                DragonAnimator dragonAnimator = jankenDragonAnimators.GetAsset(shape);
                dragonAnimator.transform.position = Vector3.zero;
                this.dragonAnimator = Instantiate(dragonAnimator, gameObject.transform);

                this.dragonAnimator.AttackSubject
                    .Subscribe(_ => Attack())
                    .AddTo(this);
            }
        }
    }
}
