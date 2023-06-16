using Cysharp.Threading.Tasks;
using FTRuntime;
using Ryocatusn.Games;
using Ryocatusn.Janken;
using System;
using UnityEngine;
using UniRx;
using Zenject;

namespace Ryocatusn.Characters
{
    public class Tunnel : MonoBehaviour
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
        private GameManager gameManager;
        [Inject]
        private DiContainer diContainer;

        private void Start()
        {
            if (autoPlay) InvokeRepeating(nameof(Play), 0, 8 / rateScale);
        }
        public void Play()
        {
            if (gameManager.gameContains.gameCamera.IsOutSideOfCamera(gameObject)) return;

            Hand.Shape shape = (Hand.Shape)UnityEngine.Random.Range(0, 3);
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
                    return Instantiate(jankenTunnelAnimators.GetAsset(shape), gameObject.transform);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
