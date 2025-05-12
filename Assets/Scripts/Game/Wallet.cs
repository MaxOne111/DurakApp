using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[Serializable]
public struct Wallet
{
    public int balance;
    public WalletType wallet_type;
}

[JsonConverter(typeof(StringEnumConverter))]
[DataContract]
public enum WalletType
{
    [EnumMember(Value = "BONUS_BALANCE")]
    BonusBalance,
        
    [EnumMember(Value = "BALANCE")]
    Balance,
}