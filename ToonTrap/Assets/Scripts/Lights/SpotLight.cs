using UnityEngine;
using Ryocatusn.Util;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections;

namespace Ryocatusn
{
    [ExecuteAlways()]
    public class SpotLight : MonoBehaviour
    {
        [Range(0, 1)]
        public float intencity;
        [Range(0, 1)]
        public float diff;
        [SerializeField]
        private Distortion distortion;
        [SerializeField]
        private Image outer;
        [SerializeField]
        private Image middle;
        [SerializeField]
        private Image inner;

        private void Start()
        {
            if (Application.isPlaying)
            {
                StartCoroutine(MakeDistoration());
                IEnumerator MakeDistoration()
                {
                    Vector3 defaultSize = transform.localScale;

                    while (true)
                    {
                        bool completed = false;

                        Sequence sequence = DOTween.Sequence();
                        sequence
                            .SetLink(gameObject)
                            .Append(transform.DOScale(defaultSize + Vector3.one * distortion.GetStrength(), distortion.GetDuration()).SetEase(Ease.InOutQuad))
                            .Append(transform.DOScale(defaultSize, distortion.GetDuration()).SetEase(Ease.InOutQuad))
                            .OnComplete(() => completed = true);

                        yield return new WaitUntil(() => completed);
                        yield return new WaitForFixedUpdate();
                    }
                }
            }
        }
        private void Update()
        {
            ColorHSV outerColorHSV = outer.color.GetColorHSV();
            outer.color = new ColorHSV(outerColorHSV.h, outerColorHSV.s, intencity - diff).GetColor();

            ColorHSV middleColorHSV = middle.color.GetColorHSV();
            middle.color = new ColorHSV(middleColorHSV.h, middleColorHSV.s, intencity).GetColor();

            ColorHSV innerColorHSV = outer.color.GetColorHSV();
            inner.color = new ColorHSV(innerColorHSV.h, innerColorHSV.s, intencity + diff).GetColor();
        }

        [Serializable]
        public class Distortion
        {
            [SerializeField][Range(0, 10)]
            private float strength = 2;
            [SerializeField][Range(0, 5)]
            private float randomStrength = 0;
            [SerializeField][Range(0.1f, 5)]
            private float duration = 1;
            [SerializeField][Range(0, 2)]
            private float randomDuration = 0.2f;

            public float GetStrength()
            {
                return (strength + UnityEngine.Random.Range(-(randomStrength / 2), randomStrength / 2)) / 10;
            }
            public float GetDuration()
            {
                return duration + UnityEngine.Random.Range(-(duration / 2), randomDuration / 2);
            }
        }
    }
}
