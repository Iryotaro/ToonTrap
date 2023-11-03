using UnityEngine;
using Ryocatusn.Janken;
using Anima2D;

namespace Ryocatusn.UI
{
    [RequireComponent(typeof(SpriteMeshInstance))]
    [RequireComponent(typeof(Animator))]
    public class PlayerJankenChangerUI : MonoBehaviour
    {
        [SerializeField]
        private JankenSpriteMeshes jankenSpriteMeshes;

        private SpriteMeshInstance spriteMeshInstance;
        private Animator animator;

        private void Start()
        {
            spriteMeshInstance = GetComponent<SpriteMeshInstance>();
            animator = GetComponent<Animator>();
        }

        public void ChangeShape(Hand.Shape shape)
        {
            spriteMeshInstance.spriteMesh = jankenSpriteMeshes.GetAsset(shape);
        }
        public void ChangePlayerShape()
        {
            animator.SetTrigger("Change");
        }
    }
}
