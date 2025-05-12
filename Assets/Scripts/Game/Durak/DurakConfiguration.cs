using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Durak
{
    
    // Пример:
    //{
    //    "bet_amount": 100,
    //    "player_count": 3,
    //    "deck_count": 36,
    //    "speed": true,
    //    "mod_transfer": "transfer",
    //    "mod_can_transfer": "neighbours",
    //    "mod_deception": "deception",
    //    "mod_game": "classic",
    //    "is_private": true,
    //    "password": "string",
    //    "balance_type": "BALANCE"
    //}
    
    public sealed class DurakConfiguration
    {
        [JsonConstructor]
        public DurakConfiguration
        (
            [JsonProperty(PropertyName = "id")] int id
        )
        {
            Id = id;
        }
        
        public DurakConfiguration
        (
            int bankAmount, 
            int playerCount, 
            int deckCount, 
            bool speed, 
            ETransferMode modTransfer, 
            EWhoTransferMode modCanTransfer, 
            EDeceptionMode modDeception,
            EGameMode modGame,
            bool isPrivate,
            string password,
            EBalanceType balanceType,
            bool forElimination
        )
        {
            BankAmount = bankAmount;
            PlayerCount = playerCount;
            DeckCount = deckCount;
            Speed = speed;
            ModTransfer = modTransfer;
            ModCanTransfer = modCanTransfer;
            ModDeception = modDeception;
            ModGame = modGame;
            IsPrivate = isPrivate;
            Password = password;
            BalanceType = balanceType;
            ForElimination = forElimination;
        }

        public int Id
        {
            get;
        }
        
        [JsonProperty(PropertyName = "bank_amount")]
        public int BankAmount
        {
            get;
        }

        [JsonProperty(PropertyName = "player_count")]
        public int PlayerCount
        {
            get;
        }

        [JsonProperty(PropertyName = "deck_count")]
        public int DeckCount
        {
            get;
        }

        [JsonProperty(PropertyName = "speed")]
        public bool Speed
        {
            get;
        }

        [JsonProperty(PropertyName = "mod_transfer")]
        public ETransferMode ModTransfer
        {
            get;
        }

        [JsonProperty(PropertyName = "mod_can_transfer")]
        public EWhoTransferMode ModCanTransfer
        {
            get;
        }

        [JsonProperty(PropertyName = "mod_deception")]
        public EDeceptionMode ModDeception
        {
            get;
        }

        [JsonProperty(PropertyName = "mod_game")]
        public EGameMode ModGame
        {
            get;
        }

        [JsonProperty(PropertyName = "is_private")]
        public bool IsPrivate
        {
            get;
        }
        
        [JsonProperty(PropertyName = "is_public")]
        public bool IsPublic
        {
            get;
        }
        
        [JsonProperty(PropertyName = "is_test")]
        public bool IsTest
        {
            get;
        }

        [JsonProperty(PropertyName = "password")]
        public string Password
        {
            get;
        }

        [JsonProperty(PropertyName = "balance_type")]
        public EBalanceType BalanceType
        {
            get;
        }
        
        [JsonProperty(PropertyName = "elimination_mode")]
        public bool ForElimination
        {
            get;
        }
    }
    
}

public sealed class DurakTournamentConfiguration
    {
        [JsonConstructor]
        public DurakTournamentConfiguration
        (
            [JsonProperty(PropertyName = "id")] int id
        )
        {
            Id = id;
        }
        
        public DurakTournamentConfiguration
        (
            int betAmount, 
            int playerCount, 
            int deckCount, 
            bool speed, 
            ETransferMode modTransfer, 
            EWhoTransferMode modCanTransfer, 
            EDeceptionMode modDeception,
            EGameMode modGame,
            bool isPrivate,
            bool isPublic,
            bool isTest,
            string password,
            EBalanceType balanceType 
        )
        {
            BetAmount = betAmount;
            PlayerCount = playerCount;
            DeckCount = deckCount;
            Speed = speed;
            ModTransfer = modTransfer;
            ModCanTransfer = modCanTransfer;
            ModDeception = modDeception;
            ModGame = modGame;
            IsPrivate = isPrivate;
            IsPublic = isPublic;
            IsTest = isTest;
            Password = password;
            BalanceType = balanceType;
        }

        public int Id
        {
            get;
        }
        
        [JsonProperty(PropertyName = "bet_amount")]
        public int BetAmount
        {
            get;
        }

        [JsonProperty(PropertyName = "player_count")]
        public int PlayerCount
        {
            get;
        }

        [JsonProperty(PropertyName = "deck_count")]
        public int DeckCount
        {
            get;
        }

        [JsonProperty(PropertyName = "speed")]
        public bool Speed
        {
            get;
        }

        [JsonProperty(PropertyName = "mod_transfer")]
        public ETransferMode ModTransfer
        {
            get;
        }

        [JsonProperty(PropertyName = "mod_can_transfer")]
        public EWhoTransferMode ModCanTransfer
        {
            get;
        }

        [JsonProperty(PropertyName = "mod_deception")]
        public EDeceptionMode ModDeception
        {
            get;
        }

        [JsonProperty(PropertyName = "mod_game")]
        public EGameMode ModGame
        {
            get;
        }

        [JsonProperty(PropertyName = "is_private")]
        public bool IsPrivate
        {
            get;
        }
        
        [JsonProperty(PropertyName = "is_public")]
        public bool IsPublic
        {
            get;
        }
        
        [JsonProperty(PropertyName = "is_test")]
        public bool IsTest
        {
            get;
        }

        [JsonProperty(PropertyName = "password")]
        public string Password
        {
            get;
        }

        [JsonProperty(PropertyName = "balance_type")]
        public EBalanceType BalanceType
        {
            get;
        }
    }