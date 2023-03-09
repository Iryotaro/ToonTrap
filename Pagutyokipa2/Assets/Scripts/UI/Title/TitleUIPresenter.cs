using UniRx;
using UnityEngine;

namespace Ryocatusn.UI
{
    [RequireComponent(typeof(TitleUIController))]
    [RequireComponent(typeof(TitleUIView))]
    public class TitleUIPresenter : MonoBehaviour
    {
        private TitleUIController controller;
        private TitleUIView view;

        private void Start()
        {
            controller = GetComponent<TitleUIController>();
            view = GetComponent<TitleUIView>();

            controller.ClickTitleEvent
                .Subscribe(_ => view.HandleClickTitle())
                .AddTo(this);

            controller.ClickStartButtonEvent
                .Subscribe(_ => view.HandleClickStartButton())
                .AddTo(this);

            controller.ClickSettingsButtonEvent
                .Subscribe(_ => view.HandleClickSettingsButton())
                .AddTo(this);

            controller.ClickDeleteButtonEvent
                .Subscribe(_ => view.HandleClickDeleteButton())
                .AddTo(this);

            controller.LostDeleteButtonEvent
                .Subscribe(_ => view.HandleLostDeleteButton())
                .AddTo(this);

            controller.EnableDeleteButtonEvent
                .Subscribe(x => view.HandleEnableDeleteButton(x))
                .AddTo(this);

            controller.ClickExitButtonEvent
                .Subscribe(_ => view.HandleClickExitButton())
                .AddTo(this);

            controller.EscapeExitButtonEvent
                .Subscribe(x => view.HandleEscapeExitButton(x))
                .AddTo(this);

            controller.LostExitButtonEvent
                .Subscribe(_ => view.HandleLostExitButton());
        }
    }
}
