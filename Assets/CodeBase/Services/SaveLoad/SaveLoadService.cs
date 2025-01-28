using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Humanoids.AbstractLevel;
using Data;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using Newtonsoft.Json;
using Services.PlayerAuthorization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Services.SaveLoad
{
    public class SaveLoadService : MonoCache, ISaveLoadService
    {
        private const string Key = "Key";

        private GameData _gameData;
        private EventSystem _eventSystem;
        // Доступ к подклассам
        public CharacterData Characters => _gameData.Characters;
        public EnemyData Enemies => _gameData.EnemyData;
        public AchievementsData Achievements => _gameData.Achievements;
        public MoneyData Money => _gameData.Money;
        public LocationData Locations => _gameData.Locations;
        
        public CameraState CameraState=>_gameData.CameraState;

        public event Action OnSetActiveHumanoid;
        public event Action LastHumanoidDie;
        public event Action<WorkPoint> OnSelectedNewPoint;
        public event Action<Character> OnSelectedNewCharacter;
        public event Action<int> OnChangeEnemiesCountOnWave;
        public event Action<Enemy> OnEnemyDeath;
        public event Action LastEnemyRemained;
        public event Action OnLocationCompleted;
     
        private int _timeBeforeNextWave = 5;
        private bool _isSelectContinueGame;
        private bool _isExitFromLocation;

        public bool IsSelectContinueGame => _isSelectContinueGame;
        public bool IsExitFromLocation => _isExitFromLocation;

        private void Awake()
        {
            if (!PlayerPrefs.HasKey(Key))
            {
                _gameData = new GameData();
                InitializeGameData();
            }
            else
            {
                _gameData = JsonConvert.DeserializeObject<GameData>(PlayerPrefs.GetString(Key));
            }

            OnGameStart();
        }

        private void InitializeGameData()
        {
            // Установить стартовые параметры
            Money.AddMoney(300000);
            Save();
        }

        public void Save()
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            PlayerPrefs.SetString(Key, JsonConvert.SerializeObject(_gameData, Formatting.Indented, settings));
            PlayerPrefs.Save();
        }

        public void SaveData()
        {
            throw new NotImplementedException();
        }

        public GameData LoadData() => _gameData;

        public void ResetProgress()
        {
            _gameData = new GameData();
            InitializeGameData();
        }

        public void SetSelectedPoint(WorkPoint point)
        {
            Locations.ChangeSelectedPoint(point);
            OnSelectedNewPoint?.Invoke(point);
        }

        public void SetSelectedCharacter(Character character)
        {
            if (Characters.SelectedCharacter != null && character != Characters.SelectedCharacter)
            {
                IWeaponController weaponController = (IWeaponController)Characters.SelectedCharacter.GetComponent(typeof(IWeaponController));
                weaponController.SetSelected(false);
            }

            Characters.SetSelectedCharacter(character);
            OnSelectedNewCharacter?.Invoke(character);
        }

        public Character GetSelectedCharacter() => Characters.SelectedCharacter;

        public void SetActiveCharacters(List<Character> activeCharacters)
        {
            Characters.ClearCharacters();
            foreach (var character in activeCharacters)
            {
                Characters.AddActiveCharacter(character);
            }
            OnSetActiveHumanoid?.Invoke();
        }

        public List<Character> GetActiveCharacters() => new List<Character>(Characters.ActiveCharacters);

        public void SetInactiveHumanoids(List<Character> inactiveHumanoids)
        {
            foreach (var character in inactiveHumanoids)
            {
                Characters.AddInactiveCharacter(character);
            }
        }

        public List<Character> GetInactiveHumanoids() => new List<Character>(Characters.InactiveCharacters);

        public void AddActiveEnemy(Enemy enemy)
        {
            Enemies.AddActiveEnemy(enemy);
        }

        public void RemoveActiveEnemy(Enemy enemy)
        {
            Enemies.RemoveActiveEnemy(enemy);
        }

        public List<Enemy> GetActiveEnemies() => new List<Enemy>(Enemies.ActiveEnemies);

        public void EnemyDeath(Enemy enemy)
        {
            RemoveActiveEnemy(enemy);

            if (Enemies.GetActiveEnemyCount() == 1)
            {
                LastEnemyRemained?.Invoke();
            }

            Achievements.AddKilledEnemy();
            OnEnemyDeath?.Invoke(enemy);
        }

        public void LocationCompleted()
        {
            Locations.CompleteCurrentLocation();
            OnLocationCompleted?.Invoke();
        }

        public List<int> GetCompletedLocationIds() => Locations.CompletedLocations.ToList();

        public void SetMaxEnemiesOnWave(int count)
        {
            _timeBeforeNextWave = count;
        }

        private void OnGameStart()
        {
            _gameData.OnGameStart();
        }

        private void OnGameEnd()
        {
            _gameData.OnGameEnd();
            Save();
        }

        protected override void OnDisabled()
        {
            OnGameEnd();
        }
        
        public void SetEvenSystem(EventSystem eventSystem)
        {
            _eventSystem=eventSystem;
        }
        public EventSystem GetEventSystem()=> _eventSystem;
        
        
        public void SetCameras(Camera cameraPhysical, Camera cameraUI)
        {
            _gameData.CameraState.SetCameras(cameraPhysical, cameraUI);
        }

        public Camera GetPhysicalCamera() => _gameData.CameraState.PhysicalCamera;

        public Camera GetUICamera() => _gameData.CameraState.UICamera;
        public void OnLastHumanoidDie()
        {
            LastHumanoidDie?.Invoke();
        }
        public void EnemyDeath(Enemy enemy)
        {
            
            SetKilledEnemiesOnWave();
            
            if (_gameData.ReadActiveEnemy().Count==1)
            { 
                LastEnemyRemained?.Invoke();
            }
            
            _gameData.ChangeNumberKilledEnemies();
            _gameData.ChangeInactiveEnemy(enemy);
            OnEnemyDeath?.Invoke(enemy);
        }



        
    }
}
