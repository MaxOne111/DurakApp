using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Durak.UI
{
    public sealed class TextMessageView
        : MonoBehaviour
    {
        [field: SerializeField]
        public Button Button
        {
            get;
            private set;
        }

        [field: SerializeField]
        public TMP_Text Label
        {
            get;
            private set;
        }
    }
}