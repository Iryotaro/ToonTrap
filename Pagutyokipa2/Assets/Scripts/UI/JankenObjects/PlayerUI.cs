using Photon.Pun.Demo.PunBasics;
using Ryocatusn.Janken.JankenableObjects;
using UniRx;
using UnityEngine;
using Zenject;

namespace Ryocatusn.UI
{
    public class PlayerUI : MonoBehaviour, IJankenableObjectUI
    {
        [Inject]
        private JankenableObjectApplicationService jankenableObjectApplicationService { get; }

        [SerializeField]
        private ButtonMappingUI buttonMappingUI;
        [SerializeField]
        private HPBar hpBar;
        [SerializeField]
        private TMPro.TextMeshProUGUI winComboText;

        public void Setup(JankenableObjectId id)
        {
            JankenableObjectEvents jankenableObjectEvents = jankenableObjectApplicationService.GetEvents(id);

            jankenableObjectEvents.ChangeShapeEvent
                .Subscribe(x => buttonMappingUI.HandleChangeShape(x))
                .AddTo(this);

            jankenableObjectEvents.JankenReverseEvent
                .Subscribe(x => buttonMappingUI.HandleJankenReverse(x))
                .AddTo(this);

            jankenableObjectEvents.ChangeHpEvent
                .Subscribe(x => hpBar.ChangeHp(x))
                .AddTo(this);

            jankenableObjectEvents.DoJankenEvent
                .Subscribe(_ => winComboText.text = jankenableObjectApplicationService.Get(id).winCombo.ToString())
                .AddTo(this);
        }
    }
}
