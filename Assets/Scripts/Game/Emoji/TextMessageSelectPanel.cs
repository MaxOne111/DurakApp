using System;
using System.Collections.Generic;
using Game.Durak.Enums;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Durak.UI
{
    public sealed class TextMessageSelectPanel
        : SelectPanelBase<ChatMessage>
    {
        [SerializeField] private TextMessageRepository repository;

        [SerializeField] private Transform parent;
        [SerializeField] private TextMessageView prefab;
        
        protected override IDictionary<Button, ChatMessage> Fill(out Action clear, out Action<ChatMessage> select)
        {
            var values = repository.Messages;

            var list = new List<GameObject>();
            var dictionary = new Dictionary<Button, ChatMessage>();
            foreach (var value in values)
            {
                var view = Instantiate(prefab, parent);
                view.Label.text = value;
                
                list.Add(view.gameObject);
                
                dictionary.Add(view.Button, new ChatMessage(EChatMessageType.Message, value));
            }

            clear = Clear;
            select = OnSelect;
            
            return dictionary;

            void Clear()
            {
                foreach (var item in list)
                {
                    Destroy(item);
                }
            }

            void OnSelect(ChatMessage target)
            {
                RecentMessagesHelper.AddRecent(target.Type, target.Text);
            }
        }
    }
}