using Anima2D;
using Ryocatusn.Janken;
using UnityEngine;
using UnityEngine.Playables;

namespace Ryocatusn.UI
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PlayerJankenChangerUI : MonoBehaviour
    {
        [SerializeField]
        private SpriteMeshInstance spriteMeshInstance;
        [SerializeField]
        private JankenSpriteMeshes jankenSpriteMeshes;
        [SerializeField]
        private JankenColors inkColors;

        private PlayableDirector playableDirector;

        [SerializeField]
        private Material inkRenderer;

        private void Start()
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        public void ChangeShape(Hand.Shape shape)
        {
            spriteMeshInstance.spriteMesh = jankenSpriteMeshes.GetAsset(shape);
        }
        public void ChangePlayerShape(Hand.Shape shape)
        {
            playableDirector.Play();
            inkRenderer.SetColor("_Color", inkColors.GetAsset(shape));
        }
    }
}
