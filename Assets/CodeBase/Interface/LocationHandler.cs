using System.Collections.Generic;
using System.Linq;
using Data;
using Infrastructure.Location;
using UnityEngine;
using Interface;
using Services;
using Services.SaveLoad;

namespace Interface
{
    public class LocationHandler : ILocationHandler
    {
        private List<LocationProgressData> _locationProgressData;
        private int _selectedLocationId;
        private readonly List<int> _completedLocations = new List<int>();
        private int _selectedPointId;
        private WorkPoint _selectedPoint;
        private int _timeBeforeNextWave = 5;
        private int _maxEnemiesOnScene;
        private bool _isExitFromLocation;
        private GameData _gameData;
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

        public LocationHandler( List<LocationProgressData> locationProgressData)
        {
            _locationProgressData = locationProgressData;
            AllServices.Container.Single<IGameEventBroadcaster>().OnExitedLocation+= OnExitedLocation;
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
                selectedLocation.SetCurrentWaveLevel(selectedLocation.CurrentWaveLevel + 1);
                _completedLocations.Add(selectedLocation.Id);

                AllServices.Container.Single<ISaveLoadService>().UpdateLocationProgressData(_locationProgressData);

                AllServices.Container.Single<ISaveLoadService>().Save();
            }
        }

        public List<int> GetCompletedLocationId()
        {
            return _locationProgressData
                .Where(location => location.IsCompleted)
                .Select(location => location.Id)
                .ToList();
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
        
        private void OnExitedLocation()
        {
            _isExitFromLocation = true;
        }
    }
}
