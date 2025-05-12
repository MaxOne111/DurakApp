using System;
using System.Collections.Generic;
using System.Linq;
using Game.Durak.Enums;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Durak.UI
{
    public class RecentMessageSelectPanel
        : SelectPanelBase<ChatMessage>
    {
        private const string RecentKey = "Game/Emoji/Recent";
        
        [SerializeField] private TextMessageRepository textMessageRepository;
        [SerializeField] private EmojiRepository emojiRepository;

        [SerializeField] private Transform parent;
        
        [SerializeField] private EmojiView emojiPrefab;
        [SerializeField] private TextMessageView textMessagePrefab;
        
        protected override IDictionary<Button, ChatMessage> Fill(out Action clear, out Action<ChatMessage> select)
        {
            var values = RecentMessagesHelper.GetRecent().Reverse();

            var list = new List<GameObject>();
            var dictionary = new Dictionary<Button, ChatMessage>();
            foreach (var value in values)
            {
                switch (value.Type)
                {
                    case EChatMessageType.Message:
                        var textView = Instantiate(textMessagePrefab, parent);
                        textView.Label.text = value.Text;
                
                        list.Add(textView.gameObject);
                
                        dictionary.Add(textView.Button, new ChatMessage(value.Type, value.Text));
                        break;
                    
                    case EChatMessageType.Emoji:
                        var emojiView = Instantiate(emojiPrefab, parent);
                        var frames = emojiRepository.GetEmoji(value.Text);
                        emojiView.SetAnimation(frames);
                        
                        list.Add(emojiView.gameObject);
                        
                        dictionary.Add(emojiView.Button, new ChatMessage(value.Type, value.Text));
                        break;
                }
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