using System;
using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using Services.SaveLoad;
using UnityEngine;

namespace UI.Locations
{
    public class LocationUIManager:MonoCache
    {

        [SerializeField] private GameObject _locationContainer;

        private List<LocationUIElement> _locationUIElements;
        private LocationManager _locationManager;
        public Action OnSelectLocation;
        private SaveLoadService _saveLoadService;
        public void Initialize(SaveLoadService saveLoadService, LocationManager locationManager)
        {
            _locationManager = locationManager;
            _saveLoadService=saveLoadService;
            InitializeUIElements();
        }

        private void InitializeUIElements()
        {
            _locationUIElements = new List<LocationUIElement>();
            
            foreach (Transform child in _locationContainer.transform)
            {
                var uiElement = child.GetComponent<LocationUIElement>();
                if (uiElement != null)
                {
                    _locationUIElements.Add(uiElement);
                    
                    LocationData locationData = _locationManager.GetLocationById(uiElement.Id);
                    uiElement.Initialize(locationData.IsLocked,locationData.IsCompleted);
                    uiElement.OnClick += HandleLocationClick;
                }
            }
        }

        private void HandleLocationClick(int id)
        {
            AllServices.Container.Single<LocationHandler>().SetSelectedLocationId(id);
            OnSelectLocation?.Invoke();
           
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