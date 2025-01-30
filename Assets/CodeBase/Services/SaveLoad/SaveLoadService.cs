using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Humanoids.AbstractLevel;
using Data;
using Enemies.AbstractEntity;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using CharacterData = Data.CharacterData;

namespace Services.SaveLoad
{
    public class SaveLoadService : MonoCache, ISaveLoadService
    {
        private const string Key = "Key";

        private GameData _gameData;
        public GameData GameData=>_gameData;
        private EventSystem _eventSystem;
        public CharacterData Characters => _gameData.Characters;
        public EnemyData Enemies => _gameData.EnemyData;
        public AchievementsData AchievementsData => _gameData.AchievementsData;
        public MoneyData Money => _gameData.Money;
        public Location Location => _gameData.Location;
        
        public CameraState CameraState=>_gameData.CameraState;
        public AudioData AudioData=>_gameData.AudioData;

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
        private GameBootstrapper _gameBootstrapper;
        public GameBootstrapper GameBootstrapper=>_gameBootstrapper;

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
           // InitializeGameData();
        }

        public void SetSelectedPoint(WorkPoint point)
        {
            Location.ChangeSelectedPoint(point);
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
        
        public void SetCameras(Camera cameraPhysical, Camera cameraUI)
        {
            _gameData.CameraState.SetCameras(cameraPhysical, cameraUI);
        }
        
        public void SetGameBootstrapper(GameBootstrapper gameBootstrapper)
        {
            _gameBootstrapper=gameBootstrapper;
        }
    }
}