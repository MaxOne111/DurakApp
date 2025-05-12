using System;
using Mopsicus.TwinSlider;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game.Durak.UI
{
    public sealed class BetSliderText
        : MonoBehaviour
    {
        [SerializeField] private NumberFormatProvider numberFormatProvider;
        
        [SerializeField] private int minBet;
        [SerializeField] private int maxBet;
        
        [SerializeField] private string format;

        //[SerializeField] private TwinSlider slider;
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text text;
        
        private float _bet;

        public float Bet => _bet;

        private void Start()
        {
            _bet = minBet;
            
            text.text = $"Ставка: <color=#F6AB19>{_bet}</color>";
        }

        private void HandleSliderChange(float min, float max)
        {
            min = Mathf.Lerp(minBet, maxBet, min);
            max = Mathf.Lerp(minBet, maxBet, max);

            min = Mathf.Round(min);
            max = Mathf.Round(max);

            var minText = numberFormatProvider.Format(min);
            var maxText = numberFormatProvider.Format(max);
            
            text.text = string.Format(format, minText, maxText);
        }

        private void SliderValueChange()
        {
            _bet = minBet * slider.value;

            text.text = $"Ставка: <color=#F6AB19>{_bet}</color>";
        }
        
        private void OnEnable()
        {
            //slider.OnSliderChange += HandleSliderChange;
            slider.onValueChanged.AddListener(delegate(float arg0) {SliderValueChange();});
        }

        private void OnDisable()
        {
            //slider.OnSliderChange -= HandleSliderChange;
        }
    }
}