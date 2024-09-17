using System;
using System.Collections.Generic;
using System.Linq;
using Boot.SO;
using Common;
using Data;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryLocation;
using Infrastructure.Location;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Interface;
using Service;
using Service.SaveLoad;
using UI.Locations;
using UnityEngine;

namespace UI.Levels
{
    
    public class LocationManager:MonoCache
    {
        public Location Location => _location;
        
        private GameStateMachine _stateMachine;
        private SaveLoadService _saveLoadService;
        private List<Location> _locations=new List<Location>();
        private Location _location;
        private LocationFactory _locationFactory = new LocationFactory();
        
        public void Init(GameStateMachine stateMachine,SaveLoadService saveLoadService)
        {
            _saveLoadService=saveLoadService;
            _saveLoadService.OnSetCompletedLocation += SetCompletedLocation;
            _stateMachine=stateMachine;
            
           // _currentLocations =  new List<Location>(_saveLoadService.GetLocationGroup()) ;
            SetLocations();
        }
        
        public LocationPrefab CreateLocation()
        {
            
            GameObject locationPrefab = _locationFactory.Create( _locations[_saveLoadService.GetSelectedLocationId()].Id.ToString());
            LocationPrefab prefab = locationPrefab.GetComponent<LocationPrefab>();
            
            // _playerCharacterInitializer=location.GetComponentInChildren<PlayerCharacterInitializer>();
            // _enemyCharacterInitializer=location.GetComponentInChildren<EnemyCharacterInitializer>();
            // CameraData cameraData = location.GetComponentInChildren<CameraData>();
            // _cameraInputMovement.SetPosition(cameraData); 
            // _saveLoadService.SetInfoDay(cameraData.IsDay);
            // _location=location;
            // tempLocation=null;
            //
            // if (_saveLoadService.GetSelectedLocationId().IsTutorial)
            // {
            //     _isTutorialLevel=true;
            //     TutorialLevel tutorialLevel = location.GetComponent<TutorialLevel>();
            //     OnLoaded+=()=> tutorialLevel.Initialize(this);
            //     tutorialLevel.OnEndTutorial+=SwicthScene;
            // }
            // else
            // {
            //     _isTutorialLevel = false;
            // }

            return prefab;
        }

        public void SetCompletedLocation()
        {
            _locations[_saveLoadService.GetSelectedLocationId()].SetCompleted(true);
        }

        public void SetSelectedLocationId(int id)
        {
            _saveLoadService.SetSelectedLocationId(id);
            EnterLevel();
        }
        
        private void SetLocations()
        {
            string pathLocations = AssetPaths.LocationsData;
            int count= GetterFolderCount.GetFolderItemsCount(pathLocations);
            
            for (int i = 0; i < count; i++)
            {
                string id = i.ToString();
                LocationData data = AllServices.Container.Single<IResourceLoadService>().Load<LocationData>($"{pathLocations}{id}");
                Location location = new Location(data.Id,data.IsTutorial,data.IsLocked,data.IsCompleted);
                
                _locations.Add(location);
            }
            
            _locations[0].SetLock(false);
                
            List<int> completedLocationsId=_saveLoadService.GetCompletedLocationId();
            
            if (completedLocationsId.Count==0)
            {
                completedLocationsId.Add(0);
            }
            
            foreach (var id in completedLocationsId)
            {
                Location data = _locations.FirstOrDefault(x => x.Id == id);
                
                    data.SetCompleted(true);
                    data.SetLock(false);
                    data = _locations.FirstOrDefault(x => x.Id == id + 1);
                    data.SetLock(false);
            }

            _saveLoadService.SetLocationsDatas(_locations);
        }

        public List<Location> GetLocations() => _locations;

        private void EnterLevel()
        {
            Debug.Log("EnterLevel()");
            _stateMachine.Enter<LoadLevelState, string>(Constants.Location);
        } 
        
       
    }
}