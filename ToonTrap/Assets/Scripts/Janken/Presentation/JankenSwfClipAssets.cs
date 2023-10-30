using FTRuntime;
using System;
using UnityEngine;

namespace Ryocatusn.Janken
{
    [Serializable]
    public class JankenSwfClipAssets : IJankenAsset<SwfClipAsset, SwfClipController>
    {
        public SwfClipAsset rockSwfClipAsset;
        public SwfClipAsset scissorsSwfClipAsset;
        public SwfClipAsset paperSwfClipAsset;

        private SwfClipController swfClipController;

        public SwfClipAsset GetAsset(Hand.Shape shape)
        {
            return shape switch
            {
                Hand.Shape.Rock => rockSwfClipAsset,
                Hand.Shape.Scissors => scissorsSwfClipAsset,
                Hand.Shape.Paper => paperSwfClipAsset,
                _ => null
            };
        }

        public bool TryGetRenderer<T>(out SwfClipController renderer, T forJankenViewEditor) where T : MonoBehaviour
        {
            if (swfClipController != null)
            {
                renderer = swfClipController;
                return true;
            }

            if (forJankenViewEditor.TryGetComponent(out SwfClipController s))
            {
                swfClipController = s;
                renderer = swfClipController;
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
