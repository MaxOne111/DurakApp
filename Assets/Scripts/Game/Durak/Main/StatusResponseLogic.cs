using Game.Durak.Enums;
using Game.Durak.Network.Responses;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class StatusResponseLogic : IResponse
{
    private DurakGameUI _durakGameUI;
    
    private bool _isReady; // Temp


    [Inject]
    private StatusResponseLogic(DurakGameUI durakGameUI)
    {
        _durakGameUI = durakGameUI;
    }
    
    
    public void Invoke(string response)
    {
        StatusResponse statusResponse = JsonConvert.DeserializeObject<StatusResponse>(response);

        switch (statusResponse.Status)
        {
            case EGameStatus.In_Game:
                break;
                
            case EGameStatus.GameInReady:

                if (!_isReady)
                    _durakGameUI.SwitchButton(_durakGameUI.Ready);
                    
                break;
        }
    }
}