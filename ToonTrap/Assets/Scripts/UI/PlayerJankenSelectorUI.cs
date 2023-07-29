using Ryocatusn.Janken;
using System;
using UnityEngine;
using DG.Tweening;

namespace Ryocatusn.UI
{
    public class PlayerJankenSelectorUI : MonoBehaviour 
    {
        [SerializeField]
        private GameObject arrow;

        public void ChangeSelectShape(Hand.Shape shape, float rate)
        {
            int sequenceNumber = Array.IndexOf(Hand.GetSequence(), shape);
            int angle = sequenceNumber * -120 + 120;

            arrow.transform.DORotate(new Vector3(0, 0, angle), 1 / rate).SetEase(Ease.OutExpo);
        }
    }
}
