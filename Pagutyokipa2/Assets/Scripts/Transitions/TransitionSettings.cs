using UnityEngine;
using Ryocatusn.Janken;

namespace Ryocatusn
{
    public class TransitionSettings
    {
        private bool defaultPosition { get; }
        private Transform focusTransform { get; }
        private Camera camera { get; }
        public Hand.Shape shape { get; }

        public static TransitionSettings Default()
        {
            return new TransitionSettings();
        }

        public TransitionSettings(Transform focusTransform, Camera camera)
        {
            defaultPosition = false;
            this.focusTransform = focusTransform;
            this.camera = camera;
            shape = Random.Range(1, 3 + 1) switch
            {
                1 => Hand.Shape.Rock,
                2 => Hand.Shape.Scissors,
                3 => Hand.Shape.Paper,
                _ => default
            };
        }
        public TransitionSettings(Transform focusTransform, Camera camera, Hand.Shape shape)
        {
            defaultPosition = false;
            this.focusTransform = focusTransform;
            this.camera = camera;
            this.shape = shape;
        }
        private TransitionSettings()
        {
            defaultPosition = true;
            shape = Random.Range(1, 3 + 1) switch
            {
                1 => Hand.Shape.Rock,
                2 => Hand.Shape.Scissors,
                3 => Hand.Shape.Paper,
                _ => default
            };
        }

        public void SetPosition(RectTransform rectTransform)
        {
            if (defaultPosition)
            {
                rectTransform.localPosition = Vector3.zero;
            }
            else
            {
                if (focusTransform == null || camera == null) return;

                Vector3 position = Vector2.zero;
                Vector2 screenPoint = camera.WorldToScreenPoint(focusTransform.position);
                RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, null, out position);
                rectTransform.position = position;
            }
        }
    }
}
