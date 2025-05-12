using System;
using Game.Durak.Enums;
using Game.Durak.Repositories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Durak.UI
{
    public sealed class RoleIndicator
        : MonoBehaviour
    {
        [SerializeField] private RoleIndicatorRepository repository;
        
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text label;
        
        public void SetRole(EPlayerRole role)
        {
            var hasValue = repository.TryGetValue(role, out var result);
            if (hasValue)
            {
                iconImage.sprite = result.Icon;
                label.text = result.Label;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}