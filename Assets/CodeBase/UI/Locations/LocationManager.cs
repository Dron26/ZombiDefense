using System.Collections.Generic;
using System.Linq;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryLocation;
using Infrastructure.Location;
using Infrastructure.StateMachine;
using Interface;
using Services;
using Services.SaveLoad;
using UnityEngine;

namespace UI.Locations
{
    public class LocationManager:MonoCache
    {
        public LocationData LocationData => _locationData;
        
        private GameStateMachine _stateMachine;
        private SaveLoadService _saveLoadService;
        private List<LocationData> _locations=new List<LocationData>();
        private LocationData _locationData;
        private LocationFactory _locationFactory = new LocationFactory();
        private LocationDataLoader _locationDataLoader;
        public List<LocationData> GetLocations() => _locations;
        
        public void Initialize(GameStateMachine stateMachine)
        {
            _saveLoadService=AllServices.Container.Single<SaveLoadService>();;
            _saveLoadService.OnLocationCompleted += LocationCompleted;
            _stateMachine=stateMachine;
            _locationDataLoader = new LocationDataLoader(_saveLoadService);
            
            // _currentLocations =  new List<Location>(_saveLoadService.GetLocationGroup()) ;
            SetLocations();
        }
        
        public LocationData GetLocationById(int id)
        {
            return _locations.FirstOrDefault(x => x.Id == id);
        }
        
        public void SetLocationCompleted(int id)
        {
            LocationData locationData = _locations.FirstOrDefault(x => x.Id == id);
            if (locationData != null)
            {
                locationData.SetCompleted(true);
                //   _saveLoadService.SetCompletedLocationId(id);
            }
        }
        public LocationPrefab CreateLocation(int locationId)
        {
            GameObject locationPrefab = _locationFactory.Create(locationId);
            return locationPrefab.GetComponent<LocationPrefab>();
        }

        public void LocationCompleted()
        {
            _locations[AllServices.Container.Single<LocationHandler>().GetSelectedLocationId()].SetCompleted(true);
        }

        
        private void SetLocations()
        {
            _locations=_locationDataLoader.LoadLocations();
        }
    }
}