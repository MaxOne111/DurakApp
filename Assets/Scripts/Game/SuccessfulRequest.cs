using System;

[Serializable]
public struct SuccessfulRequest
{
    public int id;
    public string username;
    public string avatar;
    public Wallet[] wallets;
    public string detail;
}
