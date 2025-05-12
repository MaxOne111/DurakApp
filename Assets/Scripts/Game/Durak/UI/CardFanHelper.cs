using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Game.Durak.UI
{
    [CreateAssetMenu(menuName = "Game/Durak/UI/CardFanConfigurationRepository", fileName = "CardFanConfigurationRepository")]
    public sealed class CardFanConfigurationRepository
        : ScriptableObject
    {
        [SerializeField] private List<DirectionConfigurationPair> configurations;

        public CardFanConfiguration GetConfiguration(EDirection2D direction)
        {
            var result = configurations
                .First(value => value.Direction == direction)
                .Configuration;
            
            return result;
        }

        [Serializable]
        private struct DirectionConfigurationPair
        {
            [field: SerializeField]
            public EDirection2D Direction
            {
                get;
                private set;
            }

            [field: SerializeField]
            public CardFanConfiguration Configuration
            {
                get;
                private set;
            }
        }
    }
}