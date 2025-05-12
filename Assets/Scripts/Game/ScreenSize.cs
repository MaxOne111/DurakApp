using UnityEngine;

public static class ScreenSize
{
    public static Vector3 Size()
    {
        Vector3 _size = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        return _size;
    }
    
    public static Vector3 Size(float _offset_X, float _offset_Y)
    {
        Vector3 _size = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + _offset_X, Screen.height + _offset_Y));

        return _size;
    }

    public static Vector3 CameraSize(Camera _camera)
    {
        float _Height = _camera.orthographicSize * 2;
        float _Width = _camera.aspect * _Height;

        Vector3 _size = new Vector3(_Width, _Height);

        return _size;

    }
    
    public static Vector3 CameraBorders(Camera _camera)
    {
        float _Border_Y = _camera.orthographicSize;
        float _Border_X = _camera.aspect * _Border_Y;

        Vector3 _borders = new Vector3(_Border_X, _Border_Y);

        return _borders;

    }
    
    public static Vector3 CameraBorders(Camera _camera, float _offset_X, float _offset_Y)
    {
        float _Border_Y = _camera.orthographicSize;
        float _Border_X = _camera.aspect * _Border_Y;

        Vector3 _borders = new Vector3(_Border_X + _offset_X, _Border_Y + _offset_Y);

        return _borders;

    }
}