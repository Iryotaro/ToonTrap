using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Ryocatusn.UI
{
    public class GameBackground : MonoBehaviour
    {
        public Image image1;
        public Image image2;

        [SerializeField]
        private BackgroundColors rockColors;
        [SerializeField]
        private BackgroundColors scissorsColors;
        [SerializeField]
        private BackgroundColors paperColors;

        [SerializeField]
        private GameManager gameManager;

        private void Start()
        {
            gameManager.SetStageEvent.Subscribe(_ =>
            {
                BackgroundColors backgroundColors = Random.Range(1, 3 + 1) switch
                {
                    1 => rockColors,
                    2 => scissorsColors,
                    3 => paperColors,
                    _ => null
                };

                image1.color = backgroundColors.color1;
                image2.color = backgroundColors.color2;
            }).AddTo(this);
        }

        [System.Serializable]
        private class BackgroundColors
        {
            public Color color1;
            public Color color2;
        }
    }
}
