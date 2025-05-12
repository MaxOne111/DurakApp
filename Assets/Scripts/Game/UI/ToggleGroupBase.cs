using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game.UI
{
    public abstract class ToggleGroupBase<T>
        : MonoBehaviour
    {
        [SerializeField] private List<ToggleValuePair> toggles;

        private IDisposable _token;

        public T SelectedValue
        {
            get;
            private set;
        }

        protected virtual void OnSelect(T value)
        {
            // overload it!
        }

        private void OnEnable()
        {
            var token = new CompositeDisposable();
            
            foreach (var toggle in toggles)
            {
                var toggleToken = toggle
                    .Toggle
                    .onValueChanged
                    .AddDisposableListener(HandleSelect);
                
                token.Add(toggleToken);
                
                void HandleSelect(bool isSelected)
                {
                    foreach (var item in toggles)
                    {
                        item.Toggle.SetIsOnWithoutNotify(false);
                        SelectedValue = toggle.Value;
                    }
                }
            }

            _token = token;
        }

        private void OnDisable()
        {
            _token.Dispose();
        }

        [Serializable]
        private struct ToggleValuePair
        {
            [field: SerializeField]
            public Toggle Toggle
            {
                get;
                private set;
            }

            [field: SerializeField]
            public T Value
            {
                get;
                private set;
            }
        }
    }
}