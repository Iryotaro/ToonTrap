using Coffee.UIExtensions;
using DG.Tweening;
using Ryocatusn.Audio;
using Ryocatusn.Janken;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using UniRx;

namespace Ryocatusn
{
    public class Transition : MonoBehaviour
    {
        [SerializeField]
        private Image transitionImage;
        [SerializeField]
        private Unmask transitionMask;
        [SerializeField]
        private AnimationCurve shrinkEase;
        [SerializeField]
        private SE transitionSE;
        [SerializeField]
        private JankenSprites handSprites;

        private TransitionSettings transitionSettings;

        private void Start()
        {
            transitionSettings = TransitionSettings.Default();
        }
        public void LoadScene(string unloadSceneName, string loadSceneName, TransitionSettings transitionSettings, Action finish = null)
        {
            LoadScene(new string[] { unloadSceneName }, new string[] { loadSceneName }, transitionSettings, finish);
        }
        public void LoadScene(string[] unloadSceneNames, string[] loadSceneNames, TransitionSettings transitionSettings, Action finish = null)
        {
            this.transitionSettings = transitionSettings;

            SEPlayer sePlayer = new SEPlayer(gameObject);
            sePlayer.DontDestroyOnLoad();
            sePlayer.Play(transitionSE);

            ChangeHandSprite();

            Sequence sequence = DOTween.Sequence();

            sequence
                .SetLink(transitionMask.gameObject)
                .Append(ShrinkTransitionMask(transitionMask, 3))
                .AppendCallback(() =>
                {
                    foreach (string loadSceneName in loadSceneNames)
                    {
                        SceneManager.LoadScene(loadSceneName, LoadSceneMode.Additive);
                    }
                    
                    Observable.NextFrame()
                    .Subscribe(_ =>
                    {
                        foreach (string unloadSceneName in unloadSceneNames)
                        {
                            SceneManager.UnloadSceneAsync(unloadSceneName);
                        }
                    })
                    .AddTo(this);
                })
                .AppendInterval(0.5f)
                .Append(EnlargeTransitionMask(transitionMask, 3))
                .OnComplete(() => finish?.Invoke());
        }

        private Tween EnlargeTransitionMask(Unmask transitionMask, float duration)
        {
            RectTransform rectTransform = transitionMask.GetComponent<RectTransform>();

            return DOTween.To
                (
                () => rectTransform.localScale,
                x => rectTransform.localScale = x,
                Vector3.one * 100,
                duration
                );
        }
        private Tween ShrinkTransitionMask(Unmask transitionMask, float duration)
        {
            RectTransform rectTransform = transitionMask.GetComponent<RectTransform>();

            return DOTween.To
                (
                () => rectTransform.localScale,
                x => rectTransform.localScale = x,
                Vector3.zero,
                duration
                )
                .SetEase(shrinkEase);
        }

        private void ChangeHandSprite()
        {
            Sprite handSprite = transitionSettings.shape switch
            {
                Hand.Shape.Rock => handSprites.rockSprite,
                Hand.Shape.Scissors => handSprites.scissorsSprite,
                Hand.Shape.Paper => handSprites.paperSprite,
                _ => null
            };

            transitionImage.sprite = handSprite;
            transitionImage.SetNativeSize();
        }
    }
}
