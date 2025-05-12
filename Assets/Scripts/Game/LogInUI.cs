using System;
using System.Collections;
using Game.Durak;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogInUI : MonoBehaviour
{
    [Header("Request data")]
    [SerializeField] private string _Registration_Address;
    [SerializeField] private string _Log_In_Address;
    [SerializeField] private string _User_Data_Address;
    [SerializeField] private string dailyBonusAddress;
    [Header("UI")] 
    [SerializeField] private GameObject _Loading_Bar;
    
    [SerializeField] private TMP_InputField _Name_Input;
    [SerializeField] private TMP_InputField _Password_Input;
    

    [Header("Registration fields")]
    [SerializeField] private Button _Registration_Button;
    [SerializeField] private TMP_InputField _Password_Repeat_Input;
    [SerializeField] private TextMeshProUGUI _Registration_Tab_Name;
    [SerializeField] private Button _Have_Account_Button;
    
    [Header("Log In fields")]
    [SerializeField] private Button _LogIn_Button;
    [SerializeField] private TextMeshProUGUI _LogIn_Tab_Name;
    [SerializeField] private Button _Have_Not_Account_Button;
    
    [Header("Log In Failed")] 
    [SerializeField] private GameObject _Valid_Information;
    [SerializeField] private GameObject _Incorrect_Password;

    private UserAuthorization _User_Authorization;
    
    
    
    private void OnEnable()
    {
        _LogIn_Button.onClick.AddListener(LogInButton);
        _Registration_Button.onClick.AddListener(RegistrationButton);

        _Have_Account_Button.onClick.AddListener(delegate { ShowLogInFields(); HideRegistrationFields(); });
        _Have_Not_Account_Button.onClick.AddListener(delegate { ShowRegistrationFields(); HideLogInFields(); });

        MenuEvents._On_Logged_In_Failed += ShowIncorrectInput;
    }

    private void Awake()
    {
        _User_Authorization = new UserAuthorization(this, _Registration_Address, _Log_In_Address, _User_Data_Address,
            dailyBonusAddress);
    }

    private void Start() => FirstEntry();

    private IEnumerator Registration()
    {
        HideIncorrectInput();
        
        if (IsInputFieldsEmpty())
        {
            MenuEvents.LoggedInFailed();
            yield break;
        }
        
        UserRegistrationData _user_Registration_Data = new UserRegistrationData
        {
            username = _Name_Input.text,
            avatar = "avatar",
            password = _Password_Input.text,
            password_repeat = _Password_Repeat_Input.text,
        };

        if (_user_Registration_Data.password_repeat != _user_Registration_Data.password)
        {
            MenuEvents.LoggedInFailed();
            yield break;
        }
        
        _User_Authorization = new UserAuthorization(this, _Registration_Address, _Log_In_Address, _User_Data_Address,
            dailyBonusAddress);
        
        _Loading_Bar.SetActive(true);
        
        yield return StartCoroutine(_User_Authorization.RegisterUser(_user_Registration_Data));

        _Loading_Bar.SetActive(false);
    }

    private IEnumerator LogIn()
    {
        HideIncorrectInput();
        
        if (IsInputFieldsEmpty())
        {
            MenuEvents.LoggedInFailed();
            yield break;
        }
        
        UserLoginData userLoginData = new UserLoginData
        {
            username = _Name_Input.text,
            password = _Password_Input.text,
        };

        _User_Authorization = new UserAuthorization(this, _Registration_Address, _Log_In_Address, _User_Data_Address,
            dailyBonusAddress);
        
        _Loading_Bar.SetActive(true);
        
        yield return StartCoroutine(_User_Authorization.LogInUser(userLoginData));
        
        _Loading_Bar.SetActive(false);
    }
    
    private IEnumerator LogInWithToken()
    {
        HideIncorrectInput();
        
        _Loading_Bar.SetActive(true);
        
        yield return StartCoroutine(_User_Authorization.LogInWithTokenUser());
        
        _Loading_Bar.SetActive(false);
    }

    private void ShowIncorrectInput()
    {
        _Valid_Information.SetActive(true);   
        _Incorrect_Password.SetActive(true);   
    }

    private void HideIncorrectInput()
    {
        _Valid_Information.SetActive(false);   
        _Incorrect_Password.SetActive(false);
    }
    
    private void FirstEntry()
    {
        if (_User_Authorization.IsFirstEntry())
        {
            ShowRegistrationFields();
            HideLogInFields();
            
            return;
        }

        StartCoroutine(LogInWithToken());
        
        
        ShowLogInFields();
        HideRegistrationFields();
    }

    private void ShowRegistrationFields()
    {
        _Registration_Button.gameObject.SetActive(true);
        _Password_Repeat_Input.gameObject.SetActive(true);
        _Registration_Tab_Name.gameObject.SetActive(true);
        _Have_Account_Button.gameObject.SetActive(true);
    }

    private void ShowLogInFields()
    {
        _LogIn_Button.gameObject.SetActive(true);
        _LogIn_Tab_Name.gameObject.SetActive(true);
        _Have_Not_Account_Button.gameObject.SetActive(true);
    }
    
    private void HideRegistrationFields()
    {
        _Registration_Button.gameObject.SetActive(false);
        _Password_Repeat_Input.gameObject.SetActive(false);
        _Registration_Tab_Name.gameObject.SetActive(false);
        _Have_Account_Button.gameObject.SetActive(false);
    }

    private void HideLogInFields()
    {
        _LogIn_Button.gameObject.SetActive(false);
        _LogIn_Tab_Name.gameObject.SetActive(false);
        _Have_Not_Account_Button.gameObject.SetActive(false);
    }

    private bool IsInputFieldsEmpty()
    {
        if (_Name_Input.text.Length == 0 || _Password_Input.text.Length == 0)
            return true;

        return false;
    }
    
    private void LogInButton() => StartCoroutine(LogIn());
    
    private void RegistrationButton() => StartCoroutine(Registration());

    private void OnDisable()
    {
        MenuEvents._On_Logged_In_Failed -= ShowIncorrectInput;
    }
    
}