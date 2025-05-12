using System;
using System.Collections.Generic;
using System.Threading;
using Game.Durak.Enums;
using Game.Durak.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DurakGameUI : MonoBehaviour
{
    public event Action<string> OnSelectTextMessage = _ => { };
    public event Action<string> OnSelectEmoji = _ => { };

    [Header("Action buttons")]
    [SerializeField] private Button joinButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private Button beatButton;
    [SerializeField] private Button takeButton;
    [SerializeField] private Button passButton;
    
    [Header("Role images")]
    [SerializeField] private RoleIndicator roleIndicator;
    
    [Header("Text")] 
    [SerializeField] private TextMeshProUGUI lobbyID;
    [SerializeField] private TextMeshProUGUI bet;
    [SerializeField] private TextMeshProUGUI cardsCount;
    
    [Header("Deck")]
    [SerializeField] private DeckView deckView;

    [SerializeField] private DisconnectPanelUI disconnectPanel;
    
    private readonly List<Button> _actionButtons = new List<Button>();

    private CancellationTokenSource _emojiToken;
    private CancellationTokenSource _messageToken;
    

    private void Start()
    {
        AddButtons();
    }

    public Button Join => joinButton;
    public Button Ready => readyButton;
    public Button Beat => beatButton;
    public Button Take => takeButton;
    public Button Pass => passButton;

    public void InitializeTryConnectBtn(UnityAction action)
    {
        disconnectPanel.InitializeTryConnect(action);
    }
    
    public void InitializeQuitBtn(UnityAction action)
    {
        disconnectPanel.InitializeQuit(action);
    }
    
    public void SwitchButton(Button button)
    {

        for (int i = 0; i < _actionButtons.Count; i++)
        {
            if (button == _actionButtons[i])
            {
                button.gameObject.SetActive(true);
                continue;
            }

            if (_actionButtons[i])
                _actionButtons[i].gameObject.SetActive(false);
        }
    }
    
    public void ChangeRoleImage(EPlayerRole role)
    {
        roleIndicator.SetRole(role);
    }

    public void ShowLobbyID(int id) => lobbyID.text = $"ID: {id:000-000}";
    
    public void ShowLobbyBet(decimal count) => bet.text = $"Bet: {count}";

    public void ShowLobbyCardsCount(int count) => cardsCount.text = $"{count}";

    public void SetTrumpSuit(ECardSuit suit)
    {
        deckView.SetTrumpSuit(suit);
    }
    
    public void ShowCardsCount(int count)
    {
        deckView.SetCardsCount(count);
    }

    public void DisableButton(Button button)
    {

        for (int i = 0; i < _actionButtons.Count; i++)
        {
            if (button != _actionButtons[i])
                continue;

            button.gameObject.SetActive(false);
            break;
        }
    }

    public void DisableButtons()
    {
        for (int i = 0; i < _actionButtons.Count; i++)
            _actionButtons[i].gameObject.SetActive(false);
    }
    
    private void AddButtons()
    {
        _actionButtons.Add(joinButton);
        _actionButtons.Add(readyButton);
        _actionButtons.Add(beatButton);
        _actionButtons.Add(takeButton);
        _actionButtons.Add(passButton);
    }
}