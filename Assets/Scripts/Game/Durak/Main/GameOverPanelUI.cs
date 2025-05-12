using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelUI : PanelUI
{
    [SerializeField] private Image cashTextBackground;
        
    [SerializeField] private Color victoryBackgroundColor;
    [SerializeField] private Color defeatBackgroundColor;

    private void OnEnable()
    {
        GameEvents.OnGameStarted += HidePanel;
        GameEvents.OnGameFinished += ShowPanel;

        GameEvents.OnPlayerWonCash += ShowVictory;
        GameEvents.OnPlayerLostCash += ShowDefeat;
    }

    public void ShowVictory(decimal value)
    {
        ShowLabel($"+{value}");
        cashTextBackground.color = victoryBackgroundColor;
    }

    public void ShowDefeat(decimal value)
    {
        ShowLabel($"-{value}");
        cashTextBackground.color = defeatBackgroundColor;
    }
    
    private void OnDisable()
    {
        GameEvents.OnGameStarted -= HidePanel;
        GameEvents.OnGameFinished -= ShowPanel;
        
        GameEvents.OnPlayerWonCash -= ShowVictory;
        GameEvents.OnPlayerLostCash -= ShowDefeat;
    }
    
}