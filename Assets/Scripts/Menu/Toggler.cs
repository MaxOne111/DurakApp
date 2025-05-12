using UnityEngine;
using UnityEngine.UI;

public class Toggler : MonoBehaviour
{
    private bool _isOn;

    [SerializeField] private Sprite _on;
    [SerializeField] private Sprite _off;

    [SerializeField] private Image _image;

    public bool IsOn { get; set; }


    public void SwitchOn()
    {
        IsOn = true;
        _isOn = true;
        _image.sprite = _on;
    }

    public void SwitchOff()
    {
        IsOn = false;
        _isOn = false;
        _image.sprite = _off;
    }
}