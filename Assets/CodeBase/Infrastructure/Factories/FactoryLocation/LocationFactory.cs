using Infrastructure.AssetManagement;
using UnityEngine;

namespace Infrastructure.Factories.FactoryLocation
{
    public class LocationFactory
    {
        public GameObject Create(int Id)
        {
            string path =AssetPaths.LocationsPrefabs + Id;
            GameObject locationPrefab = Resources.Load<GameObject>(path);

            if (locationPrefab == null)
            {
                Debug.LogError($"Location prefab not found at Resources/{path}.");
                return null;
            }

            GameObject location = Object.Instantiate(locationPrefab);
            return location;
        }
    }
}