using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public sealed class TextWaitingMarker
        : MonoBehaviour
    {
        private const float CycleDuration = 0.25f;

        private static readonly IReadOnlyList<string> Values = new[]
        {
            "Loading",
            "Loading.",
            "Loading..",
            "Loading...",
            "Loading",
        };

        [SerializeField] private TMP_Text text;

        private float _timer;
        private int _current;

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > CycleDuration)
            {
                _timer = 0f;
                _current++;

                text.text = Values[_current % Values.Count];
            }
        }
    }
}