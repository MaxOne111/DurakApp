using System;
using TMPro;
using UnityEngine;

namespace Game.Durak.UI
{
    public sealed class FastGameScreen
        : MonoBehaviour
    {
        [Header("Timer")]
        [SerializeField] private string timerFormat = "{0:mm\\:ss}";
        [SerializeField] private TMP_Text timer;

        private float _currentTime;
        
        private void OnDisable()
        {
            _currentTime = 0f;
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;
            
            var value = TimeSpan.FromSeconds(_currentTime);
            timer.text = value.ToString(timerFormat);
        }
    }
}