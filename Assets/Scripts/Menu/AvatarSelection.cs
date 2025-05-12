using System.IO;
using UnityEngine;
using UnityEngine.UI;
using File = System.IO.File;

public class AvatarSelection : MonoBehaviour
{
    [SerializeField] private PlayerAvatar _Player_Avatar;
    
    [SerializeField] private Button _Player_Avatar_Button;

    private string _Path;
    
    private DirectoryInfo _Avatar_Directory;
    
    private static int _Counter;
    
    private void Awake() => _Player_Avatar_Button.onClick.AddListener(SetPlayerAvatar);

    private void Start()
    {
        CreateDirectory();
        
        CheckAvatar();
    }

    private void SetPlayerAvatar()
    {
        NativeGallery.GetImageFromGallery( ( _path ) =>
        {
            if (_path == null)
                return;
            
            Texture2D _texture = NativeGallery.LoadImageAtPath( _path, -1, false);

            if( _texture == null )
            {
                Debug.Log( "Couldn't load texture from " + _path );
                return;
            }

            FileInfo[] _icons = _Avatar_Directory.GetFiles();

            if (_icons.Length > 0)
                for (int i = 0; i < _icons.Length; i++)
                    _icons[i].Delete();

            byte[] _saved_Texture = _texture.EncodeToJPG(100);

            File.WriteAllBytes(_Avatar_Directory.FullName + "/Avatars", _saved_Texture);
            
            LoadAvatar();
            
        } );

    }

    private void CheckAvatar()
    {
        if (File.Exists(_Avatar_Directory.FullName + "/Avatars"))
            LoadAvatar();
    }

    private void LoadAvatar()
    {
        byte[] _byte_Icon = File.ReadAllBytes(_Avatar_Directory.FullName + "/Avatars");

        Texture2D _icon = new Texture2D(1, 1);

        _icon.LoadImage(_byte_Icon);

        SetAvatar(_icon);
    }

    private void SetAvatar(Texture2D _icon) => _Player_Avatar.SetImage(_icon);

    private void CreateDirectory()
    {
        string _path = Path.Combine(Application.persistentDataPath, "Avatars");
        _Avatar_Directory = Directory.CreateDirectory(_path);
    }
    
}