using System.Collections.Generic;
using System.Linq;
using Infrastructure.AssetManagement;
using Interface;
using Services;
using Services.SaveLoad;

namespace UI.Locations
{
    public class LocationDataLoader
    {
        private readonly IResourceLoadService _resourceLoadService;
        private readonly SaveLoadService _saveLoadService;
        private readonly LocationHandler _locationHandler;
        public LocationDataLoader(SaveLoadService saveLoadService)
        {
            _resourceLoadService =  AllServices.Container.Single<IResourceLoadService>();
            _saveLoadService = saveLoadService;
            _locationHandler=AllServices.Container.Single<LocationHandler>();
        }

        public List<LocationData> LoadLocations()
        {
            List<LocationData> locations = new List<LocationData>();
            string pathLocations = AssetPaths.LocationsData;
            int count = GetterFolderCount.GetFolderItemsCount(pathLocations);

            // Загрузка данных локаций
            for (int i = 0; i < count; i++)
            {
                string id = i.ToString();
                LocationData data = _resourceLoadService.Load<LocationData>($"{pathLocations}{id}");
                locations.Add(new LocationData(data.Id, data.IsTutorial, data.IsLocked, data.IsCompleted));
            }

            // Настройка начальных состояний
            InitializeLocations(locations);

            // Синхронизация с сохраненными данными
            SyncWithSaveData(locations);

            return locations;
        }

        private void InitializeLocations(List<LocationData> locations)
        {
            // Первая локация всегда разблокирована
            locations[0].SetLock(false);
        }
    
        private void SyncWithSaveData(List<LocationData> locations)
        {
            List<int> completedLocationsId = _locationHandler.GetCompletedLocationId();

            // Если нет завершенных локаций, добавляем первую
            if (completedLocationsId.Count == 0)
            {
                completedLocationsId.Add(0);
            }

            // Обновление состояний локаций на основе сохраненных данных
            foreach (var id in completedLocationsId)
            {
                LocationData locationData = locations.FirstOrDefault(x => x.Id == id);
                if (locationData != null)
                {
                    locationData.SetCompleted(true);
                    locationData.SetLock(false);

                    // Разблокируем следующую локацию
                    LocationData nextLocationData = locations.FirstOrDefault(x => x.Id == id + 1);
                    nextLocationData?.SetLock(false);
                }
            }

            // Сохраняем обновленные данные
            _locationHandler.SetLocationsDatas(locations);
        }
    }
}