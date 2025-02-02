using System.Collections.Generic;
using System.Linq;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using UnityEngine;

namespace UI.Locations
{
    public class LocationManager : MonoCache
    {
        public LocationProgressData LocationData => _locationData;

        private LocationDataLoader _locationDataLoader;
        private ILocationHandler _locationHandler;
        private IGameEventBroadcaster _gameEventBroadcaster;

        private List<LocationProgressData> _locations = new List<LocationProgressData>();
        private LocationProgressData _locationData;

        public List<LocationProgressData> GetLocations() => _locations;

        public void Initialize()
        {
            _gameEventBroadcaster = AllServices.Container.Single<IGameEventBroadcaster>();
            _gameEventBroadcaster.OnLocationCompleted += LocationCompleted;
            _locationHandler = AllServices.Container.Single<ILocationHandler>();
            _locationDataLoader = new LocationDataLoader();
            SetLocationsData();
        }

        public LocationProgressData GetLocationById(int id) => _locations.FirstOrDefault(x => x.Id == id);

        public void SetCompletedLocation(int id)
        {
            LocationProgressData locationData = _locations.FirstOrDefault(x => x.Id == id);
            if (locationData != null)
            {
                locationData.SetCompleted(true);
            }
        }

        public void LocationCompleted()
        {
            _locations[_locationHandler.GetSelectedLocationId()].SetCompleted(true);
        }

        private void SetLocationsData()
        {
            _locations = _locationDataLoader.LoadLocations();
        }
    }
}