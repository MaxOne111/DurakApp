using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatar : MonoBehaviour
{
    [SerializeField] private Image _Image;
    
    private Sprite _Sprite;

    private Rect _Start_Rect;

    public Sprite Icon => _Image.sprite;

    private void Awake()
    {
        _Start_Rect = _Image.rectTransform.rect;
    }

    public void SetImage(Texture2D _texture)
    {
        float _width = _Start_Rect.width * ((float) _texture.width / _texture.height);
        
        _Sprite = Sprite.Create(_texture, new Rect(Vector2.zero, new Vector2(_texture.width,_texture.height)), new Vector2(0.5f,0.5f));

        _Image.sprite = _Sprite;

        _Image.rectTransform.sizeDelta = new Vector2(_width, _Start_Rect.height);
    }
}
