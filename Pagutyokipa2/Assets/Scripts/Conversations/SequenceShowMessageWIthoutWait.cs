using Ryocatusn.Conversations;
using Ryocatusn.Ryoseqs;

namespace Ryocatusn
{
    public class SequenceShowMessageWithoutWait : SequenceCommand
    {
        public SequenceShowMessageWithoutWait(Conversation conversation, Message message)
        {
            handler = new TaskHandler(_ =>
            {
                conversation.ShowMessage(message);
            });
        }
    }
}
