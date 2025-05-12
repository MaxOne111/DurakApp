using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class SliderLabel
        : MonoBehaviour
    {
        [SerializeField] private string format;
        [SerializeField] private bool isRounded;
        
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text label;

        private void SetValue(float value)
        {
            if (isRounded)
            {
                value = Mathf.Round(value);
            }
            
            label.text = string.Format(format, value);
        }

        private void OnEnable()
        {
            slider.onValueChanged.AddListener(SetValue);
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(SetValue);
        }

        private void OnValidate()
        {
            if (slider == null || label == null)
            {
                return;
            }
            
            SetValue(slider.value);
        }
    }
}
