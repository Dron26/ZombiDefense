using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using Services.SaveLoad;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Locations
{
    public class LocationUIManager : MonoCache
    {
        [SerializeField] private GameObject _locationContainer;
        [SerializeField] private GameObject _selecterPanel;
        [SerializeField] private GameObject _enterLocationPanel;
        [SerializeField] private TextMeshProUGUI _locationInfo;
        [SerializeField] private TextMeshProUGUI _selectedLocationInfo;
        [SerializeField] private TextMeshProUGUI _selectedLocationHistory;
        [SerializeField] private Button _back;
        [SerializeField] private Button _backEnterLocationr;
        [SerializeField] private Button _enter;
        
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
            FillGlobalInfo();
            AddListener();
            
            _selecterPanel.SetActive(false);
            _enterLocationPanel.SetActive(false);
        }

        private void FillGlobalInfo()
        {
            int openLocation = 0;
            foreach (var location in _locationUIElements)
            {
                if (!location.IsLock)
                {
                    openLocation++;
                }
                
            }
            
                _locationInfo.text = $"Миссий: {openLocation}/{_locationUIElements.Count-1}\n" +
                                     $" $ : {_currencyHandler.GetCurrentMoney()}";
            
        }

        private void FillLocationInfo()
        {
            var currentLocation = _locationManager.GetLocationById(_selectedLocationId);
            if (currentLocation != null)
            {
                _selectedLocationInfo.text = $"Волн: {currentLocation.WaveCount}\n" +
                                             $"Количество зомби: {currentLocation.EnemyCount}\n" +
                                             $"Награда: {currentLocation.BaseReward}";
            }
            else
            {
                _selectedLocationHistory.text = "Информация о локации недоступна.";
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

                    LocationProgressData locationProgressData = _locationManager.GetLocationById(uiElement.Id); 
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
            _selectedLocationId=id;
            AllServices.Container.Single<ILocationHandler>().SetSelectedLocationId(_selectedLocationId);
            SwitchEnterPanelState(true);
            FillLocationInfo(); 
        }
        
        private void AddListener()
        {
            _back.onClick.AddListener(()=>SwitchPanelState(false));
            _backEnterLocationr.onClick.AddListener(()=>SwitchEnterPanelState(false));
            _enter.onClick.AddListener(Enter);
        }

        public void SwitchPanelState(bool isActive)
        {
            _selecterPanel.SetActive(isActive);
        } 

        public void SwitchEnterPanelState(bool isActive)
        {
            _enterLocationPanel.SetActive(isActive);
        }

        private void Enter()
        {
            OnSelectLocation?.Invoke();
        }
        
        private void RemoveListener()
        {
            foreach (var uiElement in _locationUIElements)
            {
                uiElement.OnClick -= HandleLocationClick;
            }
            
            _back.onClick.RemoveListener(()=>SwitchPanelState(false));
            _backEnterLocationr.onClick.RemoveListener(()=>SwitchEnterPanelState(false));
            _enter.onClick.RemoveListener(Enter);
        }

        protected override void OnDisabled()
        {
            RemoveListener();
        }
    }
}
