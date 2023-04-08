using System;
using Anima2D;
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
        public bool TryGetRenderer<T>(out SpriteMeshInstance spriteMeshInstance, T forJankenViewEditor) where T : MonoBehaviour, IForJankenViewEditor
        {
            if (this.spriteMeshInstance != null)
            {
                spriteMeshInstance = this.spriteMeshInstance;
                return true;
            }

            if (forJankenViewEditor.TryGetComponent(out SpriteMeshInstance s))
            {
                this.spriteMeshInstance = s;
                spriteMeshInstance = this.spriteMeshInstance;
                return true;
            }
            else
            {
                spriteMeshInstance = null;
                return false;
            }
        }
    }
}
