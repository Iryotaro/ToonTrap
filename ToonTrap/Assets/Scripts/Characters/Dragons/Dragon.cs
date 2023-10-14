using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using UnityEngine;
using UniRx;

namespace Ryocatusn.Characters
{
    public class Dragon : JankenBehaviour, IForJankenViewEditor, IReceiveAttack
    {
        [SerializeField]
        private int hp;

        [SerializeField]
        private JankenDragonAnimators jankenDragonAnimators;

        private DragonAnimator dragonAnimator;

        [SerializeField]
        private AppearType appearType;

        public enum AppearType
        {
            FirstAppearance,
            Appear
        }

        public bool isAllowedToReceiveAttack { get; } = true;

        private void Start()
        {
            Hand.Shape shape = Hand.GetRandomShape();
            Create(new Hp(hp), shape);

            gameManager.SetStageEvent
                .Subscribe(_ => { if (appearType == AppearType.FirstAppearance) dragonAnimator.ShowAppearanceFirstFrame(); })
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
                    if (Application.isPlaying) Destroy(transform.GetChild(i).gameObject);
                    else DestroyImmediate(transform.GetChild(i).gameObject);
                }
                DragonAnimator dragonAnimator = jankenDragonAnimators.GetAsset(shape);
                this.dragonAnimator = Instantiate(dragonAnimator, gameObject.transform);
            }
        }
    }
}
