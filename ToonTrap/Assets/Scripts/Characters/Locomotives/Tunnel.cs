using Ryocatusn.Janken;
using System;
using UnityEngine;
using UniRx;
using Zenject;
using Ryocatusn.Photographers;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Ryocatusn.Characters
{
    public class Tunnel : MonoBehaviour, IPhotographerSubject
    {
        [SerializeField]
        private LocomotiveData[] datas;
        [SerializeField, Min(0.5f)]
        private float rateScale = 1;
        [SerializeField]
        private JankenTunnelAnimators jankenTunnelAnimators;
        [SerializeField]
        private Railway railway;
        [SerializeField]
        private Locomotive locomotive;

        private Subject<Unit> playEvent = new Subject<Unit>();

        private bool isRepeating = false;

        [Inject]
        private DiContainer diContainer;
        [Inject]
        private PhotographerSubjectManager photographerSubjectManager;

        public int priority { get; } = 0;

        public int photographerCameraSize { get; } = 3;

        public Subject<Unit> showOnPhotographerEvent { get; } = new Subject<Unit>();

        private void Start()
        {
            photographerSubjectManager.Save(this);

            playEvent
                .ThrottleFirst(TimeSpan.FromSeconds(5 / rateScale))
                .Subscribe(_ =>
                {
                    Hand.Shape shape = Hand.GetRandomShape();
                    Action action;
                    action = () => CreateLocomotive(shape);
                    PlayAnimation(shape, action);
                })
                .AddTo(this);

            showOnPhotographerEvent
                .DelaySubscription(TimeSpan.FromSeconds(1.5f))
                .Subscribe(_ => Play())
                .AddTo(this);
        }
        private void OnDestroy()
        {
            photographerSubjectManager.Delete(this);
        }

        private void Update()
        {
            if (isRepeating) Play();
        }

        public void Play()
        {
            playEvent.OnNext(Unit.Default);
        }

        public void Repeat()
        {
            isRepeating = true;
        }
        public void StopRepeat()
        {
            isRepeating = false;
        }

        private Locomotive CreateLocomotive(Hand.Shape shape)
        {
            Locomotive newLocomotive = Instantiate(locomotive, transform.parent);
            diContainer.InjectGameObject(newLocomotive.gameObject);

            newLocomotive.SetUp(shape, railway, datas[UnityEngine.Random.Range(0, datas.Length)]);
            return newLocomotive;
        }
        private void PlayAnimation(Hand.Shape shape, Action CreateLocomotiveEvent)
        {
            TunnelAnimator tunnelAnimator = CreateController();
            if (tunnelAnimator == null) return;

            tunnelAnimator.ChangeRateScale(rateScale);

            tunnelAnimator.CreateLocomotiveEvent
                .Subscribe(_ => CreateLocomotiveEvent())
                .AddTo(this);

            TunnelAnimator CreateController()
            {
                if (jankenTunnelAnimators.TryGetRenderer(out GameObject gameObject, this))
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        Destroy(transform.GetChild(i).gameObject);
                    }
                    TunnelAnimator tunnelAnimator = Instantiate(jankenTunnelAnimators.GetAsset(shape), gameObject.transform);
                    diContainer.InjectGameObject(tunnelAnimator.gameObject);
                    return tunnelAnimator;
                }
                else
                {
                    return null;
                }
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position + new Vector3(0, 1.5f, 0);
        }
    }
}
