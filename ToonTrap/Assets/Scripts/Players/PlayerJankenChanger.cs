using System.Collections;
using UnityEngine;
using Ryocatusn.Janken;
using Ryocatusn.UI;
using Zenject;
using Ryocatusn.Games;
using UniRx;

namespace Ryocatusn
{
    public class PlayerJankenChanger : MonoBehaviour
    {
        private Hand.Shape selectShape;
        private float rate = 1;

        [SerializeField]
        private PlayerJankenChangerUI playerJankenChangerUI;

        [Inject]
        private GameManager gameManager;

        private void Start()
        {
            gameManager.SetStageEvent
                .FirstOrDefault()
                .Subscribe(_ => StartCoroutine(ChangeShape()))
                .AddTo(this);

            IEnumerator ChangeShape()
            {
                while (true)
                {
                    selectShape = Hand.GetNextShape(selectShape);
                    playerJankenChangerUI.ChangeShape(selectShape);
                    yield return new WaitForSeconds(1 / rate);
                }
            }
        }

        public Hand.Shape GetShape()
        {
            return selectShape;
        }

        public void ChangePlayerShape(Hand.Shape shape)
        {
            playerJankenChangerUI.ChangePlayerShape(shape);
        }
    }
}
