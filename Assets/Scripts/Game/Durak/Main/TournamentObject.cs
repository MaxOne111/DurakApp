using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TournamentObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI playerCount;
    [SerializeField] private TextMeshProUGUI bet;
    [SerializeField] private Button joinButton;

    public void InitializeDescription(string headerText,
        string playerCount = null,
        string bet = null
    )
    {
        header.text = headerText;
        this.playerCount.text = playerCount;
        this.bet.text = bet;
    }

    public void InitializeActions(UnityAction join)
    {
        joinButton.onClick.AddListener(join);
    }
}