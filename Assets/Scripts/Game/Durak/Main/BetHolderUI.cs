using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Durak.Max
{
    public class BetHolderUI : MonoBehaviour
    {
        [Header("Bet holder settings")]
        [SerializeField] private TextMeshProUGUI betText;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color victoryColor;
        [SerializeField] private Color defeatColor;
    
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void ShowWinnerBetHolder(string value)
        {
            _image.color = victoryColor;
            betText.text = $"+{value}";
            betText.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
            betText.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InQuart);
        }
    
        public void ShowLooserBetHolder(string value)
        {
            _image.color = defeatColor;
            betText.text = $"-{value}";
            betText.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
            betText.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InQuart);
        }
    
        public void ShowDefaultBetHolder(string value)
        {
            _image.color = defaultColor;
            betText.text = $"{value}";

        }
    }
}