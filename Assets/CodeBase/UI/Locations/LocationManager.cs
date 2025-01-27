using System.Collections.Generic;
using System.Linq;
using Boot.SO;
using Common;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryLocation;
using Infrastructure.Location;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Interface;
using Services;
using Services.SaveLoad;
using UnityEngine;

namespace UI.Locations
{
    public class LocationManager:MonoCache
    {
        public Location Location => _location;
        
        private GameStateMachine _stateMachine;
        private SaveLoadService _saveLoadService;
        private List<Location> _locations=new List<Location>();
        private Location _location;
        private LocationFactory _locationFactory = new LocationFactory();
        private LocationDataLoader _locationDataLoader;
        public List<Location> GetLocations() => _locations;
        public void Initialize(GameStateMachine stateMachine)
        {
            _saveLoadService=AllServices.Container.Single<SaveLoadService>();;
            _saveLoadService.OnLocationCompleted += LocationCompleted;
            _stateMachine=stateMachine;
            _locationDataLoader = new LocationDataLoader(_saveLoadService);
            
           // _currentLocations =  new List<Location>(_saveLoadService.GetLocationGroup()) ;
            SetLocations();
        }
        
        public Location GetLocationById(int id)
        {
            return _locations.FirstOrDefault(x => x.Id == id);
        }
        
        public void SetLocationCompleted(int id)
        {
            Location location = _locations.FirstOrDefault(x => x.Id == id);
            if (location != null)
            {
                location.SetCompleted(true);
                    //   _saveLoadService.SetCompletedLocationId(id);
            }
        }
        public LocationPrefab CreateLocation(int locationId)
        {
            GameObject locationPrefab = _locationFactory.Create(locationId.ToString());
            return locationPrefab.GetComponent<LocationPrefab>();
        }

        public void LocationCompleted()
        {
            _locations[_saveLoadService.GetSelectedLocationId()].SetCompleted(true);
        }

        public void SetSelectedLocationId(int id)
        {
            _saveLoadService.SetSelectedLocationId(id);
        }
        private void SetLocations()
        {
            _locationDataLoader.LoadLocations();
        }
    }
}