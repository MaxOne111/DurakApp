using System;
using Game.Durak.Enums;
using Game.Durak.Repositories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Durak.UI
{
    public sealed class DeckView
        : MonoBehaviour
    {
        [SerializeField] private TrumpIndicatorRepository trumpIndicatorRepository;

        [SerializeField] private GameObject root;
        [SerializeField] private Image trumpIndicator;

        [SerializeField] private TMP_Text cardsCountText;

        public void SetTrumpSuit(ECardSuit suit)
        {
            var hasSprite = trumpIndicatorRepository.TryGetValue(suit, out var sprite);
            if (hasSprite)
            {
                trumpIndicator.sprite = sprite;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void SetCardsCount(int count)
        {
            var isZero = count == 0;
            cardsCountText.text = count.ToString();
            
            trumpIndicator.gameObject.SetActive(isZero);
            root.SetActive(!isZero);
        }
    }
}