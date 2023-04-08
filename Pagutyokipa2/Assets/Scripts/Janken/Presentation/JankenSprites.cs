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
        public bool TryGetRenderer<T>(out SpriteRenderer spriteRenderer, T forJankenViewEditor) where T : MonoBehaviour, IForJankenViewEditor
        {
            if (this.spriteRenderer != null)
            {
                spriteRenderer = this.spriteRenderer;
                return true;
            }

            if (forJankenViewEditor.TryGetComponent(out SpriteRenderer s))
            {
                this.spriteRenderer = s;
                spriteRenderer = this.spriteRenderer;
                return true;
            }
            else
            {
                spriteRenderer = null;
                return false;
            }
        }
    }
}
