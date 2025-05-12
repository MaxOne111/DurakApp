using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Game.Countries;
using Game.Countries.Enums;
using Game.Durak;
using Game.Durak.Core;
using Game.Durak.Enums;
using Game.Durak.Max;
using Game.Durak.UI;
using Game.Max;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayerInfo = Game.Durak.PlayerInfo;

public class TestPlayer : MonoBehaviour
{
    private const float MessageVisibilityDuration = 3f;
    
    [SerializeField] private TextMeshProUGUI nameField;

    [SerializeField] private List<TestCard> sleeve;

    [SerializeField] private Image defenderFrame;
    
    [SerializeField] private Image attackerFrame;
    
    [SerializeField] private Image avatar;

    [SerializeField] private GameObject waitingRoot;
    [SerializeField] private GameObject activeRoot;
    [SerializeField] private GameObject disconnectMessage;

    [SerializeField] private Image icon;

    [SerializeField] private Vector2 sleeveCardSize;
    
    [Header("Messages")]
    [SerializeField] private Transform messageParent;
    [SerializeField] private Transform emojiParent;

    [SerializeField] private MessagePopupBase<TextMessage> messagePrefab;
    [SerializeField] private MessagePopupBase<string> emojiPrefab;

    [SerializeField] private TMP_Text actionMessageText;
    [SerializeField] private Image actionMessageBackground;

    [Header("Flag")]
    [SerializeField] private CountryFlagRepository countryFlagRepository;
    [SerializeField] private Image flagImage;
    [SerializeField] private Vector2 leftFlagPosition;
    [SerializeField] private Vector2 rightFlagPosition;
    
    [Header("Cards count indicator")]
    [SerializeField] private CardsCountIndicator cardsCountIndicator;

    private MessagePopupBase<TextMessage> _currentMessage;
    private MessagePopupBase<string> _currentEmoji;

    [Header("Winner signs")]
    [SerializeField] private Color blackout;

    [Header("Bet holder")] 
    [SerializeField] private BetHolderUI betHolderUI;
    
    [Space]
    [SerializeField] private TimerObject timerObject;

    private event Action<int> OnCardCountChanged; 

    private TimerObject _currentTimerObject;
    
    private Image _currentRoleFrame;

    private PlayerInfo _playerInfo;

    private EPlayerRole _role;
    
    private string _name;

    private int _slotNumber;

    private bool _timerIsPlaying;

    public EPlayerRole Role => _role;

    public PlayerInfo PlayerInfo => _playerInfo;

    public int SleeveCount => sleeve.Count;

    private void OnEnable()
    {
        OnCardCountChanged += cardsCountIndicator.SetAmount;
    }
    
    private void OnDisable()
    {
        OnCardCountChanged -= cardsCountIndicator.SetAmount;
    }
    
    public void Initialize(string name, int id, int balance)
    {
        waitingRoot.SetActive(false);
        activeRoot.SetActive(true);
        
        _playerInfo = new PlayerInfo(name, id, balance);

        _name = _playerInfo.username;
        
        nameField.text = _name;
        
        _role = EPlayerRole.Waiting;
    }

    public void SetFlagPosition(EFlagPosition position)
    {
        flagImage.rectTransform.anchoredPosition =
            position == EFlagPosition.Left ? leftFlagPosition : rightFlagPosition;
    }

    public void SetCardsCountIndicatorEnabled(bool isEnabled)
    {
        cardsCountIndicator.SetActive(isEnabled);
    }

    public void SetCountry(ECountry country)
    {
        var sprite = countryFlagRepository.GetFlag(country);
        flagImage.sprite = sprite;
    }

    //----------Temporary---------
    public void EnemyChangeIcon(Sprite icon) => GetComponent<Image>().sprite = icon;
    //----------------------------
    public void SetRole(EPlayerRole role) => _role = role;

    public void AddCard(TestCard card)
    {
        sleeve.Add(card);
        
        card.ActionInitialize(Action);
        
        OnCardCountChanged?.Invoke(sleeve.Count);
    }

    public void SetAvatar(Sprite sprite) => avatar.sprite = sprite;

    public void SetNameField(string name) => nameField.text = name;

