using System.Collections.Generic;
using UI.Locations;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationDataRegistry", menuName = "Game Data/LocationDataRegistry")]
public class LocationDataRegistry : ScriptableObject
{
    public List<LocationData> Locations;
}