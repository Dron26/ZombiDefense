using System;
using System.Collections.Generic;
using Infrastructure.Location;
using UI.Locations;

public interface ILocationProvider
{
    List<Location> GetLocations();
    Location GetLocationById(int id);
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