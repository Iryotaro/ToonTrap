using Cysharp.Threading.Tasks;
using Ryocatusn.Ryoseqs;
using System;
using System.Collections;
using UnityEngine;

namespace Ryocatusn.Conversations
{
    public class SequenceShowMessage : SequenceWaitCommand
    {
        private bool finishShowMessage = false;

        public SequenceShowMessage(Conversation conversation, Message message, float waitTime = 0)
        {
            SequenceShowMessage sequenceWaitForSeconds = this;
            handler = new TaskHandler(Finish =>
            {
                conversation.ShowMessage(message, () => finishShowMessage = true);

                sequenceWaitForSeconds.Start(Finish, waitTime).Forget();
            });
        }

        private async UniTaskVoid Start(Action Finish, float waitTime)
        {
            await Wait(Finish, waitTime);
        }
        private IEnumerator Wait(Action Finish, float waitTime)
        {
            yield return new WaitUntil(() => finishShowMessage);
            yield return (object)new WaitForSeconds(waitTime);
            Finish();
        }
    }
}