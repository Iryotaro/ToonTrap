using System;
using UnityEngine;
using DG.Tweening;

namespace Ryocatusn
{
    public class ObjectMovement : MonoBehaviour
    {
        [SerializeField]
        private Point[] points;

        private void Start()
        {
            transform.position = points[0].position;

            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < points.Length; i++)
            {
                if (i == 0) continue;
                sequence.Append(transform.DOMove(points[i].position, points[i].duration).SetEase(Ease.Linear));
            }
            sequence.Append(transform.DOMove(points[0].position, points[0].duration).SetEase(Ease.Linear));
            sequence.SetLoops(-1, LoopType.Restart);
            sequence.SetLink(gameObject);
        }

        [Serializable]
        private class Point
        {
            public Vector3 position;
            public float duration;
        }
    }
}
