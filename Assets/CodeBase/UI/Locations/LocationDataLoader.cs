using System.Collections.Generic;
using System.Linq;
using Boot.SO;
using Infrastructure.AssetManagement;
using Interface;
using Services;
using Services.SaveLoad;
using UI.Locations;

public class LocationDataLoader
{
    private readonly IResourceLoadService _resourceLoadService;
    private readonly SaveLoadService _saveLoadService;

    public LocationDataLoader(SaveLoadService saveLoadService)
    {
        _resourceLoadService =  AllServices.Container.Single<IResourceLoadService>();
        _saveLoadService = saveLoadService;
    }

    public List<Location> LoadLocations()
    {
        List<Location> locations = new List<Location>();
        string pathLocations = AssetPaths.LocationsData;
        int count = GetterFolderCount.GetFolderItemsCount(pathLocations);

        // Загрузка данных локаций
        for (int i = 0; i < count; i++)
        {
            string id = i.ToString();
            LocationData data = _resourceLoadService.Load<LocationData>($"{pathLocations}{id}");
            locations.Add(new Location(data.Id, data.IsTutorial, data.IsLocked, data.IsCompleted));
        }

        // Настройка начальных состояний
        InitializeLocations(locations);

        // Синхронизация с сохраненными данными
        SyncWithSaveData(locations);

        return locations;
    }

    private void InitializeLocations(List<Location> locations)
    {
        // Первая локация всегда разблокирована
        locations[0].SetLock(false);
    }

    private void SyncWithSaveData(List<Location> locations)
    {
        List<int> completedLocationsId = _saveLoadService.GetCompletedLocationId();

        // Если нет завершенных локаций, добавляем первую
        if (completedLocationsId.Count == 0)
        {
            completedLocationsId.Add(0);
        }

        // Обновление состояний локаций на основе сохраненных данных
        foreach (var id in completedLocationsId)
        {
            Location location = locations.FirstOrDefault(x => x.Id == id);
            if (location != null)
            {
                location.SetCompleted(true);
                location.SetLock(false);

                // Разблокируем следующую локацию
                Location nextLocation = locations.FirstOrDefault(x => x.Id == id + 1);
                nextLocation?.SetLock(false);
            }
        }

        // Сохраняем обновленные данные
        _saveLoadService.SetLocationsDatas(locations);
    }
}