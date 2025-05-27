using Game.Durak.Enums;
using Game.Durak.Network.Responses;
using Newtonsoft.Json;
using UnityEngine;

public class ErrorResponseLogic : IResponse
{
    private TestPlayer _player; // Bind ?
    
    public void Invoke(string response)
    {
        Debug.Log(_player);
        return;
        
        ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response);

        switch (errorResponse.Error)
        {
            case ELogicError.WeakCard:
                break;
            case ELogicError.MissingTableCard:
                //TODO
                break;
            case ELogicError.MissingPlayerCard:
                //TODO
                break;
            case ELogicError.OpenSlot:
                //TODO
                break;
            case ELogicError.NotInit:
                //TODO
                break;
            case ELogicError.NotEnemy:
                //TODO
                break;
            case ELogicError.WaitInit:
                //TODO
                break;
            case ELogicError.SlotClosed:
                //TODO
                break;
                    
            case ELogicError.TournamentRoomNotFound:
                SceneMediator.Room = errorResponse.next_game;
                break;
        }

        if (_player)
            _player.ReturnSleeveToPlayer();
    }
}