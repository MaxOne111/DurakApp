using System;

public static class MenuEvents
{
    public static Action<string, int, Wallet[]> _On_Logged_In_Succesful;
    
    public static Action _On_Logged_In_Failed;

    public static Action _On_Profile_Saved;

    public static void LoggedInSuccessful(string username, int id, Wallet[] wallet) => _On_Logged_In_Succesful?.Invoke(username, id, wallet);

    public static void LoggedInFailed() => _On_Logged_In_Failed?.Invoke();

    public static void ProfileSaved() => _On_Profile_Saved?.Invoke();
}