using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Durak.Abstractions;
using Game.Durak.Enums;
using Game.Durak.Network;
using Infrastructure;
using UnityEngine;
using WebSocketSharp;

//"https://hooly.space/api/lobby";
//"https://duraktest.sino0on.ru/api/lobby
//"https://durak.sino0on.ru/api/lobby";

namespace Game.Durak
{
    public static class DurakHelper
    {
        private static GameObject _currentLoadBar;

        private static string _urlEdge = "hooly.space";
        
        //private static string _urlGame = $"https://hooly.space/api/lobby"; 
        private static string _urlGame = $"https://durak.sino0on.ru/api/lobby";
        //private static string _urlHealthCheck = "https://hooly.space/api/health"; 
        private static string _urlHealthCheck = "https://durak.sino0on.ru/api/health";
        //private static string _wssGame = @$"wss://hooly.space/api/ws/";
        private static string _wssGame = @$"wss://durak.sino0on.ru/api/ws/";
        //private static string _urlTournamentGame = $"https://hooly.space/api/"; 
        private static string _urlTournamentGame = $"https://durak.sino0on.ru/api/"; 
        
        private static string _urlUser = $"https://userservice.hooly.space/api/user"; //not used

        public static string URLGame
        {
            get => _urlGame;
            private set => _urlGame = value;
        }
        
        public static string URLTournamentGame
        {
            get => _urlTournamentGame;
            private set => _urlTournamentGame = value;
        }

        public static string URLHealthCheck => _urlHealthCheck;

        
        public static string URLUser
        {
            get => _urlUser;
            private set => _urlUser = value;
        }
        
        public static string WSS
        {
            get => _wssGame;
            private set => _wssGame = value;
        }
        
        public static void SetGameUrl(string edge)
        {
            _urlEdge = edge;

            URLGame = $"https://{_urlEdge}/api/lobby";
            WSS = @$"wss://{_urlEdge}/api/ws/";
            
            Debug.Log("Game url: " + URLGame);
        }
        
        public static void SetUserUrl(string edge)
        {
            _urlEdge = edge;
            
            URLUser = $"https://{_urlEdge}/api/user";
            
            Debug.Log("User url: " + URLUser);
        }
        
        public static async Task<IWaitingDurakSession> CreateSession(DurakConfiguration configuration)
        {
            var service = new DurakLobbyService(WebHelper.Client);
            var id = await service.CreateLobby(configuration);
            
            var socket = CreateDurakSocket(id);
            Debug.Log($"ID: {id}");
            
            var result = new DurakSession(socket);
            result.Initialize();
            
            return result;
        }

        public static WebSocket CreateDurakSocket(int id)
        {
            const int port = 8000;
            
            var url = @$"ws://188.225.84.38:{port}/api/ws/{id}";
            
            var result = new WebSocket(url);
            return result;
        }
        
        public static TestCard GetCard(List<TestCard> cards, CardInfo cardInfo)
        {
            var result = cards.First(value => value.CardInfo.rank == cardInfo.rank &&
                                              value.CardInfo.suit == cardInfo.suit);

            return result;
        }
        
        public static TestCard GetCard(TestCard[] cards, CardInfo cardInfo)
        {
            var result = cards.First(value => value.CardInfo.rank == cardInfo.rank &&
                                              value.CardInfo.suit == cardInfo.suit);

            return result;
        }
        
        public static TestPlayer GetPlayer(List<TestPlayer> players, PlayerInfo playerInfo)
        {
            var result = players.First(value => value.PlayerInfo.user_id == playerInfo.user_id);

            return result;
        }
        
        public static TestPlayer GetPlayer(TestPlayer[] players, PlayerInfo playerInfo)
        {
            var result = players.First(value => value.PlayerInfo.user_id == playerInfo.user_id);

            return result;
        }
        
        public static TestPlayer GetPlayer(TestPlayer[] players, int id)
        {
            var result = players.First(value => value.PlayerInfo.user_id == id);
            return result;
        }
        
        public static TestPlayer GetPlayer(List<TestPlayer> players, int id)
        {
            var result = players.First(value => value.PlayerInfo.user_id == id);

            return result;
        }
        
        public static void CheckTrump(ECardSuit trumpSuit, TestCard card)
        {
            if (card.CardInfo.suit == trumpSuit)
                card.DoTrump();
        }

        public static bool IsPlayer(TestPlayer player, TestPlayer otherPlayer)
        {
            if (player.PlayerInfo.user_id != otherPlayer.PlayerInfo.user_id)
                return false;

            return true;
        }
        
        public static bool IsPlayer(TestPlayer player, PlayerInfo otherPlayerInfo)
        {
            if (player.PlayerInfo.user_id != otherPlayerInfo.user_id)
                return false;

            return true;
        }

        public static void CreateLoadBar()
        {
            if (_currentLoadBar)
                DestroyLoadBar();
            
            GameObject loadBarPrefab = Resources.Load<GameObject>("LoadBar/LoadBar");

            _currentLoadBar = Object.Instantiate(loadBarPrefab, GameObject.FindWithTag("Canvas").transform);
        }
        
        public static void CreateLoadBar(Transform parent)
        {
            if (_currentLoadBar)
                DestroyLoadBar();
            
            GameObject loadBarPrefab = Resources.Load<GameObject>("LoadBar/LoadBar");

            _currentLoadBar = Object.Instantiate(loadBarPrefab, parent);
        }

        public static void DestroyLoadBar()
        {
            if (!_currentLoadBar)
                return;
            
            Object.Destroy(_currentLoadBar.gameObject);

            _currentLoadBar = null;
        }
        
        public static IEnumerator Loading(AsyncOperation asyncOperation)
        {
            CreateLoadBar();
            
            while (!asyncOperation.isDone)
                yield return null;
            
            DestroyLoadBar();
        }
        
        public static Wallet GetWallet(Wallet[] wallet, WalletType type)
        {
            if (wallet == null)
                return new Wallet(); 
            
            for (int i = 0; i < wallet.Length; i++)
            {
                if (wallet[i].wallet_type == type)
                {
                    return wallet[i];
                }
            }

            return new Wallet();
        }
        

    }
}