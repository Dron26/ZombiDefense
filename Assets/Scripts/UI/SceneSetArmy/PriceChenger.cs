using System;
using System.Collections.Generic;
using HumanoidsUI.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using TMPro;
using UI.Empty;
using UI.SceneSetArmy.Slots;
using UI.Unit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.SceneSetArmy
{
    [DisallowMultipleComponent]
    public class PriceChenger : MonoCache
    {
        private SaveLoad _saveLoad;

        public UnityAction ChengeArmy;
        private int _money => _saveLoad.ReadAmountMoney();

        private int _tempMoney;
        private TMP_Text _textInfoUI;
        private Slider _slider;
        private TotalInfoUI _totalInfoUI;
        private int _characterPrice;
        private PlayerSlotsInitializer _slotsInitializer;
        private List<ParametrSlot> _slots = new();
        private List<HumanoidUI> _humanoids = new();
        private List<int> _prices = new();
        private List<SliderUnit> _sliders = new();
        private List<ShowerSliderValue> _showers = new();
        private List<int> _previousValues = new();
        private int _tempValue;
        private int _price;
        private int indexSlider;
        private int _previousValue;
        private bool _isBuy = false;


        public void Initialize(PlayerSlotsInitializer slotsInitializer, SaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
            _tempMoney = _money;
            _totalInfoUI = GetComponentInChildren<TotalInfoUI>();
            _textInfoUI = _totalInfoUI.GetComponentInChildren<TMP_Text>();
            _textInfoUI.text = Mathf.Round(_tempMoney).ToString();
            _slotsInitializer = slotsInitializer;

            SetParametrs();

            PlayerArmyInfoUI _playerArmyInfoUI = GetComponentInChildren<PlayerArmyInfoUI>();
            _playerArmyInfoUI.Initialize(_showers);
            UnitPriceInfoUI unitPriceInfoUIUI = GetComponentInChildren<UnitPriceInfoUI>();
            unitPriceInfoUIUI.Initialize(_sliders, _prices);
        }

        public int ReadAmountMoney =>
            _tempMoney;

        public void SetValidQuantity(Slider slider)
        {
            _slider = slider;
            indexSlider = _slider.GetComponent<SliderUnit>().Index;
            int sliderValue = Convert.ToInt32(Math.Round(_slider.value));

            _characterPrice = _prices[indexSlider];
            _previousValue = _previousValues[indexSlider];

            if (sliderValue > _previousValue)
                SpendMoney(sliderValue);
            else if (sliderValue < _previousValue) AddMoney(sliderValue);

            SetSliderValue();
            ChengeArmy?.Invoke();
        }

        private int GetMaxCanBuy()
        {
            int maxNumberOfUnit = _tempMoney / _characterPrice;
            return maxNumberOfUnit;
        }

        private void SetSliderValue()
        {
            for (int i = 0; i < _sliders.Count; i++)
            {
                _characterPrice = _humanoids[i].GetPrice();

                _sliders[i].SetMaxVolume(_money / _characterPrice + 1);
            }
        }

        private void SpendMoney(int sliderValue)
        {
            _tempValue = sliderValue - _previousValue;
            _price = _tempValue * _characterPrice;

            if (_tempMoney >= _price)
            {
                _tempMoney -= Math.Clamp(_price, 0, _tempMoney);
                _textInfoUI.text = Mathf.Round(_tempMoney).ToString();
                _previousValues[indexSlider] = sliderValue;
                _slots[indexSlider].AddQuantity(sliderValue);
            }
            else
            {
                _slider.value = GetMaxCanBuy() + _previousValues[indexSlider];
                SetValidQuantity(_slider);
            }
        }

        private void AddMoney(int sliderValue)
        {
            _tempValue = _previousValue - sliderValue;
            _price = _tempValue * _characterPrice;
            _tempMoney += Mathf.Clamp(_price, 0, _money);
            _textInfoUI.text = Mathf.Round(_tempMoney).ToString();
            _previousValues[indexSlider] = sliderValue;
            _slots[indexSlider].AddQuantity(sliderValue);
        }

        private void SetParametrs()
        {
            int heroeslevel = 8;

            foreach (GameObject panel in _slotsInitializer.GetPanels())
            {
                ParametrSlot slot = panel.GetComponentInChildren<ParametrSlot>();
                HumanoidUI character = slot.GetComponentInChildren<HumanoidUI>();
                SliderUnit slider = panel.GetComponentInChildren<SliderUnit>();
                ShowerSliderValue shower = panel.GetComponentInChildren<ShowerSliderValue>();


                if (character.GetLevel() < heroeslevel)
                {
                    _sliders.Add(slider);
                    _humanoids.Add(character);
                    _prices.Add(character.GetPrice());
                    slider.ChangeValue += OnPointerUp;
                    _previousValues.Add(1);
                }
                else
                {
                    slot.AddQuantity(1);
                    slider.gameObject.SetActive(false);
                    panel.GetComponentInChildren<ShowerSliderValue>().SetHeroesValue(1);
                }

                _showers.Add(shower);
                _slots.Add(slot);
            }

            SetSliderValue();
        }

        private void OnPointerUp(GameObject slider) => SetValidQuantity(slider.GetComponent<Slider>());

        public void SaveParametrsArmy()
        {
            List<int> levels = new List<int>();
            List<int> amount = new List<int>();

            foreach (var slot in _slots)
            {
                int levelHumanoid = slot.GetComponentInChildren<HumanoidUI>().GetLevel();
                int amountHumanoids = slot.NumberOfUnit + 1;

                print(levelHumanoid);
                print(amountHumanoids);

                SpendMoney(_money - _tempMoney);


                levels.Add(levelHumanoid);
                amount.Add(amountHumanoids);
            }

            _saveLoad.SaveHumanoidAndCount(levels, amount);
            _saveLoad.SetStartBattle();
        }

        public void OnClickReset()
        {
            for (int i = 0; i < _sliders.Count; i++)
            {
                _sliders[i].SetMinVolume();
                _previousValues[i] = 1;
                _tempMoney = _money;
                _textInfoUI.text = Mathf.Round(_tempMoney).ToString();
            }

            ChengeArmy?.Invoke();
        }

        protected override void OnDisabled()
        {
            if (!_saveLoad.GetStartBattle())
            {
                _saveLoad.ApplyMoney(GetPrice());
            }
        }

        public int GetPrice()
        {
            int price = 0;

            foreach (var slot in _slots)
            {
                HumanoidUI humanoid = slot.GetComponentInChildren<HumanoidUI>();
                if (humanoid != null)
                {
                    price += humanoid.GetPrice();
                }
            }

            return price;
        }
    }
}