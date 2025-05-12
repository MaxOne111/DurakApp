using System.Collections;
using System.Collections.Generic;
using Game.Durak;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TournamentListRequest : MonoBehaviour
{
    private readonly string _url = DurakHelper.URLGame + "/list/";
    //"https://duraktest.sino0on.ru/api/lobby/list/";
    //"https://durak.sino0on.ru/api/lobby/list/";

    [SerializeField] private TournamentObject tournamentObject;

    [SerializeField] private Transform lobbiesContainer;

    private List<TournamentObject> tournaments = new List<TournamentObject>();

    private const int _connectAttempts = 3;

    private int _currentAttempt = 0;


    public void LoadCreateTourMenu()
    {
        StartCoroutine(DurakHelper.Loading(SceneManager.LoadSceneAsync("Durak Create Tour")));
    }

    public void ShowTournaments() => StartCoroutine(TournamentsRequest());
    
    private IEnumerator TournamentsRequest()
    {
        string url = DurakHelper.URLTournamentGame + "tournament";
        
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

            StartCoroutine(TournamentsRequest());
            yield break;
        }

        _currentAttempt = 0;
        
        DurakHelper.DestroyLoadBar();

        List<TournamentMessage> lobbiesInfo =
            JsonConvert.DeserializeObject<List<TournamentMessage>>(request.downloadHandler.text);

        UpdateTournaments(lobbiesInfo);
    }
    
    private IEnumerator RoomRequest(int id)
    {
        string url = DurakHelper.URLTournamentGame + $"public/tournament/{id}";
        
        Debug.Log(url);
        
        UnityWebRequest request = UnityWebRequest.Get(url);

        request.timeout = 2;
        
        DurakHelper.CreateLoadBar();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            DurakHelper.DestroyLoadBar();
            
            yield break;
        }

        DurakHelper.DestroyLoadBar();

        TournamentRoomMessage roomInfo =
            JsonConvert.DeserializeObject<TournamentRoomMessage>(request.downloadHandler.text);

        LoadGameScene(roomInfo);
    }

    private void UpdateTournaments(List<TournamentMessage> tournamentsInfo)
    {
        if (tournaments.Count > 0)
        {
            for (int i = 0; i < tournaments.Count; i++)
                Destroy(tournaments[i].gameObject);

            tournaments.Clear();
        }

        for (int i = 0; i < tournamentsInfo.Count; i++)
        {
            var tournament = Instantiate(tournamentObject, lobbiesContainer);
            var tournamentInfo = tournamentsInfo[i];

            string playerCount = $"{tournamentInfo.player_count}";
            string bet = $"{tournamentInfo.bet_amount}";

            tournament.InitializeDescription($"ID: {tournamentInfo.id}",
                playerCount,
                bet);
            
            tournaments.Add(tournament);
            
            tournament.InitializeActions(() =>JoinAction(tournamentInfo));
        }
    }
    
    private void JoinAction(TournamentMessage tournamentInfo)
    {
        if (DurakHelper.GetWallet(SceneMediator.UserGameData.wallets, WalletType.BonusBalance).balance < tournamentInfo.bet_amount)
            return;
        
        SceneMediator.SetPlayerStatus(SceneMediator.EPlayerStatus.Player);

        StartCoroutine(RoomRequest(tournamentInfo.id));

    }
    
    private void WatchAction(TournamentRoomMessage tournamentRoomInfo)
    {
        LoadGameScene(tournamentRoomInfo);
        SceneMediator.SetPlayerStatus(SceneMediator.EPlayerStatus.Watcher);
    }
    
    private void LoadGameScene(TournamentRoomMessage tournamentRoomInfo)
    {
        SceneMediator.Room = tournamentRoomInfo;
        
        StartCoroutine(DurakHelper.Loading(SceneManager.LoadSceneAsync("MaxScene")));
    }
}