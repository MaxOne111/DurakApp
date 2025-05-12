using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Durak
{
    [CreateAssetMenu(menuName = "Game/Durak/TextMessageContainer", fileName = "TextMessageContainer")]
    public sealed class TextMessageRepository
        : ScriptableObject
    {
        [SerializeField] private List<string> messages;

        public IEnumerable<string> Messages => messages;
    }
}