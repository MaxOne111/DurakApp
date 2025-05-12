using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LobbyObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI playerCount;
    [SerializeField] private TextMeshProUGUI transferMode;
    [SerializeField] private TextMeshProUGUI bet;
    [SerializeField] private TextMeshProUGUI eliminationMode;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private TextMeshProUGUI buttonLabel;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button watchButton;

    public void InitializeDescription(string headerText,
        string playerCount = null,
        string transferMode = null,
        string bet = null,
        string eliminationMode = null
        )
    {
        header.text = headerText;
        this.playerCount.text = playerCount;
        this.transferMode.text = transferMode;
        this.bet.text = bet;
        this.eliminationMode.text = eliminationMode;
    }

    public void InitializeActions(UnityAction join, UnityAction watch)
    {
        joinButton.onClick.AddListener(join);
        watchButton.onClick.AddListener(watch);
    }
}