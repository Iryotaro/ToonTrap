using Coffee.UIExtensions;
using DG.Tweening;
using Ryocatusn.Audio;
using Ryocatusn.Janken;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ryocatusn
{
    [RequireComponent(typeof(Canvas))]
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

        private bool isLoading = false;

        public static void FullLoadScene(string unloadSceneName, string loadSceneName, TransitionSettings transitionSettings, Action<bool> finish = null)
        {
            Transition.FullLoadScenes(new string[] { unloadSceneName }, new string[] { loadSceneName }, transitionSettings, finish);
        }
        public static void FullLoadScenes(string[] unloadSceneNames, string[] loadSceneNames, TransitionSettings transitionSettings, Action<bool> finish = null)
        {
            SceneManager.LoadScene("FullTransition", LoadSceneMode.Additive);
            Observable.NextFrame()
                .FirstOrDefault()
                .Subscribe(_ =>
                {
                    GameObject[] rootGameObjects = SceneManager.GetSceneByName("FullTransition").GetRootGameObjects();
                    Transition transition = rootGameObjects.Select(x => x.GetComponent<Transition>()).Where(x => x != null).First();

                    transition.LoadScenes(unloadSceneNames, loadSceneNames, transitionSettings, x =>
                    {
                        SceneManager.UnloadSceneAsync("FullTransition");
                        finish?.Invoke(x);
                    });
                });
        }

        public void LoadScene(string unloadSceneName, string loadSceneName, TransitionSettings transitionSettings, Action<bool> finish = null)
        {
            LoadScenes(new string[] { unloadSceneName }, new string[] { loadSceneName }, transitionSettings, finish);
        }
        public void LoadScenes(string[] unloadSceneNames, string[] loadSceneNames, TransitionSettings transitionSettings, Action<bool> finish = null)
        {
            if (isLoading)
            {
                finish?.Invoke(false);
                return;
            };
            isLoading = true;

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
                    LoadScenes(loadSceneNames);

                    Observable.NextFrame()
                    .Subscribe(_ =>
                    {
                        UnloadScenes(unloadSceneNames);
                    })
                    .AddTo(this);
                })
                .AppendInterval(0.5f)
                .Append(EnlargeTransitionMask(transitionMask, 3))
                .OnComplete(() =>
                {
                    isLoading = false;
                    finish?.Invoke(true);
                });
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

        private void LoadScenes(string[] loadSceneNames)
        {
            foreach (string loadSceneName in loadSceneNames)
            {
                SceneManager.LoadScene(loadSceneName, LoadSceneMode.Additive);
            }
        }

        private void UnloadScenes(string[] unloadSceneNames)
        {
            foreach (string unloadSceneName in unloadSceneNames)
            {
                SceneManager.UnloadSceneAsync(unloadSceneName);
            }
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
