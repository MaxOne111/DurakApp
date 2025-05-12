using UnityEngine;

namespace Game.Durak.UI
{
    public sealed class EmojiMessagePopup
        : MessagePopupBase<string>
    {
        [SerializeField] private EmojiRepository emojiRepository;
        
        [SerializeField] private GameObject root;
        [SerializeField] private EmojiView emoji;

        protected override void BeforeShow(string value)
        {
            var frames = emojiRepository.GetEmoji(value);
            
            root.SetActive(true);
            emoji.SetAnimation(frames);
        }

        protected override void AfterShow()
        {
            if (!root)
                return;
            
            root.SetActive(false);
            Destroy(gameObject);
        }
    }
}