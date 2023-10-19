using Cysharp.Threading.Tasks;
using Ryocatusn.Games;
using Ryocatusn.Janken;
using System;
using UnityEngine;
using UniRx;
using Zenject;
using Ryocatusn.Photographers;

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
        [SerializeField]
        private bool autoPlay = true;

        [Inject]
        private DiContainer diContainer;
        [Inject]
        private GameManager gameManager;
        [Inject]
        private PhotographerSubjectManager photographerSubjectManager;

        private void Start()
        {
            if (autoPlay) InvokeRepeating(nameof(Play), 0, 8 / rateScale);

            photographerSubjectManager.Save(this);
        }
        private void OnDestroy()
        {
            photographerSubjectManager.Delete(this);
        }

        [SerializeField]
        private Hand.Shape _shape = Hand.Shape.Rock;

        public int priority { get; } = 0;

        public int photographerCameraSize { get; } = 3;

        public void Play()
        {
            //if (gameManager.gameContains.gameCamera.IsOutSideOfCamera(gameObject)) return;

            Hand.Shape shape = _shape;//Hand.GetRandomShape();
            Action action;
            action = () => CreateLocomotive(shape);
            PlayAnimation(shape, action);
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
