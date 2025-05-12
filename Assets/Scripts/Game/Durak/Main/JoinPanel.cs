public class JoinPanel : PanelUI
{
    public override void ShowPanel()
    {
        base.ShowPanel();

        var label = $"Вход: {SceneMediator.Room.Bank}";
        
        ShowLabel(label);
    }
}