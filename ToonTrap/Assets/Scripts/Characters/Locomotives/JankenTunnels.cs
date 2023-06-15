using System;
using UnityEngine;
using Ryocatusn.Janken;

namespace Ryocatusn.Characters
{
    [Serializable]
    public class JankenTunnels : IJankenAsset<TunnelAnimator, GameObject>
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
