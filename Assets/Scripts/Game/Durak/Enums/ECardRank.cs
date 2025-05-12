using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Durak.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [DataContract]
    public enum ECardRank
        : byte
    {
        [EnumMember(Value = "A")]
        Ace = 1,
        
        [EnumMember(Value = "2")]
        Two,
        
        [EnumMember(Value = "3")]
        Three,
        
        [EnumMember(Value = "4")]
        Four,
        
        [EnumMember(Value = "5")]
        Five,
        
        [EnumMember(Value = "6")]
        Six,
        
        [EnumMember(Value = "7")]
        Seven,
        
        [EnumMember(Value = "8")]
        Eight,
        
        [EnumMember(Value = "9")]
        Nine,
        
        [EnumMember(Value = "10")]
        Ten,
        
        [EnumMember(Value = "J")]
        Jack,
        
        [EnumMember(Value = "Q")]
        Queen,
        
        [EnumMember(Value = "K")]
        King
    }
    
    //public sealed class CardSuitConverter
    //    : JsonConverter<ECardRank>
    //{
    //    public override void WriteJson(JsonWriter writer, ECardRank value, JsonSerializer serializer)
    //    {
    //        if ((byte) value > 10)
    //        {
    //            writer.WriteRawValue(value.ToString().ToLower());
    //        }
    //        else
    //        {
    //            writer.WriteRawValue(((byte) value).ToString());
    //        }
    //    }
    //
    //    public override ECardRank ReadJson
    //        (JsonReader reader, Type objectType, ECardRank existingValue, bool hasExistingValue, JsonSerializer serializer)
    //    {
    //        if (reader.Value == null)
    //        {
    //            throw new ArgumentException();
    //        }
    //        
    //        var isInteger = byte.TryParse(reader.Value.ToString(), out var integer);
    //        if (isInteger)
    //        {
    //            return (ECardRank) integer;
    //        }
    //
    //        var canParse = Enum.TryParse<ECardRank>(reader.Value.ToString(), true, out var result);
    //        if (!canParse)
    //        {
    //            throw new ArgumentException();
    //        }
    //
    //        return result;
    //    }
    //}
}