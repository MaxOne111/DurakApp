public static class ScenePlayerSlotNumber
{
    private static int _playerSlot;

    public static int PlayerSlot => _playerSlot;


    public static void SetPlayerSlot(int index) => _playerSlot = index;

    public static void ResetPlayerSlot() => _playerSlot = 0;
}