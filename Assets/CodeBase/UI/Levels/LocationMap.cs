using System.Collections.Generic;
using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Service.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Levels
{
    public class LocationMap:MonoCache
    {
        [SerializeField] private List<LocationDataUI> _locations;
        [SerializeField] private LocationDataUI _tutorial;

        [SerializeField] private TMP_Text _cash;
        private int _selectNumber;
        private GameStateMachine _stateMachine;
        private SaveLoadService _saveLoadService;
        private bool _isFirstStarted=>_saveLoadService.IsFirstStart;
         List<LocationData> _locationData=new ();
        
        public void Initialize(GameStateMachine stateMachine,SaveLoadService saveLoadService)
        {
            _tutorial.GetComponent<Button>().onClick.AddListener(() =>OnButtonClick(_tutorial));
            
            _saveLoadService=saveLoadService;
            _stateMachine=stateMachine;

            foreach (var location in _locations)
            {
                location.GetComponent<Button>().onClick.AddListener(() =>OnButtonClick(location));
                location.SetLocked(true);
            }
            
            if (_isFirstStarted)
            {
                foreach (var location in _locations)
                {
                    LocationData data = new LocationData();
                    data.Id=location.Id;
                    data.Path=location.Path;
                    data.IsTutorial=location.IsTutorial;
                    data.IsLocked=location.IsLocked;
                    _locationData.Add(data);
                }
                
            }
            else
            {
                _locationData =  new List<LocationData>(_saveLoadService.GetLocationsDatas()) ;
                
                foreach (var location in _locationData)
                {
                    location.IsLocked = !location.IsCompleted;
                }
            }
            
            
            _locationData[0].IsLocked = false;
            _locations[0].SetLocked(false);
                
            _saveLoadService.SetLocationsDatas(_locationData);
            
            _cash.text="$"+_saveLoadService.ReadAmountMoney().ToString();
        }

        private void OnButtonClick(LocationDataUI locationDataUI)
        {
            _selectNumber=locationDataUI.Id;
            
            _saveLoadService.SetSelectedLocation(locationDataUI);
            EnterLevel();
            
        }
        
        private void EnterLevel()
        {
            Debug.Log("EnterLevel()");
            _stateMachine.Enter<LoadLevelState,string>(ConstantsData.Level); 
            Destroy(gameObject);
        } 
    }
}