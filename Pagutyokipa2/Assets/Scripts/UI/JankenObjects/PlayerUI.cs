using Microsoft.Extensions.DependencyInjection;
using Ryocatusn.Janken.JankenableObjects;
using UniRx;
using UnityEngine;

namespace Ryocatusn.UI
{
    public class PlayerUI : MonoBehaviour, IJankenableObjectUI
    {
        private JankenableObjectApplicationService jankenableObjectApplicationService = Installer.installer.serviceProvider.GetService<JankenableObjectApplicationService>();

        [SerializeField]
        private ButtonMappingUI buttonMappingUI;
        [SerializeField]
        private HPBar hpBar;
        [SerializeField]
        private TMPro.TextMeshProUGUI winComboText;
        [SerializeField]
        private GameManager gameManager;

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
