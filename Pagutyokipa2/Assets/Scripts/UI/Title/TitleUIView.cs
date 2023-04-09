using DG.Tweening;
using Ryocatusn.Audio;
using Ryocatusn.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ryocatusn.UI
{
    public class TitleUIView : MonoBehaviour
    {
        [SerializeField]
        private Button title;
        [SerializeField]
        private Button startButton;
        [SerializeField]
        private Button settingsButton;
        [SerializeField]
        private Button m_deleteButton;
        [SerializeField]
        private Button m_exitButton;
        [SerializeField]
        private Image player;
        [SerializeField]
        private FakeExitButton fakeExitButton;
        [SerializeField]
        private SE buttonSE;

        public Button deleteButton { get; private set; }
        public Option<Button> exitButton = new Option<Button>(null);

        [System.NonSerialized]
        public float exitButtonSpeed = 0;

        private DeleteButtonRyoseq deleteButtonRyoseq;
        private ExitButtonRyoseq exitButtonRyoseq;

        private SEPlayer sePlayer;

        private void Start()
        {
            deleteButton = m_deleteButton;
            exitButton.Set(m_exitButton);

            deleteButtonRyoseq = new DeleteButtonRyoseq(this);
            exitButtonRyoseq = new ExitButtonRyoseq(this, fakeExitButton);

            sePlayer = new SEPlayer(gameObject);

            startButton.GetComponentInChildren<TextMeshProUGUI>().transform.DOShakePosition(1, 2).SetLoops(-1).SetLink(startButton.gameObject);
            deleteButton.GetComponentInChildren<TextMeshProUGUI>().transform.DOShakePosition(1, 2).SetLoops(-1).SetLink(deleteButton.gameObject);
            exitButton.Get().GetComponentInChildren<TextMeshProUGUI>().transform.DOShakePosition(1, 2).SetLoops(-1).SetLink(exitButton.Get().gameObject);
        }

        public void HandleClickTitle()
        {

        }
        public void HandleClickStartButton()
        {
            Transition.LoadScene("Title", "Game", new TransitionSettings(player.transform, Camera.main, Janken.Hand.Shape.Paper));
            PushButtonSequence(startButton.gameObject);
            sePlayer.Play(buttonSE);
        }
        public void HandleClickSettingsButton()
        {
            Transition.LoadScene("Title", "Game", new TransitionSettings(player.transform, Camera.main, Janken.Hand.Shape.Paper));
            PushButtonSequence(settingsButton.gameObject);
            sePlayer.Play(buttonSE);
        }
        public void HandleClickDeleteButton()
        {
            deleteButtonRyoseq.MoveNext();
            PushButtonSequence(deleteButton.gameObject);
            sePlayer.Play(buttonSE);
        }
        public void HandleLostDeleteButton()
        {
            //titleText.text = "ボタンをいじめちゃだめよ";
        }
        public void HandleEnableDeleteButton(bool enable)
        {
            if (enable) deleteButtonRyoseq.Reset();
            deleteButton.GetComponentInChildren<TextMeshProUGUI>().text = enable ? "削除" : "無効";
        }
        public void HandleClickExitButton()
        {
            exitButtonRyoseq.MoveNext();

            exitButton.Match
                (
                Some: x =>
                {
                    PushButtonSequence(x.gameObject);
                    sePlayer.Play(buttonSE);
                }
                );
        }
        public void HandleEscapeExitButton(Vector2 mousePosition)
        {
            exitButton.Match
                (
                Some: x =>
                {
                    Vector2 exitButtonPositoin = x.transform.position;

                    x.transform.Translate((exitButtonPositoin - mousePosition).normalized * exitButtonSpeed * Time.deltaTime);
                }
                );
        }
        public void HandleLostExitButton()
        {
            exitButton.Match(Some: x => Destroy(x.gameObject));
            exitButton.Set(null);
            //titleText.text = "おわるボタンを見失った！";
        }

        private Tween PushButtonSequence(GameObject button)
        {
            button.transform.localScale = Vector3.one;
            return button.transform.DOScale(new Vector2(1.3f, 1.3f), 0.07f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo).SetLink(button);
        }
    }
}
