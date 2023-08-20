using Ryocatusn.Janken;
using Ryocatusn.Janken.JankenableObjects;
using UnityEngine;
using System;

namespace Ryocatusn.Characters
{
    public class Dragon : JankenBehaviour, IForJankenViewEditor, IReceiveAttack
    {
        [SerializeField]
        private int hp;

        [SerializeField]
        private JankenDragonAnimators jankenDragonAnimators;

        private DragonAnimator dragonAnimator;

        public bool isAllowedToReceiveAttack { get; } = true;

        private void Start()
        {
            Hand.Shape shape = Hand.GetRandomShape();
            Create(new Hp(hp), shape);
        }

        public bool FirstAppearance(out Action appearAction)
        {
            appearAction = null;
            if (dragonAnimator == null) return false;

            dragonAnimator.ShowFirstAppearanceFirstFrame();

            Action finishFirstAppearance = () =>
            {
                dragonAnimator.PlayAnimation(DragonAnimator.AnimationType.Provocation);
            };

            appearAction = () => dragonAnimator.PlayAnimation(DragonAnimator.AnimationType.FirstAppearance, finishFirstAppearance);
            return true;
        }
        public void Appear()
        {
            if (dragonAnimator == null) return;

            dragonAnimator.PlayAnimation(DragonAnimator.AnimationType.Appear);
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
