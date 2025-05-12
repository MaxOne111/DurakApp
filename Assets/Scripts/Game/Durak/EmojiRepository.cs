using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Durak
{
    [CreateAssetMenu(menuName = "Game/Durak/EmojiContainer", fileName = "EmojiContainer")]
    public sealed class EmojiRepository
        : ScriptableObject
    {
        [SerializeField] private List<Emoji> emojis;

        public IEnumerable<string> Ids => emojis.Select(value => value.Id);

        public IEnumerable<Sprite> GetEmoji(string id)
        {
            var result = emojis.FirstOrDefault(value => value.Id == id);
            return result.Sprites;
        }
        
        [Serializable]
        private struct Emoji
        {
            [field: SerializeField]
            public string Id
            {
                get;
                private set;
            }

            [field: SerializeField]
            public List<Sprite> Sprites
            {
                get;
                private set;
            }
        }
    }
}