    public async void MoveCardsToSleeve(Vector3[] points, float duration, int delay, Action playSound, Action callback)
    {
        SortCards();
        
        for (int i = 0; i < sleeve.Count; i++)
        {
            sleeve[i].SetSleevePosition(points[i]);
            ((RectTransform) sleeve[i].transform).sizeDelta = sleeveCardSize;
            sleeve[i]
                .transform
                .DOMove(points[i], duration)
                .SetEase(Ease.OutQuad)
                .SetLink(sleeve[i].gameObject);
            

            playSound?.Invoke();

            await Task.Delay(delay);
            callback.Invoke();
        }
    }

    public void ReturnSleeveToPlayer()
    {
        for (int i = 0; i < sleeve.Count; i++)
            sleeve[i].ReturnCardToBack();
    }

    public bool HaveCard(CardInfo cardInfo)
    {
        for (int i = 0; i < sleeve.Count; i++)
            if (cardInfo.rank == sleeve[i].CardInfo.rank &&
                cardInfo.suit == sleeve[i].CardInfo.suit)
                return true;

        return false;
    }
    
    public TestCard GetCard(CardInfo cardInfo) => DurakHelper.GetCard(sleeve, cardInfo);
    
    public void UnlockSleeve()
    {
        for (int i = 0; i < sleeve.Count; i++)
            sleeve[i].SetDraggable(true);
    }
    
    public void RemoveCard(TestCard card)
    {
        card.SetDraggable(false);

        sleeve.Remove(card);

        OnCardCountChanged?.Invoke(sleeve.Count);
    }
    
    public void DeleteCards()
    {
        if(sleeve.Count == 0)
            return;

        for (int i = 0; i < sleeve.Count; i++)
            Destroy(sleeve[i].gameObject);

        sleeve.Clear();
        
        OnCardCountChanged?.Invoke(sleeve.Count);
    }

    public void ShowDisconnectMessage()
    {
        disconnectMessage.SetActive(true);
    }

    public void HideDisconnectMessage()
    {
        disconnectMessage.SetActive(false);
    }
    
    public void ShowTextMessage(TextMessage message)
    {
        if (_currentMessage != null)
        {
            _currentMessage.StopAllCoroutines();
            Destroy(_currentMessage.gameObject);
        }
        
        var messageView = Instantiate(messagePrefab, messageParent);
        StartCoroutine(messageView.Show(message));

        _currentMessage = messageView;
    }

    public /*async*/ void ShowActionMessage(TextMessage message)
    {
        actionMessageBackground.gameObject.SetActive(true);
        
        actionMessageText.text = message.Text;
        actionMessageText.color = message.TextColor;
        actionMessageBackground.color = message.BackgroundColor;

        // wait Task.Delay(message.Delay);

        // actionMessageText.text = string.Empty;
        // actionMessageBackground.gameObject.SetActive(false);
    }

    public void HideActionMessage()
    {
        Debug.Log($"HideActionMessage. username: {_playerInfo.username}");
        actionMessageText.text = string.Empty;
        actionMessageBackground.gameObject.SetActive(false);
    }

    public void ShowEmoji(string emoji)
    {
        if (_currentEmoji != null)
        {
            _currentEmoji.StopAllCoroutines();
            Destroy(_currentEmoji.gameObject);
        }
    
        var messageView = Instantiate(emojiPrefab, emojiParent);
        StartCoroutine(messageView.Show(emoji));

        _currentEmoji = messageView;
    }

    public void ShowRoleFrame()
    {
        DisableFrames();
        
        switch (_role)
        {
            case EPlayerRole.Attacker:
                _currentRoleFrame = attackerFrame;
                break;
            case EPlayerRole.Enemy:
                _currentRoleFrame = defenderFrame;
                break;
        }

        if (!_currentRoleFrame)
            return;
        
        _currentRoleFrame.gameObject.SetActive(true);
    }

