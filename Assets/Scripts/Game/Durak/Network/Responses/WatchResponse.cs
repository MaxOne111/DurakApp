using System;
using Game.Durak;
using Game.Durak.Enums;
using Game.Durak.Network.Responses;

[Serializable]
public sealed class WatchResponse : DurakResponseBase

{
    public PlayerCardInfo[] players;
    public CardInfo trump;
    public GameStartedResponse hand;
    public int deck;
    public int beats;
    public RoundInfo round;
    public TurnResponse role;
    public ETurn turn;
}