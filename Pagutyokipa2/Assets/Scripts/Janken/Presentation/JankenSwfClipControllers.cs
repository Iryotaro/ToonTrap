using UnityEngine;
using FTRuntime;
using System;

namespace Ryocatusn.Janken
{
    [Serializable]
    public class JankenSwfClipControllers : IJankenAsset<SwfClipController, GameObject>
    {
        public SwfClipController rockSwfClipController;
        public SwfClipController scissorsSwfClipController;
        public SwfClipController paperSwfClipController;

        public SwfClipController GetAsset(Hand.Shape shape)
        {
            return shape switch
            {
                Hand.Shape.Rock => rockSwfClipController,
                Hand.Shape.Scissors => scissorsSwfClipController,
                Hand.Shape.Paper => paperSwfClipController,
                _ => null
            };
        }
        public bool TryGetRenderer<T>(out GameObject renderer, T forJankenViewEditor) where T : MonoBehaviour
        {
            renderer = forJankenViewEditor.gameObject;
            return renderer != null;
        }
    }
}
