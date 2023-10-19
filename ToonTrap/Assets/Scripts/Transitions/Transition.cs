using Coffee.UIExtensions;
using DG.Tweening;
using Ryocatusn.Audio;
using Ryocatusn.Janken;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Ryocatusn.Games;
using Zenject;
using System;

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

        [Inject]
        private GameManager gameManager;

        private TransitionSettings transitionSettings;

        //public static bool LoadScene(string unloadSceneName, string loadSceneName, TransitionSettings transitionFocus, Action onLoadScene = null)
        //{
        //    if (active) return false;
        //    active = true;

        //    unloadSceneNames = new string[] { unloadSceneName };
        //    loadSceneNames = new string[] { loadSceneName };
        //    Transition.transitionSettings = transitionFocus;
        //    Transition.onLoadScene = onLoadScene;

        //    SceneManager.LoadScene("Transition", LoadSceneMode.Additive);

        //    return true;
        //}
        //public static bool LoadScene(string[] unloadSceneNames, string[] loadSceneNames, TransitionSettings transitionFocus, Action onLoadScene = null)
        //{
        //    if (active) return false;
        //    active = true;

        //    Transition.unloadSceneNames = unloadSceneNames;
        //    Transition.loadSceneNames = loadSceneNames;
        //    Transition.transitionSettings = transitionFocus;
        //    Transition.onLoadScene = onLoadScene;

        //    SceneManager.LoadScene("Transition", LoadSceneMode.Additive);

        //    return true;
        //}

        private void Start()
        {
            transitionSettings = TransitionSettings.Default();
        }
        public void LoadScene(string unloadSceneName, string loadSceneName, TransitionSettings transitionSettings, Action finish)
        {
            LoadScene(new string[] { unloadSceneName }, new string[] { loadSceneName }, transitionSettings, finish);
        }
        public void LoadScene(string[] unloadSceneNames, string[] loadSceneNames, TransitionSettings transitionSettings, Action finish)
        {
            this.transitionSettings = transitionSettings;

            SEPlayer sePlayer = new SEPlayer(gameObject, gameManager != null ? gameManager.gameContains.gameCamera : null);
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

        private void Update()
        {
            //ChangeMaskPosition();
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
            transitionSettings.SetPosition(maskRectTransform, gameManager.gameContains.gameCamera.camera);
        }
    }
}
