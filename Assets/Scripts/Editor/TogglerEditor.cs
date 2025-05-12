using UnityEditor;


[CustomEditor(typeof(Toggler))]
public class TogglerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Toggler _toggler = (Toggler)target;

        //_toggler.IsOn = EditorGUILayout.Toggle("IsOn", _toggler.IsOn);
        
        if (_toggler.IsOn)
            _toggler.SwitchOff();
        else
            _toggler.SwitchOn();
        
    }
}