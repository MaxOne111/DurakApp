using System.Collections.Generic;
using UnityEngine;

namespace Game.Durak.UI
{
    public sealed class TableView
        : MonoBehaviour
    {
        [SerializeField] private TableViewConfiguration configuration;

        [SerializeField] private RectTransform chairsParent;
        [SerializeField] private ChairView chairPrefab;

        public void Initialize(IEnumerable<PlayerInfo> players)
        {
            
        }
    }

    public sealed class PlayerInfo
    {
        
    }
}