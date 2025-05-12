using System.Collections;
using System.Collections.Generic;
using Game.Durak;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyListRequest : MonoBehaviour
{
    private readonly string _url = DurakHelper.URLGame + "/list/";
    //"https://duraktest.sino0on.ru/api/lobby/list/";
    //"https://durak.sino0on.ru/api/lobby/list/";

    [SerializeField] private LobbyObject lobbyObject;

    [SerializeField] private Transform lobbiesContainer;

    private List<LobbyObject> lobbies = new List<LobbyObject>();

    private const int _connectAttempts = 3;

    private int _currentAttempt = 0;


    public void ShowLobbies() => StartCoroutine(Request());

    private void Start()
    {
        var lobby = Instantiate(lobbyObject, lobbiesContainer);
        lobby.InitializeDescription($"Создание лобби");
        lobby.GetComponent<Button>().onClick.AddListener(() =>
            StartCoroutine(DurakHelper.Loading(SceneManager.LoadSceneAsync("Durak Create Lobby"))));
    }

    private IEnumerator Request()
    {
        string url = DurakHelper.URLGame + "/list/";
        
        Debug.Log(url);
        
        UnityWebRequest request = UnityWebRequest.Get(url);

        request.timeout = 1;
        
        DurakHelper.CreateLoadBar();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
             if (_currentAttempt >= _connectAttempts)
             {
                 _currentAttempt = 0;
                 DurakHelper.DestroyLoadBar();
                 yield break;
             }
             
             _currentAttempt++;

            StartCoroutine(Request());
            yield break;
        }

        _currentAttempt = 0;
        
        DurakHelper.DestroyLoadBar();

        List<LobbyMessage> lobbiesInfo =
            JsonConvert.DeserializeObject<List<LobbyMessage>>(request.downloadHandler.text);

        UpdateLobbies(lobbiesInfo);
    }

    private void UpdateLobbies(List<LobbyMessage> lobbiesInfo)
    {
        if (lobbies.Count > 0)
        {
            for (int i = 0; i < lobbies.Count; i++)
                Destroy(lobbies[i].gameObject);

            lobbies.Clear();
        }

        for (int i = 0; i < lobbiesInfo.Count; i++)
        {
            var lobby = Instantiate(lobbyObject, lobbiesContainer);
            var lobbyInfo = lobbiesInfo[i];
            
            string transfer = (lobbyInfo.mod_transfer == "transfer") ? "Переводной" : "Подкидной";
            string isElimination = (lobbyInfo.elimination_mode) ? "Да" : "Нет";
            
            string playerCount = $"Максимальное кол-во игроков: {lobbyInfo.player_count}";
            string transferMode = $"Режим игры: {transfer}";
            string bet = $"Стоимость входа: {lobbyInfo.bank_amount}";
            string eliminationMode = $"На выбывание: {isElimination}";

            lobby.InitializeDescription($"ID: {lobbyInfo.id}",
                playerCount,
                transferMode,
                bet,
                eliminationMode);
            
            lobbies.Add(lobby);
            
            lobby.InitializeActions(() =>JoinAction(lobbyInfo), ()=>WatchAction(lobbyInfo));
        }
    }
    
    private void JoinAction(LobbyMessage lobbyInfo)
    {
        if (DurakHelper.GetWallet(SceneMediator.UserGameData.wallets, WalletType.BonusBalance).balance < lobbyInfo.bank_amount)
            return;
        
        LoadGameScene(lobbyInfo);
        SceneMediator.SetPlayerStatus(SceneMediator.EPlayerStatus.Player);
    }
    
    private void WatchAction(LobbyMessage lobbyInfo)
    {
        LoadGameScene(lobbyInfo);
        SceneMediator.SetPlayerStatus(SceneMediator.EPlayerStatus.Watcher);
    }
    
    private void LoadGameScene(LobbyMessage lobbyInfo)
    {
        SceneMediator.Room = lobbyInfo;
        
        StartCoroutine(DurakHelper.Loading(SceneManager.LoadSceneAsync("MaxScene")));
    }
}