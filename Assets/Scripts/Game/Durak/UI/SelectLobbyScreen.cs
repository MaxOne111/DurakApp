using Game.Durak.Enums;
using Mopsicus.TwinSlider;
using UnityEngine;

namespace Game.Durak.UI
{
    public sealed class SelectLobbyScreen
        : MonoBehaviour
    {
        [SerializeField] private TwinSlider betSlider;
        
        [SerializeField] private PlayersToggleGroup playersToggleGroup;
        [SerializeField] private DeckToggleGroup deckToggleGroup;
        [SerializeField] private TransferModeToggleGroup transferModeToggleGroup;
        [SerializeField] private WhoTransferToggleGroup whoTransferToggleGroup;
        [SerializeField] private GameModeToggleGroup gameModeToggleGroup;
        [SerializeField] private GameSpeedToggleGroup gameSpeedToggleGroup;

        public DurakConfiguration CreateConfiguration()
        {
            var result = new DurakConfiguration
            (
                (int)betSlider.Min,
                playersToggleGroup.SelectedValue,
                deckToggleGroup.SelectedValue,
                gameSpeedToggleGroup.SelectedValue == EGameSpeed.Fast,
                transferModeToggleGroup.SelectedValue,
                whoTransferToggleGroup.SelectedValue,
                EDeceptionMode.Deception,
                gameModeToggleGroup.SelectedValue,
                false,
                string.Empty,
                EBalanceType.Default,
                false
            );

            return result;
        }
    }
}