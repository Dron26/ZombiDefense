using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.Factories.FactoryLocation
{
    public class LocationFactory: MonoCache
    {
        public GameObject Create(string Id)
        {
            string path =AssetPaths.LocationsPrefabs + Id;
            GameObject location = Instantiate(Resources.Load<GameObject>(path));
            return location;
        }
    }
}