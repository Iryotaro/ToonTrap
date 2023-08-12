using UnityEngine;
using System.Collections;
using System.Linq;

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

        public void Play()
        {
            StartCoroutine(PlayEnumerator());

            IEnumerator PlayEnumerator()
            {
                while (true)
                {
                    foreach (Frame frame in frames)
                    {
                        frame.gameObject.SetActive(false);
                    }
                    foreach (Frame frame in frames)
                    {
                        frame.gameObject.SetActive(true);
                        yield return new WaitForSeconds(frame.interval / speed);
                        frame.gameObject.SetActive(false);
                    }

                    frames[frames.Length - 1].gameObject.SetActive(true);

                    if (loopMode == LoopMode.Once) break;
                }
            }
        }
    }
}
