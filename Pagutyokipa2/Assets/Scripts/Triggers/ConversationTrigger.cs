using Ryocatusn.Conversations;
using Ryocatusn.Ryoseqs;
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
            Ryoseq ryoseq = new Ryoseq();
            ryoseq.AddTo(this);
            ISequence sequence = ryoseq.Create();

            StageManager.activeStage.SetupStageEvent
                .Subscribe(gameContains =>
                {
                    foreach (MessageAndWaitTime messageAndWaitTime in messageAndWaitTime)
                    {
                        sequence
                        .ConnectWait(new SequenceShowMessage(gameContains.conversation, messageAndWaitTime.message, messageAndWaitTime.waitTime));
                    }
                })
                .AddTo(this);

            GetComponent<TileTransformTrigger>().OnHitPlayerEvent
                .FirstOrDefault()
                .Subscribe(_ => ryoseq.MoveNext())
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
