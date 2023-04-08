using UnityEngine;

namespace Ryocatusn.Janken
{
    [ExecuteAlways()]
    [RequireComponent(typeof(IForJankenViewEditor))]
    public class JankenViewEditor : MonoBehaviour
    {
        private IForJankenViewEditor forJankenViewEditor;

        private void Awake()
        {
            ChangeSprite();
        }
        private void Update()
        {
            if (Application.isPlaying) return;

            ChangeSprite();
        }

        public void ChangeSprite()
        {
            if (forJankenViewEditor == null) forJankenViewEditor = GetComponent<IForJankenViewEditor>();
            if (forJankenViewEditor == null) return;

            Hand.Shape shape = forJankenViewEditor.GetShape();
            forJankenViewEditor.UpdateView(shape);
        }
    }
}
