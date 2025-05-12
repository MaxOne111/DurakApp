using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Durak.Enums;
using Game.Durak.Network;
using Infrastructure;
using Mopsicus.TwinSlider;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Durak.UI
{
    public sealed class CreateTourScreen
        : MonoBehaviour
    {
        [SerializeField] private TwinSlider betSlider;
        [SerializeField] private BetSliderText tempBetSlider;
        
        [SerializeField] private List<ToggleValuePair<int>> playersGroup;
        [SerializeField] private List<ToggleValuePair<int>> deckGroup;
        [SerializeField] private List<ToggleValuePair<ETransferMode>> transferModeGroup;
        [SerializeField] private List<ToggleValuePair<EWhoTransferMode>> whoTransferGroup;
        [SerializeField] private List<ToggleValuePair<EGameMode>> gameModeGroup;
        [SerializeField] private List<ToggleValuePair<EGameSpeed>> gameSpeedGroup;
        [SerializeField] private ToggleValuePair<bool> forElimination;

        [SerializeField] private Button createButton;

        private bool _isLoading;

        private async void HandleCreate()
        {
            if (_isLoading)
                return;
            
            _isLoading = true;
            
            //Debug.Log(betSlider.Min);
            //Debug.Log(betSlider.Max);
            
            //var bet = (int)betSlider.Min;
            var bet = (int)tempBetSlider.Bet;
            var playerCount = GetValue(playersGroup);
            var deckCount = GetValue(deckGroup);
            var isFast = GetValue(gameSpeedGroup) == EGameSpeed.Fast;
            var transferMode = GetValue(transferModeGroup);
            var whoTransfer = GetValue(whoTransferGroup);
            var deceptionMode = EDeceptionMode.Deception;
            var gameMode = GetValue(gameModeGroup);
            var isPrivate = false;
            var isPublic = true;
            var isTest = false;
            var password = string.Empty;
            var balance = EBalanceType.Default;

            var configuration = new DurakTournamentConfiguration
            (
                bet,
                playerCount,
                deckCount,
                isFast,
                transferMode,
                whoTransfer,
                deceptionMode,
                gameMode,
                isPrivate,
                isPublic,
                isTest,
                password,
                balance
            );


            if (DurakHelper.GetWallet(SceneMediator.UserGameData.wallets, WalletType.BonusBalance).balance < bet)
            {
                _isLoading = false;
                return;
            }
            
            Debug.Log($"Initialized configuration: {JsonConvert.SerializeObject(configuration)}");

            string url = DurakHelper.URLTournamentGame + "tournament";
            
            var service = new DurakLobbyService(WebHelper.Client);
            var tourId = await service.CreateLobby(configuration, url);
            
            SceneMediator.SetPlayerStatus(SceneMediator.EPlayerStatus.Player);
            
            Debug.Log($"Created tour with id: {tourId}");
            
            //PlayerPrefs.SetInt("lobbyID", lobbyId);

            PlayerGameData data = new PlayerGameData();

            await data.UpdatePlayerDataAsync();
            
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync("MaxScene");

            SceneMediator.Room = await RoomRequest(tourId);

            if (SceneMediator.Room.ID == 0)
                return;
            
            try
            {
                StartCoroutine(DurakHelper.Loading(loadSceneAsync));
            }
            finally
            {
                _isLoading = false;
            }

        }
        
        private T GetValue<T>(IEnumerable<ToggleValuePair<T>> pairs)
        {
            var result = pairs.First(value => value.Toggle.isOn).Value;
            return result;
        }
        
        private async Task<TournamentRoomMessage> RoomRequest(int id)
        {
            string url = DurakHelper.URLTournamentGame + $"public/tournament/{id}";
        
            Debug.Log(url);
        
            UnityWebRequest request = UnityWebRequest.Get(url);

            request.timeout = 2;

            request.SendWebRequest();
            
            while (!request.isDone)
            {
                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                return new TournamentRoomMessage();
            }

            TournamentRoomMessage roomInfo =
                JsonConvert.DeserializeObject<TournamentRoomMessage>(request.downloadHandler.text);

            return roomInfo;

        }

        public void Quit()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync("Menu");
            
            StartCoroutine(DurakHelper.Loading(loadSceneAsync));
        }

        private void OnEnable()
        {
            createButton.onClick.AddListener(HandleCreate);
        }

        private void OnDisable()
        {
            createButton.onClick.RemoveListener(HandleCreate);    
        }

        [Serializable]
        private struct ToggleValuePair<T>
        {
            // [field: SerializeField]
            // public Toggle Toggle
            // {
            //     get;
            //     private set;
            // }

            public Toggle Toggle;

            public T Value;

            // [field: SerializeField]
            // public T Value
            // {
            //     get;
            //     private set;
            // }
        }
    }
}