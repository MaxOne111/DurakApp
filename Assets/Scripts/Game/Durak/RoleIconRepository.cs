using System;
using System.Collections.Generic;
using System.Linq;
using Game.Durak.Enums;
using UnityEngine;

namespace Game.Durak
{
    [CreateAssetMenu(menuName = "Game/Durak/RoleIconRepository", fileName = "RoleIconRepository")]
    public sealed class RoleIconRepository
        : ScriptableObject
    {
        [SerializeField] private List<RoleIconPair> pairs;

        public Sprite GetIcon(EPlayerRole role)
        {
            var result = pairs.First(value => value.Role == role).Icon;
            return result;
        }
        
        [Serializable]
        private struct RoleIconPair
        {
            [field: SerializeField]
            public EPlayerRole Role
            {
                get;
                private set;
            }

            [field: SerializeField]
            public Sprite Icon
            {
                get;
                private set;
            }
        }
    }
}