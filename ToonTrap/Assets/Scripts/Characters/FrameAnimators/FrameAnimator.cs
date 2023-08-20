using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;
using System;
using UnityEngine.UI;

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

        private Subject<Unit> onCompleted = new Subject<Unit>();

        public IObservable<Unit> OnCompleted => onCompleted;

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
            onCompleted.Dispose();
        }

        public void ShowFirstFrame()
        {
            HideAllFrames();
            frames[0].gameObject.SetActive(true);
        }
        public void HideAllFrames()
        {
            foreach (Frame frame in frames)
            {
                frame.gameObject.SetActive(false);
            }
        }
        public void Play()
        {
            StartCoroutine(PlayEnumerator());

            IEnumerator PlayEnumerator()
            {
                while (true)
                {
                    HideAllFrames();
                    foreach (Frame frame in frames)
                    {
                        frame.gameObject.SetActive(true);
                        yield return new WaitForSeconds(frame.interval / speed);
                        frame.gameObject.SetActive(false);
                    }

                    frames[frames.Length - 1].gameObject.SetActive(true);

                    if (loopMode == LoopMode.Once) break;
                }
                onCompleted.OnNext(Unit.Default);
            }
        }
    }
}
