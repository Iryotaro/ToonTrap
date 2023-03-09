using UnityEngine;

namespace Ryocatusn.Janken
{
    [ExecuteAlways()]
    [RequireComponent(typeof(IHandForSpriteEditor))]
    public class HandSpriteEditor : MonoBehaviour
    {
        private void Awake()
        {
            ChangeSprite();
        }
        private void Update()
        {
            if (Application.isPlaying) return;

            ChangeSprite();
        }

        private void ChangeSprite()
        {
            IHandForSpriteEditor hand = GetComponent<IHandForSpriteEditor>();
            foreach (SpriteRenderer spriteRenderer in hand.GetSpriteRenderers())
            {
                spriteRenderer.sprite = hand.GetHandShape() switch
                {
                    Hand.Shape.Rock => hand.GetHandSprites().rockSprite,
                    Hand.Shape.Scissors => hand.GetHandSprites().scissorsSprite,
                    Hand.Shape.Paper => hand.GetHandSprites().paperSprite,
                    _ => null
                };
            }
        }
    }
}
