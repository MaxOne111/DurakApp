using System;
using TMPro;
using UnityEngine;

public abstract class PanelUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    
    [SerializeField] private TextMeshProUGUI label;

    protected virtual void ShowLabel(string text) => label.text = text;

    public virtual void ShowPanel() => panel.SetActive(true);

    public virtual void HidePanel() => panel.SetActive(false);
    
}