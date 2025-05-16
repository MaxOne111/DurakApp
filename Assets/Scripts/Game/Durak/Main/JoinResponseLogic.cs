using Game.Durak.Network.Responses;
using Newtonsoft.Json;
using Zenject;

namespace Game.Durak.Main
{
    class JoinResponseLogic : IResponse
    {
        private DurakGameUI _durakGameUI;

        private GameLogicMethods _gameLogicMethods;


        [Inject]
        private JoinResponseLogic(
            DurakGameUI durakGameUI,
            GameLogicMethods gameLogicMethods)
        {
            _durakGameUI = durakGameUI;
            _gameLogicMethods = gameLogicMethods;
        }
    
    
        public void Invoke(string response)
        {
            JoinResponse joinResponse = JsonConvert.DeserializeObject<JoinResponse>(response);
            
            if (joinResponse.players == null)
                return;

            _durakGameUI.ShowLobbyID(SceneMediator.Room.ID);
            _durakGameUI.ShowLobbyBet(SceneMediator.Room.Bank);
            
            _gameLogicMethods.CreatePlayers(joinResponse.players.Length);
            
            _gameLogicMethods.InitializePlayers(joinResponse.players);
        }
    }
}