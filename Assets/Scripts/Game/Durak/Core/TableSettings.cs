using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Durak.Core
{
    [Serializable]
    public sealed class TableSettings
    {
        [SerializeField] private List<TablePreset> presets;
        
        public IReadOnlyDictionary<int, IReadOnlyList<Vector2>> ChairPositions 
            => presets.ToDictionary(value => value.ChairsCount, value => value.ChairPositions);
    }

    [Serializable]
    public sealed class TablePreset
    {
        [field: SerializeField]
        public int ChairsCount
        {
            get;
            private set;
        }
        
        [SerializeField] 
        private List<Vector2> chairPositions;

        public IReadOnlyList<Vector2> ChairPositions => chairPositions;
    }
}