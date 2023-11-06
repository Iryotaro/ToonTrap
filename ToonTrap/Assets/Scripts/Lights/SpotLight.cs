using DG.Tweening;
using Ryocatusn.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Ryocatusn.Lights
{
    [ExecuteAlways()]
    public class SpotLight : MonoBehaviour
    {
        public bool on = true;
        [Range(0, 1)]
        public float intensity;
        [Range(0, 1)]
        public float diff;
        [Range(0, 1)]
        public float extraIntensity;
        [Min(0.2f)]
        public float lightScale = 0.2f;
        public Distortion distortion;
        [SerializeField]
        private Transform normal;
        [SerializeField]
        private SpriteRenderer outer;
        [SerializeField]
        private SpriteRenderer middle;
        [SerializeField]
        private SpriteRenderer inner;
        [SerializeField]
        private SpriteShapeController extra;

        [SerializeField]
        private Transform extraPositionsAroundLight;
        [SerializeField]
        private Transform extraPositionAroundLight1;
        [SerializeField]
        private Transform extraPositionAroundLight2;

        private bool onExtra = false;
        private List<Vector2> extraPositions = new List<Vector2>();

        private void Start()
        {
            if (Application.isPlaying)
            {
                StartCoroutine(MakeDistoration());
                IEnumerator MakeDistoration()
                {
                    Vector3 defaultSize = transform.localScale * lightScale;

                    while (true)
                    {
                        bool completed = false;

                        Sequence sequence = DOTween.Sequence();
                        sequence
                            .SetLink(normal.gameObject)
                            .Append(normal.transform.DOScale(defaultSize + Vector3.one * distortion.GetStrength(), distortion.GetDuration()).SetEase(Ease.InOutQuad))
                            .Append(normal.transform.DOScale(defaultSize, distortion.GetDuration()).SetEase(Ease.InOutQuad))
                            .OnComplete(() => completed = true);

                        yield return new WaitUntil(() => completed);
                        yield return new WaitForFixedUpdate();
                    }
                }
            }
        }
        private void Update()
        {
            ChangeIntensity();

            if (on) TurnOn();
            else TurnOff();

            SetExtra();
        }

        private void ChangeIntensity()
        {
            ColorHSV outerColorHSV = outer.color.GetColorHSV();
            outer.color = new ColorHSV(outerColorHSV.h, outerColorHSV.s, 1 - (intensity - diff)).GetColor();

            ColorHSV middleColorHSV = middle.color.GetColorHSV();
            middle.color = new ColorHSV(middleColorHSV.h, middleColorHSV.s, 1 - intensity).GetColor();

            ColorHSV innerColorHSV = outer.color.GetColorHSV();
            inner.color = new ColorHSV(innerColorHSV.h, innerColorHSV.s, 1 - (intensity + diff)).GetColor();
        }

        private void TurnOn()
        {
            outer.enabled = true;
            middle.enabled = true;
            inner.enabled = true;
        }
        private void TurnOff()
        {
            outer.enabled = false;
            middle.enabled = false;
            inner.enabled = false;
            TurnOffExtra();
        }

        private void SetExtra()
        {
            if (!Application.isPlaying) return;

            if (onExtra)
            {
                extra.spriteShapeRenderer.material.SetFloat("_alpha", extraIntensity);

                extra.transform.position = Vector3.zero;

                float angle = MathF.Atan2(extraPositions[0].y - normal.position.y, extraPositions[0].x - normal.position.x) * Mathf.Rad2Deg + 90;
                extraPositionsAroundLight.transform.localEulerAngles = new Vector3(0, 0, angle);

                extra.spriteShapeRenderer.material.SetFloat("_angle", angle);

                for (int i = 0; i < 3; i++)
                {
                    extra.spline.SetPosition(i, extraPositions[i]);
                }
            }
            else
            {
                extra.spriteShapeRenderer.material.SetFloat("_alpha", 0);
            }
        }

        public void TurnOnExtra(Vector2 extraPosition)
        {
            onExtra = true;
            extraPositions = new List<Vector2> { extraPosition, extraPositionAroundLight1.position, extraPositionAroundLight2.position };
        }
        public void TurnOffExtra()
        {
            onExtra = false;
        }

        [Serializable]
        public class Distortion
        {
            [SerializeField]
            [Range(0, 10)]
            private float strength = 2;
            [SerializeField]
            [Range(0, 5)]
            private float randomStrength = 0;
            [SerializeField]
            [Range(0.1f, 5)]
            private float duration = 1;
            [SerializeField]
            [Range(0, 2)]
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