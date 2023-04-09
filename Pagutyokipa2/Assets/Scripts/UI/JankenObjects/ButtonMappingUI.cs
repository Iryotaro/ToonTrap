using DG.Tweening;
using Ryocatusn.Audio;
using Ryocatusn.Janken;
using UnityEngine;

namespace Ryocatusn.UI
{
    public class ButtonMappingUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject rock;
        [SerializeField]
        private GameObject scissors;
        [SerializeField]
        private GameObject paper;
        [SerializeField]
        private GameObject[] outsideArrows;
        [SerializeField]
        private SE jankenReverseSE;

        private SEPlayer sePlayer;

        private void Start()
        {
            sePlayer = new SEPlayer(gameObject);
        }
        public void HandleChangeShape(Hand.Shape shape)
        {
            if (shape == Hand.Shape.Rock) CreateHandTween(rock);
            else if (shape == Hand.Shape.Scissors) CreateHandTween(scissors);
            else if (shape == Hand.Shape.Paper) CreateHandTween(paper);
        }
        public void HandleJankenReverse(bool reverse)
        {
            foreach (GameObject outsideArrow in outsideArrows)
            {
                if (reverse) outsideArrow.transform.DORotate(new Vector3(0, 0, 180), 2, RotateMode.FastBeyond360).SetLink(outsideArrow);
                else outsideArrow.transform.DORotate(Vector3.zero, 2, RotateMode.FastBeyond360).SetLink(outsideArrow);
            }

            transform.DOScale(Vector3.one * 2, 0.5f).SetEase(Ease.OutBounce).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);

            sePlayer.Play(jankenReverseSE);
        }
        public void CreateHandTween(GameObject hand)
        {
            hand.transform.localScale = Vector3.one * 0.5f;
            hand.transform.DOScale(Vector3.one * 0.7f, 0.07f)
                .SetEase(Ease.OutSine)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => hand.transform.localScale = Vector3.one * 0.5f);
        }
    }
}
