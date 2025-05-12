using System;
using System.Collections.Generic;
using Game.Durak.Enums;
using Newtonsoft.Json;

/*
Example:


/*
Example: 

{
    "id": 1015,
    "bet_amount": 0,
    "player_count": 2,
    "deck_count": 24,
    "speed": true,
    "mod_transfer": "transfer",
    "mod_can_transfer": "neighbours",
    "mod_deception": "deception",
    "mod_game": "classic",
    "is_private": false,
    "password": "",
    "trump": null,
    "players": [],
    "balance_type": "BALANCE",
    "status": "Waiting",
    "created_at": "2024-10-16T12:43:20.024506+00:00"
}
*/

namespace Game.Durak.SelectLobby
{
    public sealed class LobbyInfo
    {
        [JsonConstructor]
        public LobbyInfo
        (
            [JsonProperty(PropertyName = "id")] int id,
            [JsonProperty(PropertyName = "bet_amount")] int betAmount, 
            [JsonProperty(PropertyName = "player_count")] int playerCount, 
            [JsonProperty(PropertyName = "deck_count")] int deckCount, 
            [JsonProperty(PropertyName = "speed")] bool speed, 
            [JsonProperty(PropertyName = "mod_transfer")] ETransferMode modTransfer, 
            [JsonProperty(PropertyName = "mod_can_transfer")] EWhoTransferMode modCanTransfer, 
            [JsonProperty(PropertyName = "mod_deception")] EDeceptionMode modDeception,
            [JsonProperty(PropertyName = "mod_game")] EGameMode modGame,
            [JsonProperty(PropertyName = "is_private")] bool isPrivate,
            [JsonProperty(PropertyName = "password")] string password, 
            [JsonProperty(PropertyName = "trump")] CardInfo trump, 
            [JsonProperty(PropertyName = "players")] PlayerInfo[] players, 
            [JsonProperty(PropertyName = "balance_type")] EBalanceType balanceType,
            [JsonProperty(PropertyName = "status")] EGameStatus status,
            // TODO: Override DateTime deserialization
            [JsonProperty(PropertyName = "created_at")] DateTime createdAt
        )
        {
            Id = id;
            BetAmount = betAmount;
            PlayerCount = playerCount;
            DeckCount = deckCount;
            Speed = speed;
            ModTransfer = modTransfer;
            ModCanTransfer = modCanTransfer;
            ModDeception = modDeception;
            ModGame = modGame;
            IsPrivate = isPrivate;
            Password = password;
            Trump = trump;
            Players = players;
            BalanceType = balanceType;
            Status = status;
            CreatedAt = createdAt;
        }

        public int Id
        {
            get;
        }
        
        public int BetAmount
        {
            get;
        }

        public int PlayerCount
        {
            get;
        }

        public int DeckCount
        {
            get;
        }

        public bool Speed
        {
            get;
        }

        public ETransferMode ModTransfer
        {
            get;
        }

        public EWhoTransferMode ModCanTransfer
        {
            get;
        }

        public EDeceptionMode ModDeception
        {
            get;
        }

        public EGameMode ModGame
        {
            get;
        }

        public bool IsPrivate
        {
            get;
        }

        public string Password
        {
            get;
        }

        public CardInfo Trump
        {
            get;
        }

        public IReadOnlyList<PlayerInfo> Players
        {
            get;
        }

        public EBalanceType BalanceType
        {
            get;
        }

        public EGameStatus Status
        {
            get;
        }

        public DateTime CreatedAt
        {
            get;
        }
    }
}