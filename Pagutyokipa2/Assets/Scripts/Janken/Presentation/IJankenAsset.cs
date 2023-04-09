using UnityEngine;

namespace Ryocatusn.Janken
{
    public interface IJankenAsset<Asset, Renderer>
    {
        public Asset GetAsset(Hand.Shape shape);
        public bool TryGetRenderer<T>(out Renderer renderer, T forJankenViewEditor) where T : MonoBehaviour, IForJankenViewEditor;
    }
}
