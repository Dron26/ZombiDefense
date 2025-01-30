using System;
using System.Collections.Generic;
using Infrastructure.Location;

namespace UI.Locations
{
    public interface ILocationProvider
    {
        List<LocationData> GetLocations();
        LocationData GetLocationById(int id);
    }

    public interface ILocationLoader
    {
        LocationPrefab LoadLocation(int locationId);
    }

    public interface ILocationUIManager
    {
        void Initialize(ILocationProvider locationProvider, Action<int> onLocationSelected);
        void UpdateUI();
    }
}