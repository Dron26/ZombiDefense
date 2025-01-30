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
        public int TimeTimeBeforeNextWave=> _timeBeforeNextWave;
        private int _timeBeforeNextWave=5;
        public int MaxEnemiesOnScene => _maxEnemiesOnScene;
        public int SelectedLocationId => _selectedLocationId;
        private int _maxEnemiesOnScene;
        
        public IReadOnlyList<int> CompletedLocations => _completedLocations.AsReadOnly();
        public void SetSelectedPointId(int id) => _selectedPointId = id;
        public void CompleteCurrentLocation() => _completedLocations.Add(_selectedLocationId);
        public void ClearCompletedLocations() => _completedLocations.Clear();
        public void ChangeSelectedPoint(WorkPoint point) => _selectedPoint = point;
        public void SetMaxEnemyOnScene(int count) => _maxEnemiesOnScene = count;
        public void LocationCompleted()
        {
            // Находим текущую выбранную локацию и отмечаем её как завершённую
            var selectedLocation = _locations.Find(location => location.Id == _selectedLocationId);
            if (selectedLocation != null)
            {
                selectedLocation.SetCompleted(true);
            }
        }

        public List<int> GetCompletedLocationId()
        {
            // Возвращаем список ID завершённых локаций
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
            // Устанавливаем данные локаций
            _locations = locationDatas;
            
        }

        public void SetSelectedLocationId(int id)
        {
            _selectedLocationId = id;
        }

        public int GetSelectedLocationId()
        {
            // Возвращаем ID выбранной локации
            return _selectedLocationId;
        }
        
        public void Reset()
        {
            _selectedLocationId = 0; 

            foreach (var location in _locations)
            {
                if (location.IsTutorial||location.Id==1) 
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
    }
}