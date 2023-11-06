using Ryocatusn.Janken;
using System;
using UnityEngine;

namespace Ryocatusn.Characters
{
    [Serializable]
    public class JankenTunnelAnimators : IJankenAsset<TunnelAnimator, GameObject>
    {
        public TunnelAnimator rockAnimator;
        public TunnelAnimator scissorsAnimator;
        public TunnelAnimator paperAnimator;

        public TunnelAnimator GetAsset(Hand.Shape shape)
        {
            return shape switch
            {
                Hand.Shape.Rock => rockAnimator,
                Hand.Shape.Scissors => scissorsAnimator,
                Hand.Shape.Paper => paperAnimator,
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
