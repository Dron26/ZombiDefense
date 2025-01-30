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
     public class LocationManager : MonoCache
    {
        public LocationData LocationData => _locationData;
        
        private  LocationDataLoader _locationDataLoader;
        private  ILocationHandler _locationHandler;
        private  IGameEventBroadcaster _gameEventBroadcaster ;
        
        private List<LocationData> _locations = new List<LocationData>();
        private LocationData _locationData;

        public List<LocationData> GetLocations() => _locations;

        public void Initialize()
        {
            _gameEventBroadcaster = AllServices.Container.Single<IGameEventBroadcaster>();
            _gameEventBroadcaster.OnLocationCompleted += LocationCompleted;
            _locationHandler=AllServices.Container.Single<ILocationHandler>();
            _locationDataLoader = new LocationDataLoader();
            SetLocations();
        }

        public LocationData GetLocationById(int id) => _locations.FirstOrDefault(x => x.Id == id);

        public void SetLocationCompleted(int id)
        {
            LocationData locationData = _locations.FirstOrDefault(x => x.Id == id);
            if (locationData != null)
            {
                locationData.SetCompleted(true);
            }
        }
        
        public void LocationCompleted()
        {
            _locations[_locationHandler.GetSelectedLocationId()].SetCompleted(true);
        }

        private void SetLocations()
        {
            _locations = _locationDataLoader.LoadLocations();
        }
    }
}