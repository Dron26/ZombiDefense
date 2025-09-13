using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using Services.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI.Locations
{
    public class LocationUIManager : MonoCache
    {
        [SerializeField] private GameObject _locationContainer;
        [SerializeField] private GameObject _selecterPanel;
        [SerializeField] private GameObject _enterLocationPanel;
        [SerializeField] private TextMeshProUGUI _locationInfo;
        
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _context;
        [SerializeField] private TextMeshProUGUI _location;
        [SerializeField] private TextMeshProUGUI _objective;
        [SerializeField] private TextMeshProUGUI _tip;
        
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
        private LocationProgressData  _currentLocation;
        protected override void OnEnabled()
        {
            YG2.onSwitchLang += OnSwitchLanguage;
        }

        
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
            
                _locationInfo.text = $" {openLocation}/{_locationUIElements.Count-1}\n" +
                                     $" $ {_currencyHandler.GetCurrentMoney()}";
            
        }

        private void FillLocationInfo()
        {
            _currentLocation= _locationManager.GetLocationById(_selectedLocationId);
           
            
            if (_currentLocation != null)
            {
                
                UpdateUI(YG2.envir.language);
                // _selectedLocationInfo.text = $"Волн: {_currentLocation.WaveCount}\n" +
                //                              $"Количество зомби: {_currentLocation.EnemyCount}\n";
                 
                //$"Награда: {currentLocation.BaseReward}";
            }
            
        }

        private void UpdateUI(string lang)
        {
             _title.text = GetLocalizedText(_currentLocation.TitleRu, _currentLocation.TitleEn, _currentLocation.TitleTr, lang);
            _context.text = GetLocalizedText(_currentLocation.ContextRu, _currentLocation.ContextEn, _currentLocation.ContextTr, lang);
            _objective.text = GetLocalizedText(_currentLocation.ObjectiveRu, _currentLocation.ObjectiveEn, _currentLocation.ObjectiveTr, lang);
            _location.text = GetLocalizedText(_currentLocation.LocationRu, _currentLocation.LocationEn, _currentLocation.LocationTr, lang);
           _tip.text = GetLocalizedText(_currentLocation.TipRu, _currentLocation.TipEn, _currentLocation.TipTr, lang);
        }

        private string GetLocalizedText(string ru, string en, string tr, string currentLang)
        {
            switch (currentLang)
            {
                case "ru":
                    return string.IsNullOrEmpty(ru) ? en : ru;
                case "en":
                    return string.IsNullOrEmpty(en) ? ru : en;
                case "tr":
                    return string.IsNullOrEmpty(tr) ? en : tr;
                default:
                    Debug.LogWarning($"Unsupported language: {currentLang}. Falling back to English.");
                    return en;
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
          //  Enter();
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
            YG2.onSwitchLang -= OnSwitchLanguage;
        }

        private void OnSwitchLanguage(string lang)
        {
            UpdateUI(lang);
        }


        protected override void OnDisabled()
        {
            RemoveListener();
        }
    }
}
