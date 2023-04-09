using Anima2D;
using System;
using UnityEngine;

namespace Ryocatusn.Janken
{
    [Serializable]
    public class JankenSpriteMeshes : IJankenAsset<SpriteMesh, SpriteMeshInstance>
    {
        public SpriteMesh rockSpriteMesh;
        public SpriteMesh scissorsSpriteMesh;
        public SpriteMesh paperSpriteMesh;

        private SpriteMeshInstance spriteMeshInstance;

        public SpriteMesh GetAsset(Hand.Shape shape)
        {
            return shape switch
            {
                Hand.Shape.Rock => rockSpriteMesh,
                Hand.Shape.Scissors => scissorsSpriteMesh,
                Hand.Shape.Paper => paperSpriteMesh,
                _ => null
            };
        }
        public bool TryGetRenderer<T>(out SpriteMeshInstance renderer, T forJankenViewEditor) where T : MonoBehaviour, IForJankenViewEditor
        {
            if (spriteMeshInstance != null)
            {
                renderer = spriteMeshInstance;
                return true;
            }

            if (forJankenViewEditor.TryGetComponent(out SpriteMeshInstance s))
            {
                spriteMeshInstance = s;
                renderer = spriteMeshInstance;
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
