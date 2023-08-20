using Ryocatusn.Janken;
using System;
using UnityEngine;

namespace Ryocatusn.Characters
{
    [Serializable]
    public class JankenDragonAnimators : IJankenAsset<DragonAnimator, GameObject>
    {
        public DragonAnimator rockDragonAnimator;
        public DragonAnimator scissorsDragonAnimator;
        public DragonAnimator paperDragonAnimator;

        public DragonAnimator GetAsset(Hand.Shape shape)
        {
            return shape switch
            {
                Hand.Shape.Rock => rockDragonAnimator,
                Hand.Shape.Scissors => scissorsDragonAnimator,
                Hand.Shape.Paper => paperDragonAnimator,
                _ => null
            };
        }

        public bool TryGetRenderer<T>(out GameObject renderer, T forJankenViewEditor) where T : MonoBehaviour
        {
            renderer = forJankenViewEditor.gameObject;
            return renderer != null;
        }
    }
}
