using Game.Durak;
using UnityEngine;

public class TestSlot : MonoBehaviour
{
    [SerializeField] private SlotInfo _slotInfo;

    private int _childCount = 0;

    public SlotInfo SlotInfo => _slotInfo;

    public void Initialize(SlotInfo slotInfo)
    {
        _slotInfo = slotInfo;
    }

    [ContextMenu("Apply Size")]
    public void ApplySize()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var rect = (RectTransform) child;
            rect.sizeDelta = ((RectTransform) transform).sizeDelta;
        }
    }

    public bool CheckSlotForDefence()
    {
        if (_slotInfo == null)
            return false;

        return true;
    }
    
    public TestSlot CheckSlotForAttack()
    {
        if (_slotInfo != null)
            if (_slotInfo.init_card == null)
                return this;

        return null;
    }

    private void Update()
    {
        var childCount = transform.childCount;
        if (childCount != _childCount)
        {
            ApplySize();
            _childCount = childCount;
        }
    }
}