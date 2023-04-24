using System;
using UnityEngine;

namespace Ryocatusn.Janken
{
    [Serializable]
    public class JankenSprites : IJankenAsset<Sprite, SpriteRenderer>
    {
        public Sprite rockSprite;
        public Sprite scissorsSprite;
        public Sprite paperSprite;

        private SpriteRenderer spriteRenderer;

        public Sprite GetAsset(Hand.Shape shape)
        {
            return shape switch
            {
                Hand.Shape.Rock => rockSprite,
                Hand.Shape.Scissors => scissorsSprite,
                Hand.Shape.Paper => paperSprite,
                _ => null
            };
        }
        public bool TryGetRenderer<T>(out SpriteRenderer renderer, T forJankenViewEditor) where T : MonoBehaviour
        {
            if (spriteRenderer != null)
            {
                renderer = spriteRenderer;
                return true;
            }

            if (forJankenViewEditor.TryGetComponent(out SpriteRenderer s))
            {
                spriteRenderer = s;
                renderer = spriteRenderer;
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
