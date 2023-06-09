using Ryocatusn.Conversations;
using Ryocatusn.Games;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class ConversationTrigger : MonoBehaviour
    {
        [Inject]
        private GameManager gameManager;

        [SerializeField]
        private MessageAndWaitTime[] messageAndWaitTime;

        private void Start()
        {
            Conversation conversation = gameManager.gameContains.conversation;

            GetComponent<TileTransformTrigger>().OnHitPlayerEvent
                .FirstOrDefault()
                .Where(_ => conversation != null)
                .Subscribe(_ => { /*メッセージ表示*/ })
                .AddTo(this);
        }

        [Serializable]
        class MessageAndWaitTime
        {
            public Message message;
            public float waitTime;
        }
    }
}
