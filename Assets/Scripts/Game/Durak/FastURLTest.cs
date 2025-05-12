using System;
using Game.Durak;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FastURLTest : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown urlDropdownField;

    private void Start()
    {
        int value = PlayerPrefs.GetInt("savedUrl", urlDropdownField.value);
        
        UrlMenu(value);

        urlDropdownField.value = value;
    }

    public void UrlMenu(int value)
    {
        switch (value)
        {
            case 0:
                DurakHelper.SetGameUrl("durak.sino0on.ru");
                DurakHelper.SetUserUrl("userservice.sino0on.ru");
                PlayerPrefs.SetInt("savedUrl", 0);
                break;
            case 1:
                DurakHelper.SetGameUrl("hooly.space");
                DurakHelper.SetUserUrl("userservice.hooly.space");
                PlayerPrefs.SetInt("savedUrl", 1);
                break;
            case 2:
                DurakHelper.SetGameUrl("duraktest.sino0on.ru");
                DurakHelper.SetUserUrl("userservice.sino0on.ru");
                PlayerPrefs.SetInt("savedUrl", 2);
                break;

        }
    }
}