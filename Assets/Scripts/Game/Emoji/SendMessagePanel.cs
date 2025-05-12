using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Game.Durak.Enums;
using Game.Max;
using Game.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game.Durak.UI
{
    public sealed class SendMessagePanel
        : MonoBehaviour
    {
        [SerializeField] private WebSocketConnection connection;
        
        [SerializeField] private Image avatarImage;
        [SerializeField] private TMP_Text nicknameText;
        [SerializeField] private TMP_Text idText;
        [SerializeField] private Image flagImage;

        [SerializeField] private Color passiveButtonColor;
        [SerializeField] private Color activeButtonColor;

        [SerializeField] private Button recentButton;
        [SerializeField] private Button initialButton;
        [SerializeField] private List<ButtonPanelPair> panels;

        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private IDisposable _buttonsToken;
        
        private async void SetActiveButton(Button button)
        {
            var player = connection.GetPlayerInfo();
            nicknameText.text = player.username;
            idText.text = $"ID: {player.user_id.ToString()}";
            
            _tokenSource?.Cancel();
            
            var target = panels.First(value => value.Button == button);
            foreach (var panel in panels)
            {
                panel.Button.image.color = passiveButtonColor;  
            }

            target.Button.image.color = activeButtonColor;

            _tokenSource = new CancellationTokenSource();
            var result = await target.Panel.Select(_tokenSource.Token);
            switch (result.Type)
            {
                case EChatMessageType.Emoji:
                    connection.SendEmoji(result.Text);
                    break;
                
                case EChatMessageType.Message:
                    connection.SendTextMessage(result.Text);
                    break;
            }
            
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            RecentMessagesHelper.Initialize();
            var recent = RecentMessagesHelper.GetRecent();
            SetActiveButton(recent.Any() ? recentButton : initialButton);

            var token = new CompositeDisposable();
            foreach (var panel in panels)
            {
                var panelToken = panel.Button.onClick.AddDisposableListener(Listen);
                token.Add(panelToken);
                
                void Listen()
                {
                    SetActiveButton(panel.Button);
                }
            }

            _buttonsToken = token;
        }

        private void OnDisable()
        {
            _buttonsToken.Dispose();
            
            RecentMessagesHelper.Dispose();
        }

        [Serializable]
        private struct ButtonPanelPair
        {
            [field: SerializeField]
            public Button Button
            {
                get;
                private set;
            }

            [field: SerializeField]
            public SelectPanelBase<ChatMessage> Panel
            {
                get;
                private set;
            }
        }
    }
}