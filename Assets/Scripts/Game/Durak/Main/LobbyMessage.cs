using System;

public interface IRoomMessage
{
    int ID { get;}
    decimal Bank { get; }
    int PlayerCount { get; }
}


[Serializable]
public struct LobbyMessage : IRoomMessage
{
    public int id;
    public decimal bank_amount;
    public int player_count;
    public int deck_count;
    public bool speed;
    public string mod_transfer;
    public string mod_can_transfer;
    public string mod_deception;
    public string mod_game;
    public bool is_private;
    public bool is_public;
    public bool is_test;
    public string password;
    public string balance_type;
    public bool elimination_mode;

    public int ID => id;
    public decimal Bank => bank_amount;
    public int PlayerCount => player_count;
}

[Serializable]
public struct TournamentMessage
{
    public int id;
    public decimal bet_amount;
    public int player_count;
    public int deck_count;
    public bool speed;
    public string mod_transfer;
    public string mod_can_transfer;
    public string mod_deception;
    public string mod_game;
    public bool is_private;
    public bool is_public;
    public bool is_test;
    public string password;
    public string balance_type;
    public TournamentRoomMessage[] rooms;

}

[Serializable]
public struct TournamentRoomMessage : IRoomMessage
{
    public int id;
    public decimal bank_amount;
    public decimal bet_amount;
    public decimal win_amount;
    public int player_count;
    public int deck_count;
    public bool speed;
    public string mod_transfer;
    public string mod_can_transfer;
    public string mod_deception;
    public string mod_game;
    public bool is_private;
    public bool is_public;
    public bool is_test;
    public string password;
    public string balance_type;
    public int order;
    public int tournament_id;
    public string status;

    public int ID => id;
    public decimal Bank => bank_amount;
    public int PlayerCount => player_count;
}