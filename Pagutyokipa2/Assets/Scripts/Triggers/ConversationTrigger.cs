using Ryocatusn.Conversations;
using System;
using UniRx;
using UnityEngine;

namespace Ryocatusn
{
    [RequireComponent(typeof(TileTransformTrigger))]
    public class ConversationTrigger : MonoBehaviour
    {
        [SerializeField]
        private MessageAndWaitTime[] messageAndWaitTime;

        private void Start()
        {
            Conversation conversation = null;
            StageManager.activeStage.SetupStageEvent
                .Subscribe(gameContains => conversation = gameContains.conversation)
                .AddTo(this);

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
