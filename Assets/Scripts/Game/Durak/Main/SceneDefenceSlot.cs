using UnityEngine;

public static class SceneDefenceSlot
{
    private static Transform _defenceSlotTransform;
    
    public static Transform DefenceSlot => _defenceSlotTransform;
    
    
    public static void SetDefenceSlot(Transform slot) => _defenceSlotTransform = slot;

    public static void ResetDefenceSlot() => _defenceSlotTransform = null;
    
    public static void FillDefenseSlot(Transform child)
    {
        child.SetParent(_defenceSlotTransform);
    }
    
    public static void ReleaseDefenseSlot()
    {
        foreach (Transform child in _defenceSlotTransform)
            child.SetParent(null);
    }
}