using Ryocatusn.Conversations;
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
        private StageManager stageManager;

        [SerializeField]
        private MessageAndWaitTime[] messageAndWaitTime;

        private void Start()
        {
            Conversation conversation = null;
            stageManager.SetupStageEvent
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
