using Game.Durak.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Durak.UI
{
    public sealed class TextMessagePopup
        : MessagePopupBase<TextMessage>
    {
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text label;
        
        protected override void BeforeShow(TextMessage value)
        {
            background.color = value.BackgroundColor;
            label.color = value.TextColor;
            label.text = value.Text;
        }

        protected override void AfterShow()
        {
            Destroy(gameObject);
        }
    }
}