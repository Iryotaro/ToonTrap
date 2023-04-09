using DG.Tweening;
using Ryocatusn.Audio;
using Ryocatusn.Util;
using System;
using TMPro;
using UnityEngine;

namespace Ryocatusn.Conversations
{
    public class Conversation : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private SE se;
        [SerializeField]
        private bool boss;

        private Option<Message> showingMessage = new Option<Message>(null);
        private Option<Tween> textTween = new Option<Tween>(null);

        private SEPlayer sePlayer;

        private bool showing = false;
        private float lastTimeToShow;

        private void Start()
        {
            HideMessage();
            sePlayer = new SEPlayer(gameObject);

            if (boss) transform.DOShakePosition(1, 20).SetLoops(-1).SetLink(gameObject);
        }
        private void Update()
        {
            if (showingMessage.Get() == null || showing) return;
            if (Time.fixedTime - lastTimeToShow > showingMessage.Get().time)
            {
                HideMessage();
            }
        }

        public void ShowMessage(Message message, Action action = null)
        {
            if (!IsAllowedToShow(message)) return;

            textTween.Match(Some: x => x.Kill(true));
            showingMessage.Set(message);

            if (message.value == "")
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
                Complete();
            }
            else
            {
                showing = true;
                sePlayer.Play(se);

                textTween.Set(DOTween.To
                    (
                    () => "",
                    x => text.text = x,
                    message.value,
                    message.value.Length / message.rate
                    )
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        Complete();
                    })
                    .SetLink(text.gameObject));
            }

            void Complete()
            {
                action?.Invoke();
                lastTimeToShow = Time.fixedTime;
                showing = false;
                sePlayer.Stop(se);
            }
        }
        private void HideMessage()
        {
            showingMessage.Set(null);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
            text.text = "";
        }

        private bool IsAllowedToShow(Message message)
        {
            if (showingMessage.Get() == null) return true;
            if (showingMessage.Get().value.Equals(message.value)) return false;
            return message.priority >= showingMessage.Get().priority;
        }
    }
}
