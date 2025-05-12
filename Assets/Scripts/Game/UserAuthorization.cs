using System.Collections;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Game.Durak;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

public class UserAuthorization
{
    private string _Registration_Address;
    private string _Log_In_Address;
    private string _User_Data_Address;
    private string _dailyBonusAddress;
    private string _User_Token;

    private readonly string _Authorization = "Authorization";

    private MonoBehaviour _Mono;

    private static int _globalAttempt = 3;
    private static int _currentAttempt = 0;
    
    public UserAuthorization(MonoBehaviour _mono,
        string _registration_Address,
        string _log_In_Address,
        string _user_Data_Address,
        string dailyBonusAddress)
    {
        _Mono = _mono;
        _Registration_Address = _registration_Address;
        _Log_In_Address = _log_In_Address;
        _User_Data_Address = _user_Data_Address;
        _dailyBonusAddress = dailyBonusAddress;
        _User_Token = null;
    }

    public IEnumerator RegisterUser(UserRegistrationData _registration_Data)
    {
        yield return _Mono.StartCoroutine(UserRegistration(_registration_Data));
    }
    
    public IEnumerator LogInUser(UserLoginData _login_Data)
    {
        yield return _Mono.StartCoroutine(UserLogIn(_login_Data));
    }
    
    public IEnumerator LogInWithTokenUser()
    {
        yield return _Mono.StartCoroutine(UserDataGetRequest());
    }

    public bool IsFirstEntry()
    {
        _User_Token = PlayerPrefs.GetString("UserToken", "NULL");

        if (_User_Token != "NULL")
            return false;
        
        return true;
    }
    
    private IEnumerator UserRegistration(UserRegistrationData _user_Registration_Data)
    {
        string _json_String = JsonUtility.ToJson(_user_Registration_Data);
        string _content_Type = "application/json";

        UnityWebRequest _web_Request = UnityWebRequest.Post(_Registration_Address, _json_String, _content_Type);
        
        yield return _web_Request.SendWebRequest();
        
        Debug.Log(_web_Request.url);
        
        if (!IsRequestSuccessful(_web_Request))
        {
            MenuEvents.LoggedInFailed();
            Debug.Log($"Error in registration");
            Debug.Log(_web_Request.downloadHandler.error);
            Debug.Log(_web_Request.downloadHandler.text);
            Debug.Log(_web_Request.responseCode);
            yield break;
        }

        UserLoginData _login_Data = new UserLoginData
        {
            username = _user_Registration_Data.username,
            password = _user_Registration_Data.password,
        };

        yield return _Mono.StartCoroutine(UserLogIn(_login_Data));
        
        _web_Request.Dispose();
    }
    
    private IEnumerator UserLogIn(UserLoginData _user_Login_Data)
    {
        string _json_String = JsonUtility.ToJson(_user_Login_Data);
        string _content_Type = "application/json";
        
        UnityWebRequest _web_Request = UnityWebRequest.Post(_Log_In_Address, _json_String, _content_Type);

        yield return _web_Request.SendWebRequest();

        Debug.Log(_web_Request.url);
        
        if (!IsRequestSuccessful(_web_Request))
        {
            MenuEvents.LoggedInFailed();
            Debug.Log(_web_Request.responseCode);
            Debug.Log(_Log_In_Address);
            Debug.Log($"Error in login");
            yield break;
        }
        
        UserToken _user_Token = JsonUtility.FromJson<UserToken>(_web_Request.downloadHandler.text);

        _User_Token = _user_Token.access_token;
        
        PlayerPrefs.SetString("UserToken", _User_Token);
        
        yield return _Mono.StartCoroutine(UserDataGetRequest());
        
        _web_Request.Dispose();
    }
    
    private IEnumerator UserDataGetRequest()
    {
        if (_User_Token is null or "NULL")
            yield break;
        
        string _token = "Bearer " + _User_Token;
        
        UnityWebRequest _web_Request = UnityWebRequest.Get(_User_Data_Address);
        
        _web_Request.timeout = 2;
        
        _web_Request.SetRequestHeader(_Authorization, _token);
        
        yield return _web_Request.SendWebRequest();

        if (_web_Request.result != UnityWebRequest.Result.Success)
        {
            if (_currentAttempt >= _globalAttempt)
            {
                _currentAttempt = 0;
                yield break;
            }
            
            _currentAttempt++;
            _Mono.StartCoroutine(UserDataGetRequest());
            yield break;
        }
        
        SuccessfulRequest _request = JsonConvert.DeserializeObject<SuccessfulRequest>(_web_Request.downloadHandler.text);

        _currentAttempt = 0;
        
        if (_request.detail == "Could not validate credentials")
            yield break;

        MenuEvents.LoggedInSuccessful(_request.username, _request.id, _request.wallets);
        
        SceneMediator.SetPlayerInfo(_request.username, _request.id, _request.avatar, _request.wallets);
        
        _web_Request.Dispose();
    }
    
    
    private bool IsRequestSuccessful(UnityWebRequest _request)
    {
        if (_request.result != UnityWebRequest.Result.Success)
            return false;

        return true;
    }

}

public class PlayerGameData
{
    private string _url = "https://userservice.sino0on.ru/api/user/user/";
    //private string _url = "https://userservice.hooly.space/api/user/user/";
    
    private static int _globalAttempt = 3;
    private static int _currentAttempt = 0;
    
    public async Task<SuccessfulRequest> UpdatePlayerDataAsync()
    {
        var data = await PlayerGetData();
        return data;
    }

    private async Task<SuccessfulRequest> PlayerGetData()
    {
        var userToken = PlayerPrefs.GetString("UserToken", "NULL");

        if (userToken is null or "NULL")
            return new SuccessfulRequest();

        string _token = "Bearer " + userToken;

        using (UnityWebRequest _web_Request = UnityWebRequest.Get(_url))
        {
            _web_Request.timeout = 2;
        
            _web_Request.SetRequestHeader("Authorization", _token);
        
            var operation = _web_Request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (_web_Request.result != UnityWebRequest.Result.Success)
            {
                if (_currentAttempt > _globalAttempt)
                {
                    _currentAttempt = 0;
                    return new SuccessfulRequest();
                }
                
                _currentAttempt++;
                await PlayerGetData();
            }

            SuccessfulRequest _request = JsonConvert.DeserializeObject<SuccessfulRequest>(_web_Request.downloadHandler.text);
        
            _currentAttempt = 0;

            if (_request.detail == "Could not validate credentials")
            {
                return new SuccessfulRequest();
            }
            
            SceneMediator.SetPlayerInfo(_request.username, _request.id, _request.avatar, _request.wallets);

            _web_Request.Dispose();
            
            return _request;
            
        }

    }
    
    
}