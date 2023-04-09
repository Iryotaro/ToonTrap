using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Ryocatusn.UI
{
    public class Title : MonoBehaviour
    {
        private InputMaster input;

        private RectTransform rectTransform;
        private Vector2 defaultLocalPosition;
        private Vector2 direction;
        private float speed;

        [SerializeField]
        private float maxSpeed;
        [SerializeField]
        private float returnSpeed;
        [SerializeField]
        private float range;

        private Subject<bool> hoverTitileEvent = new Subject<bool>();
        private Subject<Unit> stopEvent = new Subject<Unit>();

        public IObservable<bool> HoverTitileEvent => hoverTitileEvent;
        public IObservable<Unit> StopEvent;


        private void Awake()
        {
            input = new InputMaster();

            input.Title.Test.performed += ctx => direction = -ctx.ReadValue<Vector2>().normalized;
            input.Title.Test.performed += ctx => speed = ctx.ReadValue<Vector2>().magnitude;
            input.Title.Test.canceled += _ => stopEvent.OnNext(Unit.Default);

            StopEvent = stopEvent.Throttle(TimeSpan.FromSeconds(0.2f));

            StopEvent.Subscribe(_ => speed = 0);
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
            GetComponent<Button>().OnPointerEnterAsObservable().Subscribe(_ => hoverTitileEvent.OnNext(true));
            GetComponent<Button>().OnPointerExitAsObservable().Subscribe(_ => hoverTitileEvent.OnNext(false));

            HoverTitileEvent.Subscribe(x => HandleHoverTitle(x)).AddTo(this);

            rectTransform = GetComponent<RectTransform>();
            defaultLocalPosition = rectTransform.localPosition;
        }
        private void OnDestroy()
        {
            hoverTitileEvent.Dispose();
        }
        private void Update()
        {
            if (speed == 0)
            {
                rectTransform.localPosition = Vector3.MoveTowards(transform.localPosition, defaultLocalPosition, returnSpeed);
            }
            else
            {
                rectTransform.Translate(Mathf.Clamp(speed, 0, maxSpeed) * direction * Time.deltaTime);
                rectTransform.localPosition = new Vector3
                    (
                    Mathf.Clamp(rectTransform.localPosition.x, defaultLocalPosition.x - range / 2, defaultLocalPosition.x + range / 2),
                    Mathf.Clamp(rectTransform.localPosition.y, defaultLocalPosition.y - range / 2, defaultLocalPosition.y + range / 2),
                    0
                    );
            }
        }

        public void HandleHoverTitle(bool hover)
        {
            //DOTween.Kill(gameObject);

            //if (hover)
            //    transform.DOScale(Vector3.one * 0.8f, 0.5f).SetEase(Ease.InBack).SetLink(gameObject);
            //else
            //    transform.DOScale(Vector3.one, 1).SetLink(gameObject);
        }
    }
}
