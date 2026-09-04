using System.Collections.Generic;
using System.Linq;
using Infrastructure.AssetManagement;
using Interface;
using Services;
using UnityEngine;
using System.Collections;

namespace UI.Locations
{
    public class LocationDataLoader
    {
        private readonly IResourceLoadService _resourceLoadService;
        private readonly ILocationHandler _locationHandler;
        private static Dictionary<int, LocationData> _locationDataCache;
        private static readonly Dictionary<LocationData, LocationCache> _cacheData = new Dictionary<LocationData, LocationCache>();

        public LocationDataLoader()
        {
            _resourceLoadService = AllServices.Container.Single<IResourceLoadService>();
            _locationHandler = AllServices.Container.Single<ILocationHandler>();
            if (_locationDataCache == null)
            {
                _locationDataCache = new Dictionary<int, LocationData>();
            }
        }

        public List<LocationProgressData> LoadLocations()
        {
            var registry = _resourceLoadService.Load<LocationDataRegistry>("Locations/LocationDataRegistry");
            if (registry == null || registry.Locations.Count == 0) return new List<LocationProgressData>();

            var allData = LoadAllLocationData();
            var locations = new List<LocationProgressData>();

            foreach (var locationRef in registry.Locations)
            {
                if (locationRef == null) continue;
                if (!allData.ContainsKey(locationRef.Id)) continue;

                var data = allData[locationRef.Id];
                var cache = GetOrCreateCache(data);
                var location = new LocationProgressData(
                    data.Id,
                    data.IsTutorial,
                    data.IsLocked,
                    data.IsCompleted,
                    data.BaseReward,
                    cache.WaveCount,
                    cache.TotalEnemies,
                    0,
                    data.IsAdditional,
                    data.UnlockedId,
                    data.TitleRu,
                    data.TitleEn,
                    data.TitleTr,
                    data.ContextRu,
                    data.ContextEn,
                    data.ContextTr,
                    data.ObjectiveRu,
                    data.ObjectiveEn,
                    data.ObjectiveTr,
                    data.LocationRu,
                    data.LocationEn,
                    data.LocationTr,
                    data.TipRu,
                    data.TipEn,
                    data.TipTr
                );
                locations.Add(location);
            }

            SyncWithSaveData(locations);
            return locations;
        }

        private Dictionary<int, LocationData> LoadAllLocationData()
        {
            if (_locationDataCache.Count > 0) return _locationDataCache;

            var allData = _resourceLoadService.LoadAll<LocationData>("Locations/LocationData");
            foreach (var data in allData)
            {
                if (data == null) continue;
                _locationDataCache[data.Id] = data;
            }

            return _locationDataCache;
        }

        private LocationCache GetOrCreateCache(LocationData data)
        {
            if (_cacheData.ContainsKey(data))
            {
                return _cacheData[data];
            }

            var cache = new LocationCache();
            if (data.WavesContainerData != null && data.WavesContainerData.GroupWaveData != null)
            {
                cache.WaveCount = data.WavesContainerData.GroupWaveData.Count;
                foreach (var waveData in data.WavesContainerData.GroupWaveData)
                {
                    if (waveData == null || waveData.EnemyCount == null) continue;
                    foreach (var value in waveData.EnemyCount)
                    {
                        cache.TotalEnemies += value;
                    }
                }
            }
            _cacheData[data] = cache;
            return cache;
        }

        public IEnumerator LoadLocationsAsync(System.Action<List<LocationProgressData>> onComplete)
        {
            var registry = _resourceLoadService.Load<LocationDataRegistry>("Locations/LocationDataRegistry");
            if (registry == null || registry.Locations.Count == 0)
            {
                onComplete(new List<LocationProgressData>());
                yield break;
            }

            var allData = _resourceLoadService.LoadAll<LocationData>("Locations/LocationData");
            yield return null;

            var locationDict = new Dictionary<int, LocationData>();
            foreach (var data in allData)
            {
                if (data == null) continue;
                locationDict[data.Id] = data;
                GetOrCreateCache(data);
            }

            var locations = new List<LocationProgressData>();
            for (int i = 0; i < registry.Locations.Count; i++)
            {
                var locationRef = registry.Locations[i];
                if (locationRef == null) continue;
                if (!locationDict.ContainsKey(locationRef.Id)) continue;

                var data = locationDict[locationRef.Id];
                var cache = GetOrCreateCache(data);
                var location = new LocationProgressData(
                    data.Id,
                    data.IsTutorial,
                    data.IsLocked,
                    data.IsCompleted,
                    data.BaseReward,
                    cache.WaveCount,
                    cache.TotalEnemies,
                    0,
                    data.IsAdditional,
                    data.UnlockedId,
                    data.TitleRu,
                    data.TitleEn,
                    data.TitleTr,
                    data.ContextRu,
                    data.ContextEn,
                    data.ContextTr,
                    data.ObjectiveRu,
                    data.ObjectiveEn,
                    data.ObjectiveTr,
                    data.LocationRu,
                    data.LocationEn,
                    data.LocationTr,
                    data.TipRu,
                    data.TipEn,
                    data.TipTr
                );
                locations.Add(location);

                if (i % 10 == 0) yield return null;
            }

            SyncWithSaveData(locations);
            onComplete(locations);
        }

        private void SyncWithSaveData(List<LocationProgressData> locations)
        {
            var completedIds = _locationHandler.GetCompletedLocationId();
            foreach (var id in completedIds)
            {
                var location = locations.FirstOrDefault(x => x.Id == id);
                if (location == null) continue;
                location.SetCompleted(true);
                location.SetLock(false);
            }

            for (int i = 0; i < locations.Count; i++)
            {
                var unlockedId = locations[i].UnlockedId;
                if (unlockedId < 0 || unlockedId >= locations.Count) continue;
                if (locations[unlockedId].IsCompleted) locations[i].SetLock(false);
            }

            _locationHandler.SetLocationsDatas(locations);
        }

        private class LocationCache
        {
            public int TotalEnemies;
            public int WaveCount;
        }
    }
}