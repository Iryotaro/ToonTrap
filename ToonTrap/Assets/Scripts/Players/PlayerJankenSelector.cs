using System.Collections;
using UnityEngine;
using Ryocatusn.Janken;
using Ryocatusn.UI;

namespace Ryocatusn
{
    public class PlayerJankenSelector : MonoBehaviour
    {
        private Hand.Shape selectShape;
        private float rate = 1;

        [SerializeField]
        private PlayerJankenSelectorUI jankenSelectorUI;

        private void Start()
        {
            StartCoroutine(ChangeSelectShape());
            
            IEnumerator ChangeSelectShape()
            {
                while (true)
                {
                    selectShape = Hand.GetNextShape(selectShape);
                    jankenSelectorUI.ChangeSelectShape(selectShape, rate);
                    yield return new WaitForSeconds(1 / rate);
                }
            }
        }

        public Hand.Shape GetSelectShape()
        {
            return selectShape;
        }
    }
}
