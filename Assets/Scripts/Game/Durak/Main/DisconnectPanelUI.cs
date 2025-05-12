using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisconnectPanelUI : PanelUI
{
    [SerializeField] private Button tryConnect;
    [SerializeField] private Button quit;

    private void OnEnable()
    {
        GameEvents.OnConnectionReconnected += HidePanel;
        GameEvents.OnConnectionRestored += HidePanel;
        GameEvents.OnConnectionLost += ShowPanel;
    }
    
    public override void ShowPanel()
    {
        base.ShowPanel();
        ShowLabel("Connection lost, you can try again ");
    }

    public void InitializeTryConnect(UnityAction action)
    {
        tryConnect.onClick.AddListener(action);
    }
    
    public void InitializeQuit(UnityAction action)
    {
        quit.onClick.AddListener(action);
    }
    
    private void OnDisable()
    {
        GameEvents.OnConnectionReconnected -= HidePanel;
        GameEvents.OnConnectionRestored -= HidePanel;
        GameEvents.OnConnectionLost -= ShowPanel;
    }
}