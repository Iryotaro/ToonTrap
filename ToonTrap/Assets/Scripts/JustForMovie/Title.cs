using UnityEngine;
using DG.Tweening;

namespace Ryocatusn
{
    public class Title : MonoBehaviour
    {
        [SerializeField]
        private GameObject start;

        private void Start()
        {
            Invoke(nameof(GameStart), 2);
        }
        public void GameStart()
        {
            start.transform.DOScale(new Vector2(1.3f, 1.3f), 0.07f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo).SetLink(start);
            //Transition.LoadScene("Title", "Game", new TransitionSettings(Janken.Hand.Shape.Paper));
        }
    }
}
