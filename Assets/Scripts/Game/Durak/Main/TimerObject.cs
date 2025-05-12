using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerObject : MonoBehaviour
{

    private Image _currentRoleFrame;
    
    public void StartTimer(Image currentRoleFrame ,float duration)
    {
        if (!currentRoleFrame)
            return;
            
        StartCoroutine(TimerTick(currentRoleFrame, duration));
    }

    private IEnumerator TimerTick(Image currentRoleFrame ,float duration)
    {
        _currentRoleFrame = currentRoleFrame;

        _currentRoleFrame.fillAmount = 1;
        
        float currentFillAmount = _currentRoleFrame.fillAmount;
        float elapsedTime = 0f;

        Debug.Log("Start timer");
            
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float process = elapsedTime / duration;

            if (!_currentRoleFrame)
            {
                Destroy(gameObject);
                yield break;
            }
            
            _currentRoleFrame.fillAmount = Mathf.Lerp(currentFillAmount, 0, process);

            yield return null;
        }

        _currentRoleFrame.fillAmount = 1;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_currentRoleFrame)
            _currentRoleFrame.fillAmount = 1;
    }
}
