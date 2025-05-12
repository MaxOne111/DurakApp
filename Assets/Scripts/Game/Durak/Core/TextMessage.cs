using System;
using UnityEngine;

namespace Game.Durak.Core
{
    [Serializable]
    public struct TextMessage
    {
        public TextMessage(Color backgroundColor, Color textColor, string text, int delay)
        {
            BackgroundColor = backgroundColor;
            TextColor = textColor;
            Text = text;
            Delay = delay;
        }

        [field: SerializeField]
        public Color BackgroundColor
        {
            get;
            private set;
        }

        [field: SerializeField]
        public Color TextColor
        {
            get;
            private set;
        }

        [field: SerializeField]
        public string Text
        {
            get;
            private set;
        }

        [field: SerializeField]
        public int Delay
        {
            get;
            private set;
        }
    }
}