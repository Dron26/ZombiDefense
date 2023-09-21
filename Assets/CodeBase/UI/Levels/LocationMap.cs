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
        [SerializeField] private List<Location> _locations;
        [SerializeField] private Location _tutorial;

        [SerializeField] private GameObject container;
        [SerializeField] private TMP_Text _cash;
        private int _selectNumber;
        private GameStateMachine _stateMachine;
        private SaveLoadService _saveLoadService;
        private bool _isFirstStarted=>_saveLoadService.IsFirstStart;
        [SerializeField] List<LocationData> _locationData=new List<LocationData>();
        private Dictionary<int,bool> LocationsLocked=new Dictionary<int,bool>();
        
        public void Initialize(GameStateMachine stateMachine,SaveLoadService saveLoadService)
        {
            _tutorial.GetComponent<Button>().onClick.AddListener(() =>OnButtonClick(_tutorial));
            
            _saveLoadService=saveLoadService;
            _stateMachine=stateMachine;

            foreach (var level in _locations)
            {
                level.GetComponent<Button>().onClick.AddListener(() =>OnButtonClick(level));
                level.SetLocked(true);
            }
            
            if (_isFirstStarted)
            {
                foreach (var level in _locations)
                {
                    LocationData data = new LocationData();
                    data.Id=level.Id;
                    data.Path=level.Path;
                    data.IsTutorial=level.IsTutorial;
                    data.IsLocked=level.IsLocked;
                    data.MaxEnemyOnLevel=level.MaxEnemyOnLevel;
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

        private void OnButtonClick(Location location)
        {
            _selectNumber=location.Id;
            
            _saveLoadService.SetSelectedLocation(location);
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