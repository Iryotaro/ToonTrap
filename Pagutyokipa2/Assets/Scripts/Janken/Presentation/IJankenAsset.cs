using Anima2D;
using System;
using UnityEngine;

namespace Ryocatusn.Janken
{
    public interface IJankenAsset<Asset, Renderer>
    {
        public Asset GetAsset(Hand.Shape shape);
        public bool TryGetRenderer<T>(out Renderer spriteMeshInstance, T forJankenViewEditor) where T : MonoBehaviour, IForJankenViewEditor;
    }
}
