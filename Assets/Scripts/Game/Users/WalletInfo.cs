using Game.Durak.Enums;
using Newtonsoft.Json;

namespace Game.Users
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class WalletInfo
    {
        public WalletInfo
        (
            [JsonProperty(PropertyName = "balance")] 
            int balance,
            
            [JsonProperty(PropertyName = "wallet_type")] 
            EBalanceType balanceType
        )
        {
            Balance = balance;
            BalanceType = balanceType;
        }

        public int Balance
        {
            get;
        }

        public EBalanceType BalanceType
        {
            get;
        }
    }
}