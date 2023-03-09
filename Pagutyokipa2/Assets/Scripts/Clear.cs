using UnityEngine;
using Ryocatusn.Conversations;
using Ryocatusn.Ryoseqs;

namespace Ryocatusn
{
    public class Clear : MonoBehaviour
    {
        [SerializeField]
        private Conversation conversation;

        private Ryoseq ryoseq;
        private Message baseMessage;

        public void Start()
        {
            ryoseq = new Ryoseq();
            ryoseq.AddTo(this);

            baseMessage = Message.Base(0, 10, 2.5f);

            CreateSequence(ryoseq);
            ryoseq.MoveNext();
        }
        private ISequence CreateSequence(Ryoseq ryoseq)
        {
            ISequence sequence = ryoseq.Create();

            return sequence
                .ConnectWait(new SequenceWaitForSeconds(1))
                .ConnectWait(new SequenceShowMessage(conversation, baseMessage.Copy("ここから先は製作中です"), 2))
                .ConnectWait(new SequenceShowMessage(conversation, baseMessage.Copy("最後まで遊んでくださりありがとうございます！"), 2))
                .Connect(new SequenceCommand(_ => Transition.LoadScene("Clear", "Title", TransitionSettings.Default())));
        }
    }
}
