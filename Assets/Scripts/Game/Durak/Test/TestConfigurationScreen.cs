using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Durak.Abstractions;
using Game.Durak.Enums;
using Game.UI;
using Game.UI.Abstractions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
using Utils;
using Zenject;

namespace Game.Durak.Test
{
    public sealed class TestConfigurationScreen
        : MonoBehaviour
    {
        [SerializeField] private Slider betSlider;
        
        [SerializeField] private TMP_Dropdown cardsDropdown;
        [SerializeField] private TMP_Dropdown modeDropdown;
        [SerializeField] private TMP_Dropdown chairsDropdown;

        [SerializeField] private Toggle speedToggle;

        [SerializeField] private Button applyButton;

        private static readonly IReadOnlyList<int> CardCounts = new[]
        {
            24,
            36,
            54
        };

        private static readonly IReadOnlyList<int> ChairCounts = new[]
        {
            3,
            4,
            5,
            6
        };
        
        private static readonly IReadOnlyList<ETransferMode> TransferModes = new[]
        {
            ETransferMode.Transfer,
            ETransferMode.Tossing
        };
        
        public async Task<DurakConfiguration> WaitForApply()
        {
            var isClicked = false;
            
            applyButton.onClick.AddListener(Listen);
            await AsyncHelper.WaitUntil(() => isClicked);
            applyButton.onClick.RemoveListener(Listen);
            
            var cardsCount = CardCounts[cardsDropdown.value];
            var chairsCount = ChairCounts[chairsDropdown.value];
            var transferMode = TransferModes[modeDropdown.value];
            var bet = (int) betSlider.value;
            var isFast = speedToggle.isOn;

            var configuration = new DurakConfiguration(
                bet, 
                chairsCount,
                cardsCount,
                isFast,
                transferMode,
                EWhoTransferMode.Neighbours,
                EDeceptionMode.Deception,
                EGameMode.Classic,
                false,
                string.Empty,
                EBalanceType.Default,
                false);

            return configuration;

            void Listen()
            {
                isClicked = true;
            }
        }
    }
}