using System;
using System.Collections.Generic;
using Game.Durak.Enums;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Durak.UI
{
    public sealed class EmojiSelectPanel
        : SelectPanelBase<ChatMessage>
    {
        [SerializeField] private EmojiRepository repository;

        [SerializeField] private Transform parent;
        [SerializeField] private EmojiView prefab;
        
        protected override IDictionary<Button, ChatMessage> Fill(out Action clear, out Action<ChatMessage> select)
        {
            var values = repository.Ids;
            
            var dictionary = new Dictionary<Button, ChatMessage>();
            foreach (var value in values)
            {
                var frames = repository.GetEmoji(value);
                var emoji = Instantiate(prefab, parent);
                emoji.SetAnimation(frames);
                
                dictionary.Add(emoji.Button, new ChatMessage(EChatMessageType.Emoji, value));
            }

            clear = Clear;
            select = OnSelect;

            return dictionary;

            void Clear()
            {
                foreach (var item in dictionary)
                {
                    try
                    {
                        Destroy(item.Key.gameObject);
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }

            void OnSelect(ChatMessage target)
            {
                RecentMessagesHelper.AddRecent(target.Type, target.Text);
            }
        }
    }
}