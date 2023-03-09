using System;
using UnityEngine;

namespace Ryocatusn.Conversations
{
    [Serializable]
    public class Message
    {
        public string value;
        public int priority;
        public float rate;
        public float time;

        public Message(string value, int priority, float rate, float time)
        {
            this.value = value;
            this.priority = priority;
            this.rate = rate;
            this.time = time;
        }

        public static Message Base(int priority, float rate, float time)
        {
            return new Message("", priority, rate, time);
        }

        public Message Copy(string value)
        {
            return new Message(value, priority, rate, time);
        }
    }
}
