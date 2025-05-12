using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Durak.Repositories
{
    public abstract class RepositoryBase<TKey, TValue>
        : ScriptableObject
    {
        [SerializeField] private List<Pair> pairs;

        public bool TryGetValue(TKey key, out TValue result)
        {
            result = default;
            
            var hasValue = pairs.Any(value => value.Key.Equals(key));
            if (hasValue)
            {
                result = pairs.First(value => value.Key.Equals(key)).Value;
            }

            return hasValue;
        }
        
        [Serializable]
        private struct Pair
        {
            [field: SerializeField]
            public TKey Key
            {
                get;
                private set;
            }

            [field: SerializeField]
            public TValue Value
            {
                get;
                private set;
            }
        }
    }
}