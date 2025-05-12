using System;
using System.Collections;
using Game.Durak;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _Splash_Window;
    [SerializeField] private GameObject _Language_Window;
    [SerializeField] private GameObject _LogIn_Window;
    [SerializeField] private GameObject _Menu_Window;
    [SerializeField] private GameObject _Lobbies_Window;
    [SerializeField] private GameObject loadingBar;
    
    [SerializeField] private Button _Quit_Button;
    [SerializeField] private TextMeshProUGUI _User_Name;
    [SerializeField] private TextMeshProUGUI _User_Id;
    [SerializeField] private TextMeshProUGUI[] _Coins;

    [SerializeField] private LobbyListRequest _lobbyList;

    private bool _isAlive;

    private void Awake()
    {
        _Quit_Button.onClick.AddListener(Quit);
        
        CheckMenu();
    }

    private void OnEnable()
    {
        MenuEvents._On_Logged_In_Succesful += LogInComplete;
        GameEvents.OnCashChanged += ShowCoins;
    }
    
    private void Quit()
    {
        Application.Quit();
    }

    private void LogInComplete(string _username, int id, Wallet[] wallets)
    {
        _LogIn_Window.SetActive(false);
        _Menu_Window.SetActive(true);

        _User_Name.text = _username;
        _User_Id.text =  $"ID: {id}";
        ShowCoins(DurakHelper.GetWallet(wallets, WalletType.BonusBalance).balance);
    }

    private void ShowCoins(decimal count)
    {
        for (int i = 0; i < _Coins.Length; i++)
            _Coins[i].text = count.ToString();
    }
    
    private void CheckMenu()
    {
        if (!SceneMediator.FromGame)
            return;

        SetScreenOrientation _screenOrientation = new SetScreenOrientation();
        
        _screenOrientation.MenuSceneOrientation();
        
        SceneMediator.FromGame = false;
        
        _Splash_Window.SetActive(false);
        _Language_Window.SetActive(false);
        _LogIn_Window.SetActive(false);
        _Menu_Window.SetActive(true);
        _Lobbies_Window.SetActive(true);
        
        _lobbyList.ShowLobbies();

        _User_Name.text = SceneMediator.UserGameData.username;
        _User_Id.text = $"ID: {SceneMediator.UserGameData.user_id}";
        ShowCoins(DurakHelper.GetWallet(SceneMediator.UserGameData.wallets, WalletType.BonusBalance).balance);
    }

    public void LogOut()
    {
        _isAlive = false;
        
        PlayerPrefs.SetString("UserToken", "NULL");
        
        _LogIn_Window.SetActive(true);
        
        _Menu_Window.SetActive(false);
        
        loadingBar.SetActive(false);
    }

    private void OnDisable()
    {
        MenuEvents._On_Logged_In_Succesful -= LogInComplete;
        GameEvents.OnCashChanged -= ShowCoins;
    }

    private void OnDestroy()
    {
        _Quit_Button.onClick.RemoveListener(Quit);
    }
}