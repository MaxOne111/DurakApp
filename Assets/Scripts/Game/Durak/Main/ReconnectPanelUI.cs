using System;

public class ReconnectPanelUI : PanelUI
{
    private void OnEnable()
    {
        GameEvents.OnConnectionReconnected += ShowPanel;
        GameEvents.OnConnectionRestored += HidePanel;
        GameEvents.OnConnectionLost += HidePanel;
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        ShowLabel("You are reconnected, wait");
    }
    
    private void OnDisable()
    {
        GameEvents.OnConnectionReconnected -= ShowPanel;
        GameEvents.OnConnectionRestored -= HidePanel;
        GameEvents.OnConnectionLost -= HidePanel;
    }

}