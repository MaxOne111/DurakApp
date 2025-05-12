using System;
using System.Collections.Generic;
using System.Linq;
using Game.Durak.Enums;
using UnityEngine;

namespace Game.Durak.Core
{
    [CreateAssetMenu(menuName = "Game/Durak/Core/ResponseTextMessageRepository", fileName = "ResponseTextMessageRepository")]
    public sealed class ResponseTextMessageRepository
        : ScriptableObject
    {
        [SerializeField] private List<ResponseTextMessagePair> pairs;

        public bool TryGetMessage(ETurnMode mode, out TextMessage result)
        {
            result = default;
            
            var hasMessage = pairs.Any(value => value.ResponseMode == mode);
            if (hasMessage)
            {
                result = pairs.First(value => value.ResponseMode == mode).TextMessage;
            }

            return hasMessage;
        }
        
        [Serializable]
        private struct ResponseTextMessagePair
        {
            [field: SerializeField]
            public ETurnMode ResponseMode
            {
                get;
                private set;
            }

            [field: SerializeField]
            public TextMessage TextMessage
            {
                get;
                private set;
            }
        }
    }
}