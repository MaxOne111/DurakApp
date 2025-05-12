using Game.Durak.Enums;
using UnityEngine;

namespace Game.Durak.Repositories
{
    [CreateAssetMenu(menuName = "Game/Durak/Repositories/TrumpIndicatorRepository", fileName = "TrumpIndicatorRepository")]
    public sealed class TrumpIndicatorRepository
        : RepositoryBase<ECardSuit, Sprite>
    {
        
    }
}