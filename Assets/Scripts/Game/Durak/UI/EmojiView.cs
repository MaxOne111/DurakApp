using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Durak.UI
{
    public sealed class EmojiView
        : MonoBehaviour
    {
        private const float FrameDuration = 0.025f;
        
        public event Action OnSelect = () => { };
        
        [SerializeField] private Image image;
        
        [SerializeField] private List<Sprite> frames = new List<Sprite>();

        private int _counter = 0;
        private float _delta = 0f;

        [field: SerializeField]
        public Button Button
        {
            get;
            private set;
        }
        
        public void SetAnimation(IEnumerable<Sprite> sprites)
        {
            frames.Clear();
            frames.AddRange(sprites);
        }

        private void Update()
        {
            _delta += Time.deltaTime;
            if (_delta >= FrameDuration)
            {
                _counter += (int) (_delta / FrameDuration);
                _delta = 0f;
            }

            image.sprite = frames[_counter % frames.Count];
        }

        private void OnEnable()
        {
            if (Button == null)
            {
                return;
            }
            
            Button.onClick.AddListener(OnSelect.Invoke);
        }

        private void OnDisable()
        {
            if (Button == null)
            {
                return;
            }
            
            Button.onClick.RemoveListener(OnSelect.Invoke);
        }
    }
}