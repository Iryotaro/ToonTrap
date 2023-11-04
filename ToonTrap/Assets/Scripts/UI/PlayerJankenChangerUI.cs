using UnityEngine;
using Ryocatusn.Janken;
using Anima2D;
using DG.Tweening;

namespace Ryocatusn.UI
{
    [RequireComponent(typeof(SpriteMeshInstance))]
    [RequireComponent(typeof(Animator))]
    public class PlayerJankenChangerUI : MonoBehaviour
    {
        [SerializeField]
        private JankenSpriteMeshes jankenSpriteMeshes;
        [SerializeField]
        private SpriteRenderer ink;
        [SerializeField]
        private JankenColors inkColors;

        [SerializeField]
        private Transform shotPoint;
        [SerializeField]
        private Transform endPoint;

        private SpriteMeshInstance spriteMeshInstance;
        private Animator animator;

        private Hand.Shape shape;

        private void Start()
        {
            spriteMeshInstance = GetComponent<SpriteMeshInstance>();
            animator = GetComponent<Animator>();
        }

        public void ChangeShape(Hand.Shape shape)
        {
            spriteMeshInstance.spriteMesh = jankenSpriteMeshes.GetAsset(shape);
        }
        public void ChangePlayerShape(Hand.Shape shape)
        {
            animator.Play("Change", 0, 0);
            this.shape = shape;
        }

        public void FadeInkCallbackFromAnimation()
        {
            SpriteRenderer ink = Instantiate(this.ink, shotPoint.transform.position, Quaternion.identity);
            ink.material.SetColor("_color", inkColors.GetAsset(shape));

            ink.material.SetFloat("_fader", 1.2f);

            Sequence sequence = DOTween.Sequence();
            sequence
                .SetLink(ink.gameObject)
                .Append(ink.transform.DOMove(endPoint.transform.position, 0.3f).SetEase(Ease.InSine))
                .Join(ink.transform.DOScale(Vector3.one * 5, 0.5f).SetEase(Ease.InSine))
                .AppendInterval(0.2f)
                .Join(DoFade(ink.material, 0, 0.3f));

            Tween DoFade(Material material, float endValue, float duration)
            {
                return DOTween.To
                    (
                    () => material.GetFloat("_fader"),
                    x => material.SetFloat("_fader", x),
                    0,
                    1
                    );
            }
        }
    }
}
