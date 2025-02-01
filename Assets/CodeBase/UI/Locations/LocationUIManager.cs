using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using Services.SaveLoad;
using TMPro;
using UnityEngine;

namespace UI.Locations
{
    public class LocationUIManager : MonoCache
    {
        [SerializeField] private GameObject _locationContainer;
        [SerializeField] private TextMeshProUGUI _locationInfo;
        [SerializeField] private TextMeshProUGUI _cash;

        private List<LocationUIElement> _locationUIElements;
        private LocationManager _locationManager;
        public Action OnSelectLocation;
        private ISaveLoadService _saveLoadService;
        private ICurrencyHandler _currencyHandler;
        private int _completedLocationCount;

        public void Initialize(ISaveLoadService saveLoadService, LocationManager locationManager)
        {
            _locationManager = locationManager;
            _saveLoadService = saveLoadService;
            _currencyHandler = AllServices.Container.Single<ICurrencyHandler>();
            FillLocationElement();
            FillLocationInfo();
            FillCashInfo();
        }

        private void FillCashInfo()
        {
            _cash.text = _currencyHandler.GetCurrentMoney().ToString();
        }

        private void FillLocationInfo()
        {
            var currentLocation = _locationManager.GetLocationById(_locationManager.LocationData.Id);
            if (currentLocation != null)
            {
                _locationInfo.text = $"Волн: {currentLocation.WaveCount}\n" +
                                    $"Прочность зомби: {currentLocation.GetCurrentZombieHealth()}\n" +
                                    $"Награда: {currentLocation.GetCurrentReward()}";
            }
        }

        private void FillLocationElement()
        {
            _locationUIElements = new List<LocationUIElement>();

            foreach (Transform child in _locationContainer.transform)
            {
                var uiElement = child.GetComponent<LocationUIElement>();
                if (uiElement != null)
                {
                    _locationUIElements.Add(uiElement);

                    LocationData locationData = _locationManager.GetLocationById(uiElement.Id);
                    uiElement.Initialize(locationData.IsLocked, locationData.IsCompleted);
                    uiElement.OnClick += HandleLocationClick;

                    if (locationData.IsCompleted)
                    {
                        _completedLocationCount++;
                    }
                }
            }
        }

        private void HandleLocationClick(int id)
        {
            AllServices.Container.Single<ILocationHandler>().SetSelectedLocationId(id);
            OnSelectLocation?.Invoke();
            FillLocationInfo(); // Обновляем информацию при выборе локации
        }

        protected override void OnDisabled()
        {
            foreach (var uiElement in _locationUIElements)
            {
                uiElement.OnClick -= HandleLocationClick;
            }
        }
    }
}