    public void ShowMinTrump(TestCard trump)
    {
        Transform card = cardsCountIndicator.GetRandomCard();

        float offset = 150f;
        
        Vector3 cardStartPos = card.transform.localPosition;
        
        Vector3 cardMovePos = card.up * offset;

        Vector3 cardStartRot = card.transform.localEulerAngles;
        
        Sprite defSprite = card.GetComponent<Image>().sprite;

        Vector3 startRot =  new Vector3(cardStartRot.x, 0, cardStartRot.z);
        Vector3 edgeRot = new Vector3(cardStartRot.z, -90, 0);
        Vector3 endRot = new Vector3(cardStartRot.x, -180, -cardStartRot.z);

        Sprite trumpSprite = trump.GetComponent<Image>().sprite;

        DOTween.Sequence()
            .Append(card.DOLocalMove(cardStartPos + cardMovePos, 0.5f)).SetLink(card.gameObject)
            .Append(card.DOLocalRotate(edgeRot, 0.5f)).SetLink(card.gameObject)
            .AppendCallback(() => card.GetComponent<Image>().sprite = trumpSprite).SetLink(card.gameObject)
            .Append(card.DOLocalRotate(endRot, 0.5f)).SetLink(card.gameObject)
            .AppendInterval(1f)
            .Append(card.DOLocalRotate(edgeRot, 0.5f)).SetLink(card.gameObject)
            .AppendCallback(() => card.GetComponent<Image>().sprite = defSprite).SetLink(card.gameObject)
            .Append(card.DOLocalRotate(startRot, 0.5f)).SetLink(card.gameObject)
            .Append(card.DOLocalMove(cardStartPos, 0.5f)).SetLink(card.gameObject);
    }
    
    public void SetCardAmount(int amount)
    {
        Debug.Log($"TestPlayer.SetCardAmount({amount})");
        cardsCountIndicator.SetAmount(amount);
    }
    
    public void StartTimer(float timerDuration)
    {
        _currentTimerObject = Instantiate(timerObject);
        _currentTimerObject.StartTimer(_currentRoleFrame, timerDuration);
    }
    
    public void StopTimer()
    {
        if (_currentTimerObject)
        {
            Destroy(_currentTimerObject.gameObject);
            _currentTimerObject = null;
        }
            
    }

    public void EnableAttackFrame()
    {
        if (_role is EPlayerRole.Enemy)
            return;
        
        _currentRoleFrame = attackerFrame;
        _currentRoleFrame.gameObject.SetActive(true);
    }

    public void DisableFrames()
    {
        defenderFrame.gameObject.SetActive(false);
        attackerFrame.gameObject.SetActive(false);
        
        _currentRoleFrame = null;
    }
    
    public void DarkenAvatar()
    {
        icon.color = blackout;
    }
    
    public void ShowWinnerBetHolder(string value)
    {
        betHolderUI.gameObject.SetActive(true);
        betHolderUI.ShowWinnerBetHolder(value);
    }
    
    public void ShowLoserBetHolder(string value)
    {
        betHolderUI.gameObject.SetActive(true);
        betHolderUI.ShowLooserBetHolder(value);
    }
    
    public void ShowDefaultBetHolder(string value)
    {
        if (!betHolderUI)
            return;
        
        betHolderUI.gameObject.SetActive(true);
        betHolderUI.ShowDefaultBetHolder(value);
    }

    public void HideBetHolder()
    {
        betHolderUI.gameObject.SetActive(false);
    }

    public void LightenAvatar() => icon.color = Color.white;

    private void Action(TestCard card, TestSlot slot, GameObject table)
    {
        switch (_role)
        {
            case EPlayerRole.Attacker:
                WebSocketConnection.AttackMove(card.CardInfo, table);
                break;
            case EPlayerRole.Enemy:
                WebSocketConnection.DefenceMove(card.CardInfo, slot);
                WebSocketConnection.TransferMove(card.CardInfo, slot);
                break;
            case EPlayerRole.Waiting: 
                WebSocketConnection.AttackMove(card.CardInfo, table);
                break;
        }

    }
    
    private void SortCards()
    {
        var sortedList = sleeve.OrderByDescending(card => card.IsTrump)
            .ThenByDescending(card => card.StrengthIndex);
            
        sleeve = sortedList.ToList();

        for (int i = 0; i < sleeve.Count; i++)
            sleeve[i].transform.SetAsLastSibling();
    }

}
