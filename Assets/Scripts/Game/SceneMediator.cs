using Game.Durak;

public static class SceneMediator
{
    public static bool FromGame { get; set; }
    
    public static UserGameData UserGameData { get; private set; }
    
    public static IRoomMessage Room { get; set; }
    
    public static EPlayerStatus PlayerStatus { get; private set; }

    public enum EPlayerStatus
    {
        Watcher,
        Player
    }

    public static void SetPlayerInfo(string username, int id, string avatar, Wallet[] wallets)
    {
        UserGameData = new UserGameData()
        {
            username = username,
            user_id = id,
            avatar =  avatar,
            wallets = wallets,
        };
    }

    public static PlayerInfo GetPlayerInfo()
    {
        return new PlayerInfo(UserGameData.username, UserGameData.user_id, DurakHelper.GetWallet(UserGameData.wallets, WalletType.BonusBalance).balance);
    }

    public static void SetPlayerStatus(EPlayerStatus status) => PlayerStatus = status;
}
