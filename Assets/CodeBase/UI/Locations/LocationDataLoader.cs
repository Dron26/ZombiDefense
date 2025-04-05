using System.Collections.Generic;
using System.Linq;
using Infrastructure.AssetManagement;
using Interface;
using Services;
using UnityEngine;

namespace UI.Locations
{
    public class LocationDataLoader
    {
        private readonly IResourceLoadService _resourceLoadService;
        private readonly ILocationHandler _locationHandler;

        public LocationDataLoader()
        {
            _resourceLoadService = AllServices.Container.Single<IResourceLoadService>();
            _locationHandler = AllServices.Container.Single<ILocationHandler>();
        }

        public List<LocationProgressData> LoadLocations()
        {
            List<LocationProgressData> locations = new List<LocationProgressData>();
            string pathLocations = AssetPaths.LocationsData;
            int count = GetterFolderCount.GetFolderItemsCount(pathLocations);

            if (count <= 0)
            {
                Debug.LogWarning("Нет данных для загрузки локаций!");
                return locations;
            }

            for (int i = 0; i < count; i++)
            {
                string id = i.ToString();
                LocationData data = _resourceLoadService.Load<LocationData>($"{pathLocations}{id}");

                if (data != null)
                {
                    int enemyCount = 0;
                    
                    foreach (var waveData in data.WavesContainerData.GroupWaveData)
                    {
                        foreach (var value in waveData.EnemyCount)
                        {
                            enemyCount+= value;
                        }
                    }

                    LocationProgressData location = new LocationProgressData(
                        data.Id,
                        data.IsTutorial,
                        data.IsLocked,
                        data.IsCompleted,
                        data.BaseReward,
                        data.WavesContainerData.GroupWaveData.Count,
                        enemyCount,
                        0 // CurrentWaveLevel изначально 0
                    );

                    locations.Add(location);
                }
                else
                {
                    Debug.LogError($"Не удалось загрузить данные локации с ID: {id}");
                }
            }

            InitializeLocations(locations);
            SyncWithSaveData(locations);

            return locations;
        }

        private void InitializeLocations(List<LocationProgressData> locations)
        {
            if (locations.Count > 0)
            {
                locations[0].SetLock(false);
            }
        }

        private void SyncWithSaveData(List<LocationProgressData> locations)
        {
            List<int> completedLocationsId = _locationHandler.GetCompletedLocationId();

            if (completedLocationsId.Count == 0 && locations.Count > 0)
            {
                completedLocationsId.Add(0);
            }

            foreach (var id in completedLocationsId)
            {
                var locationData = locations.FirstOrDefault(x => x.Id == id);
                if (locationData != null)
                {
                    locationData.SetCompleted(true);
                    locationData.SetLock(false);

                    var nextLocationData = locations.FirstOrDefault(x => x.Id == id + 1);
                    nextLocationData?.SetLock(false);
                }
                else
                {
                    Debug.LogWarning($"Не найдена локация с ID: {id} среди загруженных данных.");
                }
            }

            _locationHandler.SetLocationsDatas(locations);
        }
    }
}
