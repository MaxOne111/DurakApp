using System.IO;
using Mirror;
using UnityEngine;

public class GameFieldAdaption : MonoBehaviour
{
    [SerializeField] private Transform _Card_Prefab;

    private float _Card_Width;
    private float _Camera_Width;
    
    private Camera _Camera;
    
    private float _Available_Width;
    
    private Vector3 _Size;


   [SerializeField] private float _Offset;
    
    [SerializeField] private float _Start_Position;

    public float Offset => _Offset;
    public float StartPosition => _Start_Position;
    
    
    private void Awake()
    {
        _Camera = Camera.main;
        
        _Card_Width = _Card_Prefab.localScale.x;
        
        _Camera_Width = ScreenSize.CameraSize(_Camera).x;
        
        _Available_Width = _Camera_Width - _Card_Width;
      
        Debug.Log(Screen.width + "/" + Screen.height);
    }

    public void SetPostion(float _value)
    {
        _Start_Position = _value;
    }

    public void SetOffset(float _value)
    {
        _Offset = _value;
    }

    //[ClientRpc]
    public void RpcCameraInit()
    {
        _Camera = Camera.main;
        
        _Card_Width = _Card_Prefab.localScale.x;
        
        _Camera_Width = ScreenSize.CameraSize(_Camera).x;
        
        _Available_Width = _Camera_Width - _Card_Width;
    }
    
    public void CameraInit()
    {
        _Camera = Camera.main;
        
        _Card_Width = _Card_Prefab.localScale.x;
        
        _Camera_Width = ScreenSize.CameraSize(_Camera).x;
        
        _Available_Width = _Camera_Width - _Card_Width;
    }
    
    //[ClientRpc]
    public void CalculateStep(int _count)
    {
        _Offset = 0;
        //float _offset = 0;

        if (_Card_Width * _count <= _Available_Width)
        {
            _Offset = _Available_Width / _count;
            //return _offset;
        }

        float _step = _Card_Width - _Available_Width / _count;
        
        _Offset = _Card_Width - _step;

        //return _offset;
    }
    

    //[ClientRpc]
    public void CalculateStartPosition()
    {
        _Start_Position = 0;
        _Start_Position = -_Available_Width / 2 + Offset / 2;
        //float _start_Position = -_Available_Width / 2 + _offset / 2;
        
        //return _start_Position;
    }
    
}
