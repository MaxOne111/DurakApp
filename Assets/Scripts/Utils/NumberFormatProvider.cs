using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(menuName = "Utils/NumberFormatProvider", fileName = "NumberFormatProvider")]
    public sealed class NumberFormatProvider
        : ScriptableObject
    {
        [SerializeField] private List<Capacity> capacities;

        public string Format(float value)
        {
            foreach (var capacity in capacities)
            {
                if (!capacity.IsEnabled)
                {
                    continue;
                }

                if (value < capacity.Min || value > capacity.Max)
                {
                    continue;
                }

                value /= capacity.Base;
                
                value = (float) Math.Round(value, capacity.DigitsAfterPoint);

                var result = string.Format(capacity.Format, value);
                return result;
            }

            return value.ToString(CultureInfo.InvariantCulture);
        }

        [Serializable]
        private struct Capacity
        {
            [field: SerializeField]
            public bool IsEnabled
            {
                get;
                private set;
            }

            [field: SerializeField]
            public float Min
            {
                get;
                private set;
            }

            [field: SerializeField]
            public float Max
            {
                get;
                private set;
            }

            [field: SerializeField]
            public float Base
            {
                get;
                private set;
            }

            [field: SerializeField]
            public int DigitsAfterPoint
            {
                get;
                private set;
            }

            [field: SerializeField]
            public string Format
            {
                get;
                private set;
            }
        }
    }
}