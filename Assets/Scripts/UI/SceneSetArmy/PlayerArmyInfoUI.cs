using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;

namespace UI.SceneSetArmy
{
    [DisallowMultipleComponent]
    public class PlayerArmyInfoUI : MonoCache
    {
        private PriceChenger _priceChenger;
        private List<ShowerSliderValue> _showers = new();

        private int _totalNumber;
        private TMP_Text _text;

        public void Initialize(List<ShowerSliderValue> showers)
        {
            foreach (ShowerSliderValue shower in showers)
            {
                _showers.Add(shower);
            }

            _priceChenger = GetComponentInParent<PriceChenger>();
            _priceChenger.ChengeArmy += OnChangeArmy;
            _text=GetComponentInChildren<TMP_Text>();
            OnChangeArmy();
        }

        private void OnChangeArmy()
        {
            _totalNumber = 0;
            
            foreach (ShowerSliderValue shower in _showers)
            {
                int number=Convert.ToInt32(shower.Text.text);
                _totalNumber+= number;
            }

            ShowTotalNumber();
        }

        private void ShowTotalNumber() => _text.text = _totalNumber.ToString();

          private void OnDisable() => _priceChenger.ChengeArmy -= OnChangeArmy;
    }
}
