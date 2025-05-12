using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Durak.UI
{
    public sealed class ChairView
        : MonoBehaviour
    {
        [SerializeField] private GameObject playerParent;
        [SerializeField] private GameObject buttonParent;

        [SerializeField] private Button button;
        
        [SerializeField] private Image avatar;
        [SerializeField] private TMP_Text nameLabel;

        private void SetPlayer(Sprite playerAvatar, string playerName)
        {
            buttonParent.SetActive(false);
            playerParent.SetActive(true);

            avatar.sprite = playerAvatar;
            nameLabel.text = playerName;
        }

        private void SetAvailable(Action callback)
        {
            buttonParent.SetActive(true);
            playerParent.SetActive(false);
            
            button.onClick.AddListener(InvokeCallback);

            void InvokeCallback()
            {
                button.onClick.RemoveListener(InvokeCallback);
                callback.Invoke();
            }
        }
    }
}