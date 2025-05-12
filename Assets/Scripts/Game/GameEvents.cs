using System;
using System.Collections.Generic;

public static class GameEvents
{
    public static event Action OnGameStarted;
    public static event Action OnGameFinished;
    public static event Action<decimal> OnPlayerWonCash;
    public static event Action<decimal> OnPlayerLostCash;
    public static event Action<decimal> OnCashChanged;
    public static event Action OnConnectionLost;
    public static event Action OnConnectionReconnected;
    public static event Action OnConnectionRestored;

    public static event Action OnLobbyCleared;
    

    public static void GameStarted() => OnGameStarted?.Invoke();
    
    public static void GameFinished() => OnGameFinished?.Invoke();

    public static void PlayerWonCash(decimal cash) => OnPlayerWonCash?.Invoke(cash);

    public static void PlayerLostCash(decimal cash) => OnPlayerLostCash?.Invoke(cash);
    public static void CashChanged(decimal cash) => OnCashChanged?.Invoke(cash);
    public static void ConnectionLost() => OnConnectionLost?.Invoke();
    public static void ConnectionReconnected() => OnConnectionReconnected?.Invoke();

    public static void LobbyCleared() => OnLobbyCleared?.Invoke();
    public static void ConnectionRestored() => OnConnectionRestored?.Invoke();
}