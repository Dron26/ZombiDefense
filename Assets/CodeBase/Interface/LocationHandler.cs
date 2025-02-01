using System;
using System.Collections.Generic;
using Infrastructure.Location;
using Services.SaveLoad;
using UI.Locations;

namespace Interface
{
    public class LocationHandler : ILocationHandler
    {
        private List<LocationData> _locations = new List<LocationData>();
        private int _selectedLocationId;
        private readonly List<int> _completedLocations = new();
        private int _selectedPointId;
        private WorkPoint _selectedPoint;
        public int TimeBeforeNextWave => _timeBeforeNextWave;
        public bool IsExitFromLocation
        {
            get => _isExitFromLocation;
            set => _isExitFromLocation = value;
        }

        private int _timeBeforeNextWave = 5;
        public int MaxEnemiesOnScene => _maxEnemiesOnScene;
        public int SelectedLocationId
        {
            get => _selectedLocationId;
            set => _selectedLocationId = value;
        }

        private int _maxEnemiesOnScene;
        private bool _isExitFromLocation;

        public IReadOnlyList<int> CompletedLocations => _completedLocations.AsReadOnly();

        public void SetSelectedPointId(int id) => _selectedPointId = id;
        public void CompleteCurrentLocation() => _completedLocations.Add(_selectedLocationId);
        public void ClearCompletedLocations() => _completedLocations.Clear();
        public void ChangeSelectedPoint(WorkPoint point) => _selectedPoint = point;
        public void SetMaxEnemyOnScene(int count) => _maxEnemiesOnScene = count;

        public void LocationCompleted()
        {
            var selectedLocation = _locations.Find(location => location.Id == _selectedLocationId);
            if (selectedLocation != null)
            {
                selectedLocation.SetCompleted(true);
                selectedLocation.IncreaseWaveLevel(); // Увеличиваем уровень сложности
            }
        }

        public List<int> GetCompletedLocationId()
        {
            var completedLocations = new List<int>();
            foreach (var location in _locations)
            {
                if (location.IsCompleted)
                {
                    completedLocations.Add(location.Id);
                }
            }
            return completedLocations;
        }

        public void SetLocationsDatas(List<LocationData> locationDatas)
        {
            _locations = locationDatas;
        }

        public void SetLocationsExite(bool isLocation)
        {
            _isExitFromLocation = isLocation;
        }

        public void SetSelectedLocationId(int id)
        {
            _selectedLocationId = id;
        }

        public int GetSelectedLocationId()
        {
            return _selectedLocationId;
        }

        public void Reset()
        {
            _selectedLocationId = 0;

            foreach (var location in _locations)
            {
                if (location.IsTutorial || location.Id == 1)
                {
                    location.SetLock(false);
                }
                else
                {
                    location.SetLock(true);
                }

                location.SetCompleted(false);
            }
        }

        // Новый метод для получения текущих данных о локации
        public LocationData GetCurrentLocationData()
        {
            return _locations.Find(location => location.Id == _selectedLocationId);
        }
    }
}