using System;
using UnityEngine;

namespace Ryocatusn.Janken
{
    [Serializable]
    public class JankenPrefabs : IJankenAsset<GameObject, GameObject>
    {
        public GameObject rockPrefab;
        public GameObject scissorsPrefab;
        public GameObject paperPrefab;

        public GameObject GetAsset(Hand.Shape shape)
        {
            return shape switch
            {
                Hand.Shape.Rock => rockPrefab,
                Hand.Shape.Scissors => scissorsPrefab,
                Hand.Shape.Paper => paperPrefab,
                _ => null
            };
        }
        public bool TryGetRenderer<T>(out GameObject renderer, T forJankenViewEditor) where T : MonoBehaviour, IForJankenViewEditor
        {
            renderer = forJankenViewEditor.gameObject;
            return renderer != null;
        }
    }
}
