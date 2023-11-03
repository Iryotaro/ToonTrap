using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;
using System;

namespace Ryocatusn.Characters
{
    public class FrameAnimator : MonoBehaviour
    {
        [SerializeField]
        private Frame[] frames;
        [SerializeField]
        private PlayMode playMode = PlayMode.Forward;
        [SerializeField]
        private LoopMode loopMode = LoopMode.Once;
        [SerializeField]
        private float speed = 1;
        [SerializeField]
        private bool autoPlay = false;

        private Subject<Unit> completeSubject = new Subject<Unit>();

        public IObservable<Unit> CompleteSubjet => completeSubject;

        public enum PlayMode
        {
            Forward,
            Backward
        }
        public enum LoopMode
        {
            Once,
            Loop
        }

        private void Awake()
        {
            if (playMode == PlayMode.Backward) frames = frames.Reverse().ToArray();
        }

        private void Start()
        {
            if (autoPlay) Play();
        }
        private void OnDestroy()
        {
            completeSubject.Dispose();
        }

        public void ShowSpecificFrame(int index)
        {
            frames[index].Show();
        }
        public void HideAllFrames()
        {
            foreach (Frame frame in frames)
            {
                frame.Hide();
            }
        }
        public void Play()
        {
            StartCoroutine(PlayEnumerator());

            IEnumerator PlayEnumerator()
            {
                while (true)
                {
                    for (int i = 0; i < frames.Length; i++)
                    {
                        Frame frame = frames[i];

                        frame.Show();
                        yield return new WaitForSeconds(frame.interval / speed);
                        if (i != frames.Length - 1) frame.Hide();
                    }

                    if (loopMode == LoopMode.Once) break;
                }
                completeSubject.OnNext(Unit.Default);
            }
        }
    }
}
