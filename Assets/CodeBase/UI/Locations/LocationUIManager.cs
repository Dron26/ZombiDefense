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
        private int _selectedLocationId;
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
            var currentLocation = _locationManager.GetLocationById(_selectedLocationId);
            if (currentLocation != null)
            {
                _locationInfo.text = $"Волн: {currentLocation.WaveCount}\n" +
                                     $"Прочность зомби: {currentLocation.BaseZombieHealth}\n" +
                                     $"Награда: {currentLocation.BaseReward}";
            }
            else
            {
                _locationInfo.text = "Информация о локации недоступна.";
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

                    LocationProgressData locationProgressData = _locationManager.GetLocationById(uiElement.Id); // Используем LocationProgressData
                    uiElement.Initialize(locationProgressData.IsLocked, locationProgressData.IsCompleted);
                    uiElement.OnClick += HandleLocationClick;

                    if (locationProgressData.IsCompleted)
                    {
                        _completedLocationCount++;
                    }
                }
            }
        }

        private void HandleLocationClick(int id)
        {
            var locationHandler = AllServices.Container.Single<ILocationHandler>();
            locationHandler.SetSelectedLocationId(id);
            _selectedLocationId=id;
            OnSelectLocation?.Invoke();
            FillLocationInfo(); 
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
