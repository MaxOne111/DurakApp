using System;
using Game.Durak.Enums;
using UnityEngine;

namespace Game.Durak.Repositories
{
    [CreateAssetMenu(menuName = "Game/Durak/Repositories/RoleIndicatorRepository", fileName = "RoleIndicatorRepository")]
    public sealed class RoleIndicatorRepository
        : RepositoryBase<EPlayerRole, SerializableRole>
    {
        
    }

    [Serializable]
    public struct SerializableRole
    {
        [field: SerializeField]
        public Sprite Icon
        {
            get;
            private set;
        }

        [field: SerializeField]
        public string Label
        {
            get;
            private set;
        }
    }
}