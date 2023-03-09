using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ryocatusn.Ryoseqs;
using Ryocatusn.Conversations;

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
