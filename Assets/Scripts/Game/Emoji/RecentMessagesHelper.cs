using System.Collections.Generic;
using System.Linq;
using Game.Durak.Enums;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Durak.UI
{
    public static class RecentMessagesHelper
    {
        private const string Key = "Emoji/Recent";

        private static readonly IList<ChatMessage> Messages = new List<ChatMessage>();

        public static void Initialize()
        {
            Messages.Clear();

            var data = PlayerPrefs.GetString(Key);
            var messages = JsonConvert.DeserializeObject<ChatMessage[]>(data);
            if (messages != null && messages.Any())
            {
                messages = messages.Distinct().ToArray();
                Messages.AddRange(messages);
            }
        }

        public static void Dispose()
        {
            var data = JsonConvert.SerializeObject(Messages.Distinct());
            PlayerPrefs.SetString(Key, data);
        }
        
        public static void AddRecent(EChatMessageType type, string name)
        {
            var item = new ChatMessage(type, name);
            Messages.Add(item);
        }

        public static IEnumerable<ChatMessage> GetRecent()
        {
            return Messages;
        }
    }
}