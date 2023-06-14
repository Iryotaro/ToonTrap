using FTRuntime;
using Ryocatusn.Games;
using Ryocatusn.Janken;
using System;
using UnityEngine;
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
        private JankenSwfClipControllers jankenSwfClips;
        [SerializeField]
        private CreateLocomotiveFrames createLocomotiveFrames;
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
            SwfClipController controller = CreateController();
            if (controller == null) return;

            controller.rateScale = rateScale;

            CallAction();

            SwfClipController CreateController()
            {
                if (jankenSwfClips.TryGetRenderer(out GameObject gameObject, this))
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        DestroyImmediate(transform.GetChild(i).gameObject);
                    }
                    return Instantiate(jankenSwfClips.GetAsset(shape), gameObject.transform);
                }
                else
                {
                    return null;
                }
            }
            void CallAction()
            {
                controller.clip.OnChangeCurrentFrameEvent += Call;

                void Call(SwfClip clip)
                {
                    if (clip.currentFrame >= createLocomotiveFrames.GetFrame(shape))
                    {
                        CreateLocomotiveEvent();
                        foreach (Delegate del in CreateLocomotiveEvent.GetInvocationList()) CreateLocomotiveEvent -= (Action)del;

                        controller.clip.OnChangeCurrentFrameEvent -= Call;
                    }
                }
            }
        }

        [Serializable]
        public class CreateLocomotiveFrames
        {
            [Min(0)]
            public float rockFrame;
            [Min(0)]
            public float scissorsFrame;
            [Min(0)]
            public float paperFrame;

            public float GetFrame(Hand.Shape shape)
            {
                return shape switch
                {
                    Hand.Shape.Rock => rockFrame,
                    Hand.Shape.Scissors => scissorsFrame,
                    Hand.Shape.Paper => paperFrame,
                    _ => 0
                };
            }
        }
    }
}
