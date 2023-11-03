using System.Collections;
using UnityEngine;
using Ryocatusn.Janken;
using Ryocatusn.UI;

namespace Ryocatusn
{
    public class PlayerJankenChanger : MonoBehaviour
    {
        private Hand.Shape selectShape;
        private float rate = 1;

        [SerializeField]
        private PlayerJankenChangerUI playerJankenChangerUI;

        private void Start()
        {
            StartCoroutine(ChangeShape());
            
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

        public void ChangePlayerShape()
        {
            playerJankenChangerUI.ChangePlayerShape();
        }
    }
}
