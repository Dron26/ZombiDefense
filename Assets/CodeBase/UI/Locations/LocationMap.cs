using System;
using System.Collections.Generic;
using Data;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Interface;
using Service;
using Service.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace UI.Levels
{
    public class LocationMap:MonoCache
    {

        private GameStateMachine _stateMachine;
        private SaveLoadService _saveLoadService;
        [SerializeField] private TMP_Text _cash;
        private List<Location> _locations;
        private List<LocationUIElement> _locationsUI;
        [SerializeField] private GameObject _locationContainer;
        
        public event Action<int> OnSelectLocation;
        public void Initialize( SaveLoadService saveLoadService,LocationManager locationManager)
        {
            _saveLoadService=saveLoadService;
            _cash.text="$"+_saveLoadService.ReadAmountMoney().ToString();
            _locations=locationManager.GetLocations();

            InitializeButton();
        }

        private void InitializeButton()
        {
            foreach (Transform childTransform in _locationContainer.transform)
            {
                LocationUIElement locationUIElement = childTransform.GetComponent<LocationUIElement>();
                
                if (locationUIElement != null)
                {
                    Button button = childTransform.GetComponentInChildren<Button>();
                    
                    if (button != null)
                    {
                        button.onClick.AddListener(() => OnButtonClick(locationUIElement.Id));
                        Location location = _locations[locationUIElement.Id];
                        locationUIElement.SetCompleted(location.IsCompleted);
                        locationUIElement.SetLock(location.IsLocked);
                    }
                }
            }
        }

        private void OnButtonClick(int id)
        {
            OnSelectLocation?.Invoke(id);
        }
        
        //менееджер отдельно от мап, ошибка при сериализации так как привязаны обьекты нужно сделать загрузку через путь, если нет то просто в префабынакидать
        
        
    }
}