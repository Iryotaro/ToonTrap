using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Ryocatusn.Janken.JankenableObjects;

namespace Ryocatusn.UI
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField]
        private Slider mainSlider;
        [SerializeField]
        private Slider subSlider;

        public void ChangeHp(Hp hp)
        {
            mainSlider.maxValue = hp.maxValue;
            subSlider.maxValue = hp.maxValue;

            mainSlider.value = hp.value;

            if (subSlider.value < hp.value)
            {
                subSlider.value = hp.value;
            }
            else
            {
                DOTween.To
                    (
                    () => subSlider.value,
                    x => subSlider.value = x,
                    hp.value,
                    0.5f
                    );
            }
        }
    }
}
