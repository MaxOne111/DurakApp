using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Durak;
using Game.Durak.Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestCard : MonoBehaviour, IDraggable
{
    [SerializeField] private ECardRank rank;
    [SerializeField] private ECardSuit suit;

    [SerializeField] private Sprite cardBack;

    [SerializeField] private int strengthIndex;
    
    private Transform _transform;

    private RectTransform _rectTransform;

    private Image _image;

    private Vector3 _sleevePosition;

    private bool _canDrag;

    [SerializeField] private bool _isTrump;

    private readonly float _zRotationValue = -20;

    private event Action<TestCard, TestSlot, GameObject> _action;

    public CardInfo CardInfo => new (suit, rank);

    public int StrengthIndex => strengthIndex;

    public bool IsTrump => _isTrump;
    
    public void OnPointerDown(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_canDrag)
            return;
        
        _transform.position = new Vector3(
            _transform.position.x + eventData.delta.x,
            _transform.position.y + eventData.delta.y);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TestSlot slot = DetectSlot(eventData);
        GameObject table = DetectTable(eventData);
        
        _action?.Invoke(this, slot, table);
    }
    
    private void Awake()
    {
        _transform = transform;
        
        _rectTransform = GetComponent<RectTransform>();

        _image = GetComponent<Image>();
    }
    
    public void SetSleevePosition(Vector3 position) => _sleevePosition = position;

    public void SetDraggable(bool canDrag) => _canDrag = canDrag;

    public void ActionInitialize(Action<TestCard, TestSlot, GameObject> action) => _action += action;

    public void ReturnCardToBack() => _transform.position = _sleevePosition;

    public void RotateCard() => _rectTransform.DORotate(new Vector3(0, 0, _zRotationValue), 0.25f);
    
    public void FaceDown() => _image.sprite = cardBack;

    public void DoTrump() => _isTrump = true;

    private TestSlot DetectSlot(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        
        EventSystem.current.RaycastAll(eventData, results);
        for (int i = 0; i < results.Count; i++)
            if (results[i].gameObject.TryGetComponent(out TestSlot slot))
                return slot;
        
        return null;
    }
    
    private GameObject DetectTable(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        
        EventSystem.current.RaycastAll(eventData, results);
        for (int i = 0; i < results.Count; i++)
            if (results[i].gameObject.CompareTag("Table"))
                return results[i].gameObject;
        
        return null;
    }
    
    private void OnDestroy()
    {
        _action = null;
    }
}

public interface IDraggable : IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    
}