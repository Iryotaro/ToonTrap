using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Coffee.UIExtensions;
using Ryocatusn.Janken;
using Ryocatusn.Audio;

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

        private static string[] unloadSceneNames;
        private static string[] loadSceneNames;
        private static TransitionSettings transitionSettings;
        private static Action onLoadScene;
        private static bool active;

        public static bool LoadScene(string unloadSceneName, string loadSceneName, TransitionSettings transitionFocus, Action onLoadScene = null)
        {
            if (active) return false;
            active = true;

            unloadSceneNames = new string[] { unloadSceneName };
            loadSceneNames = new string[] { loadSceneName };
            Transition.transitionSettings = transitionFocus;
            Transition.onLoadScene = onLoadScene;

            SceneManager.LoadScene("Transition", LoadSceneMode.Additive);

            return true;
        }
        public static bool LoadScene(string[] unloadSceneNames, string[] loadSceneNames, TransitionSettings transitionFocus, Action onLoadScene = null)
        {
            if (active) return false;
            active = true;

            Transition.unloadSceneNames = unloadSceneNames;
            Transition.loadSceneNames = loadSceneNames;
            Transition.transitionSettings = transitionFocus;
            Transition.onLoadScene = onLoadScene;

            SceneManager.LoadScene("Transition", LoadSceneMode.Additive);

            return true;
        }

        private void Start()
        {
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
                    foreach (string unloadSceneName in unloadSceneNames)
                    {
                        SceneManager.UnloadSceneAsync(unloadSceneName);
                    }
                    onLoadScene?.Invoke();
                })
                .AppendInterval(0.5f)
                .Append(EnlargeTransitionMask(transitionMask, 3))
                .OnComplete(() =>
                {
                    active = false;
                    SceneManager.UnloadSceneAsync("Transition");
                });

            Tween EnlargeTransitionMask(Unmask transitionMask, float duration)
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
            Tween ShrinkTransitionMask(Unmask transitionMask, float duration)
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
        }
        private void Update()
        {
            ChangeMaskPosition();
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
        private void ChangeMaskPosition()
        {
            RectTransform maskRectTransform = transitionMask.GetComponent<RectTransform>();
            transitionSettings.SetPosition(maskRectTransform);
        }
    }
}
