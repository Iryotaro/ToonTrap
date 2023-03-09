using UnityEngine;

namespace Ryocatusn.Janken
{
    public interface IHandForSpriteEditor
    {
        SpriteRenderer[] GetSpriteRenderers();
        Hand.Shape GetHandShape();
        HandSprites GetHandSprites();
    }
}
