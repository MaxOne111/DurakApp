using System;
using System.Collections.Concurrent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Durak.Core
{
    public sealed class PlayerView
        : MonoBehaviour
    {
        [SerializeField] private Color idleColor;
        [SerializeField] private Color readyColor;
        
        [SerializeField] private Image avatarImage;
        [SerializeField] private Image readyIndicator;
        [SerializeField] private TMP_Text nameText;

        private readonly ConcurrentStack<Action> _actions = new ConcurrentStack<Action>();

        public void SetAvatar(Sprite avatar)
        {
            _actions.Push(() => avatarImage.sprite = avatar);
        }

        public void SetNickname(string nickname)
        {
            _actions.Push(() => nameText.text = nickname);
        }

        public void SetReady(bool isReady)
        {
            _actions.Push(() => readyIndicator.color = isReady ? readyColor : idleColor);
        }

        private void Update()
        {
            while (_actions.TryPop(out var callback))
            {
                callback.Invoke();
            }
        }
    }
}