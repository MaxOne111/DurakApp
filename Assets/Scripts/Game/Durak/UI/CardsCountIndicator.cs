using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Durak.UI
{
    public sealed class CardsCountIndicator
        : MonoBehaviour
    {
        [SerializeField] private CardFanConfiguration configuration;
        
        [SerializeField] private TMP_Text counterText;

        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private Transform cardsParent;

        [Header("Debug")]
        [SerializeField] private int debugCardsCount;

        private readonly IList<GameObject> _cards = new List<GameObject>();

        private bool _isActive = true;

        public void SetActive(bool isActive)
        {
            if (_cards.Any())
            {
                try
                {
                    if (_isActive == false)
                    {
                        foreach (var card in _cards)
                        {
                            Destroy(card.gameObject);
                        }
                    }
                }
                catch (MissingReferenceException exception)
                {
                    _cards.Clear();
                }
            }
            
            _isActive = isActive;
        }

        public void SetAmount(int amount)
        {
            Debug.Log($"SetAmount({amount})");
            
            if (!_isActive)
            {
                return;
            }
            
            if (_cards.Any())
            {
                try
                {
                    var foo = _cards[0].transform;
                }
                catch (MissingReferenceException exception)
                {
                    _cards.Clear();
                }
            }
            
            Debug.Log("Set amount");
            
            var realCardAmount = Mathf.Min(amount, configuration.MaxVisibleCards);
            
            if (_cards.Count < realCardAmount)
            {
                for (var i = _cards.Count; i < realCardAmount; i++)
                {
                    var card = Instantiate(cardPrefab, cardsParent);
                    _cards.Add(card);
                }
            }
            else if (_cards.Count > realCardAmount)
            {
                var incoming = new List<GameObject>(_cards);
                for (var i = incoming.Count - 1; i >= amount; i--)
                {
                    var last = incoming[i];
                    _cards.Remove(last);
                    
                    Destroy(last);
                }
            }

            if (!configuration.ReverseOrder)
            {
                for (var index = 0; index < realCardAmount; index++)
                {
                    CreateCard(index);
                }
            }
            else
            {
                for (var index = realCardAmount - 1; index >= 0; index--)
                {
                    CreateCard(index);
                }
            }

            //counterText.gameObject.SetActive(amount > _configuration.MaxVisibleCards);
            counterText.text = amount.ToString();

            void CreateCard(int index)
            {
                var degree = configuration.Direction - configuration.Width / 2 + (float) index / (realCardAmount - 1) * configuration.Width;
                var radians = degree * Mathf.Deg2Rad;
                
                var delta = (Vector3) new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * (configuration.Radius / 2);
                var position = transform.position + (Vector3) configuration.Center + delta;
                var displacement = new Vector2((Mathf.PerlinNoise(position.x, position.y) - 0.5f) * configuration.PositionNoiseStrength.x, (Mathf.PerlinNoise(position.y, position.x) - 0.5f) * configuration.PositionNoiseStrength.y);
                position += (Vector3) displacement;
                
                var rotationNoise = (Mathf.PerlinNoise(position.x, position.y) - 0.5f) * configuration.RotationNoiseStrength;
                var polarRotation = degree - 90f + rotationNoise;

                Quaternion rotation;
                if (amount > 1)
                {
                    rotation = Quaternion.Euler(0f, 0f, polarRotation);
                }
                else
                {
                    rotation = Quaternion.identity;
                    position = transform.position + (Vector3)configuration.Center + Vector3.down * configuration.Radius / 2;
                }

                _cards[index].transform.position = position;
                _cards[index].transform.rotation = rotation;
            }
        }

        [ContextMenu("Set Debug Count")]
        private void SetDebugCardsCount()
        {
            SetAmount(debugCardsCount);
        }

        public Transform GetRandomCard()
        {
            if (cardsParent.childCount == 0)
                return null;

            int index = Random.Range(0, cardsParent.childCount);

            return cardsParent.GetChild(index);
        }
    }

    [Serializable]
    public struct CardFanConfiguration
    {
        [field: SerializeField]
        public float Direction
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public float Width
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public float Radius
        {
            get;
            private set;
        }

        [field: SerializeField]
        public int MaxVisibleCards
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public Vector2 LabelPosition
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public Vector2 Center
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public Vector2 PositionNoiseStrength
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public float RotationNoiseStrength
        {
            get;
            private set;
        }

        [field: SerializeField]
        public bool ReverseOrder
        {
            get;
            private set;
        }
    }
}