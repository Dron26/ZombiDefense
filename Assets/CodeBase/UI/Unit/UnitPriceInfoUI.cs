using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using TMPro;
using UnityEngine;

namespace UI.Unit
{
    [DisallowMultipleComponent]
    
    public class UnitPriceInfoUI : MonoCache
    {
        private List<int> _prices=  new ();
        private int _price;
        private TMP_Text _text;
        private List<SliderUnit> _sliders = new ();
        public void Initialize(List<SliderUnit> sliders, List<int> prices)
        {
            _text=GetComponentInChildren<TMP_Text>();
         
            foreach (SliderUnit sliderUnit in sliders)
            {
                sliderUnit.SelectSlider += OnSelectSlider;
                sliderUnit.ChangeValue += OnChangevalue;
            }

            foreach (var price in prices)
            {
                _prices.Add(price);
            }
                
        }

        private void OnSelectSlider(int value,int index)
        {
             _price = _prices[index]*value;
             ShowPrice();
        }

        private void OnChangevalue(GameObject _)
        {
            _price = 0;
            ShowPrice();
        }
        
        private void ShowPrice()
        {
            _text.text = _price.ToString();
        }

        protected override void  OnDisabled()
        {
            foreach (SliderUnit sliderUnit in _sliders)
            {
                sliderUnit.SelectSlider -= OnSelectSlider;
                sliderUnit.ChangeValue -= OnChangevalue;
            }
        }
    }
}