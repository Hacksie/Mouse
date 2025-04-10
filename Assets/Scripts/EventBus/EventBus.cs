using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    public class EventBus : MonoBehaviour
    {
        public static EventBus Instance { get; private set; }

        private readonly Dictionary<MessageType, UnityEvent<Message>> eventBus = new();

        private EventBus() => Instance = this;

        public void Subscribe(MessageType type, UnityAction<Message> callback)
        {
            if (!eventBus.ContainsKey(type))
            {
                CreateBus(type);
            }

            eventBus[type].AddListener(callback);
        }

        public void Unsubscribe(MessageType type, UnityAction<Message> callback)
        {
            if (eventBus.ContainsKey(type))
            {
                eventBus[type].RemoveListener(callback);
            }
        }

        public void Send(Message message)
        {
            if (eventBus.ContainsKey(message.type))
            {
                eventBus[message.type].Invoke(message);
            }
        }

        public void CreateBus(MessageType type)
        {
            eventBus.Add(type, new UnityEvent<Message>());
        }
    }

    public class Message
    {
        public int frame;
        public MessageType type;
        public GameObject source;
        public GameObject target;
        public object data;

        public static Message Create(MessageType type, GameObject source, GameObject target, object data) =>
             new()
             {
                 type = type,
                 source = source,
                 target = target,
                 data = data
             };
    }

    public enum MessageType
    {
        Generic,
    }
}
