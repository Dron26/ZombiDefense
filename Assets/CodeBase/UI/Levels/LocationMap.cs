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
         List<LocationData> _locationData=new ();
        
        public void Initialize(GameStateMachine stateMachine,SaveLoadService saveLoadService)
        {
            _tutorial.GetComponent<Button>().onClick.AddListener(() =>OnButtonClick(_tutorial));
            
            _saveLoadService=saveLoadService;
            _stateMachine=stateMachine;
            
            _locationData =  new List<LocationData>(_saveLoadService.GetLocationsDatas()) ;
            
            
            
            if (_locationData.Count==0)
            {
                foreach (var location in _locations)
                {
                    location.SetLocked(true);
                    
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
                
                _locationData[0].IsLocked = false;
                _locations[0].SetLocked(false);
                
                for (int i = 0; i < _locationData.Count; i++)
                {
                   

                    bool completed = _locationData[i].IsCompleted;
                    bool finished = i != _locationData.Count - 1;
                    
                    if (_locationData[i].IsCompleted&&i!=_locationData.Count-1)
                    {
                        _locations[i].SetCompleted( _locationData[i].IsCompleted);
                        
                        _locationData[i + 1].IsLocked=false;
                        _locations[i + 1].SetLocked(false);
                    }
                }
            }

            foreach (var location in _locations)
            {
                location.GetComponentInChildren<Button>().onClick.AddListener(() => OnButtonClick(location));
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