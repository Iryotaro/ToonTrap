using UnityEngine;

namespace Ryocatusn.Janken
{
    [ExecuteAlways()]
    [RequireComponent(typeof(IForJankenViewEditor))]
    [DefaultExecutionOrder(10)]
    public class JankenViewEditor : MonoBehaviour
    {
        private IForJankenViewEditor forJankenViewEditor;
        private Hand.Shape? shape = null;

        private void Awake()
        {
            if (Application.isPlaying) return;

            ChangeSprite();
        }
        private void Update()
        {
            if (Application.isPlaying) return;

            ChangeSprite();
        }

        private void Start()
        {
            if (!Application.isPlaying) return;

            ChangeSprite();
        }

        public void ChangeSprite()
        {
            if (forJankenViewEditor == null) forJankenViewEditor = GetComponent<IForJankenViewEditor>();
            if (forJankenViewEditor == null) return;

            Hand.Shape shape = forJankenViewEditor.GetShape();
            if (this.shape == shape) return;
            forJankenViewEditor.UpdateView(shape);
            this.shape = shape;
        }
    }
}
