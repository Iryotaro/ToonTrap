using System;
using UnityEngine;

namespace Ryocatusn.Janken
{
    [Serializable]
    public class JankenColors : IJankenAsset<Color, Renderer>
    {
        public Color rockColor;
        public Color scissorsColor;
        public Color paperColor;

        private Renderer renderer;

        public Color GetAsset(Hand.Shape shape)
        {
            return shape switch
            {
                Hand.Shape.Rock => rockColor,
                Hand.Shape.Scissors => scissorsColor,
                Hand.Shape.Paper => paperColor,
                _ => Color.white
            };
        }
        public bool TryGetRenderer<T>(out Renderer renderer, T forJankenViewEditor) where T : MonoBehaviour
        {
            if (this.renderer != null)
            {
                renderer = this.renderer;
                return true;
            }

            if (forJankenViewEditor.TryGetComponent(out SpriteRenderer s))
            {
                this.renderer = s;
                renderer = this.renderer;
                return true;
            }
            else
            {
                renderer = null;
                return false;
            }
        }
    }
}
