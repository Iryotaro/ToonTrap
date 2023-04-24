using UnityEngine;
using Ryocatusn.Janken;
using FTRuntime;
using System;
using UniRx;
using UniRx.Triggers;

namespace Ryocatusn.Characters
{
    public class Tunnel : MonoBehaviour
    {
        [SerializeField]
        private LocomotiveData[] datas;
        [SerializeField]
        private JankenSwfClipControllers jankenSwfClips;
        [SerializeField]
        private CreateLocomotiveFrames createLocomotiveFrames;
        [SerializeField]
        private Railway railway;
        [SerializeField]
        private Locomotive locomotive;

        private void Start()
        {
            InvokeRepeating("A", 5, 8);
        }
        private void A()
        {
            Hand.Shape shape = (Hand.Shape)UnityEngine.Random.Range(0, 3);
            Action action;
            action = () => CreateLocomotive(shape);
            PlayAnimation(shape, action);
        }

        private Locomotive CreateLocomotive(Hand.Shape shape)
        {
            Locomotive newLocomotive = Instantiate(locomotive);
            newLocomotive.SetUp(shape, railway, datas[UnityEngine.Random.Range(0, datas.Length)]);
            return newLocomotive;
        }
        private void PlayAnimation(Hand.Shape shape, Action CreateLocomotiveEvent)
        {
            SwfClipController controller = CreateController();
            if (controller == null) return;

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
                IDisposable disposable = null;
                disposable = this.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        if (controller.clip.currentFrame == createLocomotiveFrames.GetFrame(shape))
                        {
                            CreateLocomotiveEvent();
                            foreach (Delegate del in CreateLocomotiveEvent.GetInvocationList()) CreateLocomotiveEvent -= (Action)del;
                            disposable.Dispose();
                        }
                    });
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
