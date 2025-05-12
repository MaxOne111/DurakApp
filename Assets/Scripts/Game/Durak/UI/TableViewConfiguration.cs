using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Durak.UI
{
    [CreateAssetMenu(menuName = "Game/Durak/UI/TableViewConfiguration", fileName = "TableViewConfiguration")]
    public sealed class TableViewConfiguration
        : ScriptableObject
    {
        [SerializeField] private List<ChairPositionConfiguration> configurations;

        public IEnumerable<Vector2> GetChairPositions(int playersCount)
        {
            var result = configurations
                .First(value => value.PlayersCount == playersCount)
                .ChairPositions;
            
            return result;
        }

        [Serializable]
        private struct ChairPositionConfiguration
        {
            [field: SerializeField]
            public int PlayersCount
            {
                get;
                private set;
            }

            [field: SerializeField]
            public List<Vector2> ChairPositions
            {
                get;
                private set;
            }
        }
    }
}