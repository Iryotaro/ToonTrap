using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Ryocatusn.Util;

namespace Ryocatusn.UI
{
    public class TitleUIController : MonoBehaviour
    {
        private InputMaster input;

        [SerializeField]
        private Button title;
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button settingsButton;
        [SerializeField]
        private Button deleteButton;
        [SerializeField]
        private Button m_exitButton;

        private bool exitButtonClicked = false;
        private Vector2 mousePosition;
        private bool enableDeleteButton = true;

        private Option<Button> exitButton = new Option<Button>(null);

        private Subject<Unit> clickTitleEvent = new Subject<Unit>();
        private Subject<Unit> clickStartButtonEvent = new Subject<Unit>();
        private Subject<Unit> clickSettingsButtonEvent = new Subject<Unit>();
        private Subject<Unit> lostDeleteButtonEvent = new Subject<Unit>();
        private Subject<Unit> clickDeleteButtonEvent = new Subject<Unit>();
        private Subject<bool> enableDeleteButtonEvent = new Subject<bool>();
        private Subject<Vector2> escapeExitButtonEvent = new Subject<Vector2>();
        private Subject<Unit> lostExitButtonEvent = new Subject<Unit>();
        private Subject<Unit> clickExitButtonEvent = new Subject<Unit>();

        public IObservable<Unit> ClickTitleEvent => clickTitleEvent;
        public IObservable<Unit> ClickStartButtonEvent => clickStartButtonEvent;
        public IObservable<Unit> ClickSettingsButtonEvent => clickSettingsButtonEvent;
        public IObservable<Unit> LostDeleteButtonEvent => lostDeleteButtonEvent;
        public IObservable<Unit> ClickDeleteButtonEvent => clickDeleteButtonEvent;
        public IObservable<bool> EnableDeleteButtonEvent => enableDeleteButtonEvent;
        public IObservable<Vector2> EscapeExitButtonEvent => escapeExitButtonEvent;
        public IObservable<Unit> LostExitButtonEvent => lostExitButtonEvent;
        public IObservable<Unit> ClickExitButtonEvent => clickExitButtonEvent;


        private void Awake()
        {
            input = new InputMaster();

            input.Title.MousePosition.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        }
        private void OnEnable()
        {
            input.Title.Enable();
        }
        private void OnDisable()
        {
            input.Title.Disable();
        }
        private void Start()
        {
            title.OnClickAsObservable().Subscribe(_ => clickTitleEvent.OnNext(Unit.Default));

            startButton.OnClickAsObservable().Subscribe(_ => clickStartButtonEvent.OnNext(Unit.Default));

            settingsButton.OnClickAsObservable().Subscribe(_ => clickSettingsButtonEvent.OnNext(Unit.Default));

            deleteButton.OnClickAsObservable().Where(_ => enableDeleteButton).Subscribe(_ => clickDeleteButtonEvent.OnNext(Unit.Default));
            deleteButton.GetComponent<Renderer>().OnBecameInvisibleAsObservable()
                .Subscribe(_ => lostDeleteButtonEvent.OnNext(Unit.Default));

            exitButton.Set(m_exitButton);
            exitButton.Match
                (
                Some: x =>
                {
                    x.OnPointerDownAsObservable().Subscribe(_ =>
                    {
                        clickExitButtonEvent.OnNext(Unit.Default);
                        exitButtonClicked = true;
                    });

                    x.OnPointerDownAsObservable().FirstOrDefault().Subscribe(_ =>
                    {
                        enableDeleteButtonEvent.OnNext(false);
                        enableDeleteButton = false;
                    });

                    GameObject thisGameObject = gameObject;
                    x.GetComponent<Renderer>().OnBecameInvisibleAsObservable()
                    .Where(_ => thisGameObject != null)
                    .Subscribe(_ =>
                    {
                        lostExitButtonEvent.OnNext(Unit.Default);
                        enableDeleteButtonEvent.OnNext(true);
                        enableDeleteButton = true;
                    });
                }
                );
        }
        private void OnDestroy()
        {
            clickTitleEvent.Dispose();
            clickStartButtonEvent.Dispose();
            lostDeleteButtonEvent.Dispose();
            clickDeleteButtonEvent.Dispose();
            enableDeleteButtonEvent.Dispose();
            escapeExitButtonEvent.Dispose();
            lostExitButtonEvent.Dispose();
            clickExitButtonEvent.Dispose();
        }
        private void Update()
        {
            EscapeExitButton();
        }
        private void EscapeExitButton()
        {
            if (!exitButtonClicked) return;

            escapeExitButtonEvent.OnNext(Camera.main.ScreenToWorldPoint(mousePosition));
        }
    }
}
