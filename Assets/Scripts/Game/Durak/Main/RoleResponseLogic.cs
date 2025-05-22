using Game.Durak.Network.Responses;
using Newtonsoft.Json;
using Zenject;

public class RoleResponseLogic : IResponse
{
    private TestPlayer _player; // bind ?

    private GameLogicMethods _gameLogicMethods;

    private DurakGameUI _durakGameUI;


    [Inject]
    private RoleResponseLogic(
        GameLogicMethods gameLogicMethods,
        DurakGameUI durakGameUI)
    {
        _gameLogicMethods = gameLogicMethods;
        _durakGameUI = durakGameUI;
    }
    
    
    public void Invoke(string response)
    {
        TurnResponse turnResponse = JsonConvert.DeserializeObject<TurnResponse>(response);

        _player.SetRole(turnResponse.Turn);
        _player.ShowRoleFrame();
            
        _gameLogicMethods.SetRoleFrames(turnResponse.Users);
            
        _durakGameUI.ChangeRoleImage(turnResponse.Turn);
            
        _durakGameUI.DisableButtons();
    }
}