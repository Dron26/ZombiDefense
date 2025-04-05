using System.Collections.Generic;
using System.Linq;
using Data;
using Infrastructure.Location;
using UnityEngine;
using Interface;
using Services;

namespace Interface
{
    public class LocationHandler : ILocationHandler
    {
        private List<LocationProgressData> _locationProgressData = new List<LocationProgressData>();
        private int _selectedLocationId;
        private readonly List<int> _completedLocations = new List<int>();
        private int _selectedPointId;
        private WorkPoint _selectedPoint;
        private int _timeBeforeNextWave = 5;
        private int _maxEnemiesOnScene;
        private bool _isExitFromLocation;

        public int TimeBeforeNextWave => _timeBeforeNextWave;
        public bool IsExitFromLocation
        {
            get => _isExitFromLocation;
            set => _isExitFromLocation = value;
        }
        public int MaxEnemiesOnScene => _maxEnemiesOnScene;
        public int SelectedLocationId
        {
            get => _selectedLocationId;
            set => _selectedLocationId = value;
        }
        public float ZombieHealthMultiplier { get; set; }
        public float RewardMultiplier { get; set; }
        public IReadOnlyList<int> CompletedLocations => _completedLocations.AsReadOnly();

        public LocationHandler(GameData gameData)
        {
            _locationProgressData = gameData.LocationProgressData;
        }

        public void SetSelectedPointId(int id) => _selectedPointId = id;

        public void CompleteCurrentLocation() => _completedLocations.Add(_selectedLocationId);

        public void ClearCompletedLocations() => _completedLocations.Clear();

        public void ChangeSelectedPoint(WorkPoint point) => _selectedPoint = point;

        public void SetMaxEnemyOnScene(int count) => _maxEnemiesOnScene = count;

        public void LocationCompleted()
        {
            var selectedLocation = _locationProgressData.Find(location => location.Id == _selectedLocationId);
            if (selectedLocation != null)
            {
                selectedLocation.SetCompleted(true);
                selectedLocation.SetCurrentWaveLevel(selectedLocation.CurrentWaveLevel + 1); // Увеличиваем уровень сложности
            }
        }

        public List<int> GetCompletedLocationId()
        {
            var completedLocations = new List<int>();
            foreach (var location in _locationProgressData)
            {
                if (location.IsCompleted)
                {
                    completedLocations.Add(location.Id);
                }
            }
            return completedLocations;
        }

        public void SetLocationsDatas(List<LocationProgressData> locationProgressData)
        {
            _locationProgressData = locationProgressData;
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

            foreach (var location in _locationProgressData)
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

        public LocationProgressData GetCurrentLocationData()
        {
            return _locationProgressData.Find(location => location.Id == _selectedLocationId);
        }


        public int GetCurrentReward()
        {
            return (int)(_locationProgressData[_selectedLocationId].BaseReward * Mathf.Pow(RewardMultiplier, _locationProgressData[_selectedLocationId].CurrentWaveLevel));
        }

        public void IncreaseWaveLevel()
        {
            var currentLocation = _locationProgressData.FirstOrDefault(x => x.Id == _selectedLocationId);
            if (currentLocation != null)
            {
                currentLocation.SetCurrentWaveLevel(currentLocation.CurrentWaveLevel + 1);
            }
        }
    }
}
