using System;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Data;
using Enemies.AbstractEntity;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using Newtonsoft.Json;
using Services.PlayerAuthorization;
using UI.Locations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Services.SaveLoad
{
    public class SaveLoadService :MonoCache,ISaveLoadService
    {
        private const string Key = "Key";
        private GameData _gameData;
        public Action OnSetActiveHumanoid;
        public Action LastHumanoidDie;
        public Action OnSetInactiveEnemy;
        public Action<WorkPoint> OnSelectedNewPoint;
        public Action<Character> OnSelectedNewCharacter;
        public Action<int> OnChangeEnemiesCountOnWave;
        public Action<Enemy> OnEnemyDeath;
        public event Action LastEnemyRemained;
        public event Action OnLocationCompleted;
        public int TimeTimeBeforeNextWave=> _timeBeforeNextWave;
        private LoadingCurtain _loadingCurtain;
        public int MaxEnemiesOnWave=>_maxEnemiesOnWave;
        private YandexAuthorization _authorization=new();
        public event Action OnClearSpawnData;
        private GameBootstrapper _gameBootstrapper;
       // private bool IsAuthorized => _authorization.IsAuthorized();
        public bool IsSelectContinueGame => _isSelectContinueGame;
        public int MaxEnemiesOnScene => _maxEnemiesOnScene;
        public bool IsExitFromLocation=>_isExitFromLocation;

        private bool _isSelectContinueGame;
        private int _maxEnemiesOnWave;
        private int _maxEnemiesOnScene;
        private GraphicRaycaster _raycastPanel;
        private EventSystem _eventSystem;
        private int _timeBeforeNextWave=5;
        private bool _isDay;
        private bool _isExitFromLocation;
        private void Awake()
        {
            if (!PlayerPrefs.HasKey(Key))
            {
                _gameData = new GameData();
                SetStartParametrs();
            }
            else
            {
                _gameData = JsonConvert.DeserializeObject<GameData>(PlayerPrefs.GetString(Key));
            }

         //   _authorization.OnAuthorizeSuccessCallback += OnAuthorizeSuccess;
            OnGameStart();
        }
        
        // private void OnAuthorizeSuccess()
        // {
        //     _gameData.SetStatusAuthorization(IsAuthorized);
        // }

        public void SetTimeBeforeNextWave(int time)
        {
            _timeBeforeNextWave=time;
        }

        private void SetStartParametrs()
        {
            int Money = 300000;
            _gameData.MoneyData.AddMoney(Money); 
            Debug.Log("Add"+Money);
            AudioData audioData = new AudioData();
            SetAudioData(audioData);
            Save();
        }

        public void Save()
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            
            // PlayerPrefs.SetString(Key, JsonUtility.ToJson(_dataBase));
            PlayerPrefs.SetString(Key, JsonConvert.SerializeObject(_gameData,Formatting.Indented, settings));
            PlayerPrefs.Save();
        }

        public int ReadAmountMoney() =>
            _gameData.ReadAmountMoney;
        
        public void SetAudioData( AudioData parametrs)
        {
            _gameData.ChangeAudioData(parametrs);
        }
        
        public AudioData  GetAudioData( ) => 
            _gameData.ReadAudioData();
        
        public void SetFirstStart()
        {
            _gameData.ChangeIsFirstStart();
            Save();
        }

        public void ResetProgress()
        {
            _gameData = new GameData();
            SetStartParametrs();
        }

        public void SetSelectedPoint(WorkPoint point)
        {
            _gameData.ChangeSelectedPoint(point);
            OnSelectedNewPoint?.Invoke(point);
        }
        
        public void SetSelectedCharacter(Character character)
        {
            if (GetSelectedCharacter() != null&&character!=GetSelectedCharacter())
            {
                IWeaponController weaponController = (IWeaponController)GetSelectedCharacter().GetComponent(typeof(IWeaponController));
                weaponController.SetSelected(false);
            }
            
            _gameData.ChangeSelectedCharacters(character);
            OnSelectedNewCharacter?.Invoke(character);
        }

        public Character GetSelectedCharacter()=>
            _gameData.ReadSelectedCharacter();

        public void SetActiveCharacters(List<Character> activeHumanoids)
        {
            _gameData.ChangeActiveCharacter(activeHumanoids);
            OnSetActiveHumanoid?.Invoke();
        }

        public List<Character> GetActiveCharacters( ) => 
            _gameData.ReadActiveCharacter();

        public void SetInactiveHumanoids(List<Character> inactiveHumanoids) => 
            _gameData.ChangeInactiveCharacter( inactiveHumanoids);

        public List<Character> GetInactiveHumanoids( ) => 
            _gameData.ReadInactiveCharacter();
        
        public void SetAvailableCharacters(List<Character> avaibelCharacters) => 
            _gameData.ChangeAvailableCharacters( avaibelCharacters);

        public List <Character> GetAvailableCharacters( ) => 
            _gameData.ReadAvailableCharacters();
        
        public void SetActiveEnemy(Enemy activeEnemy)
        {
            _gameData.ChangeActiveEnemy(activeEnemy);
        }

        public List<Entity> GetActiveEnemy( ) => 
            _gameData.ReadActiveEnemy();

        public void SetInactiveEnemy(Enemy inactiveEnemy)
        {
            
        }
        
        public int GetCountEnemy() => 
            _gameData.ReadCountEnemy();

        public void SetCameras(Camera cameraPhysical, Camera cameraUI)
        {
            _gameData.ChangeCameras(cameraPhysical,cameraUI);
        }
        
        public Camera GetPhysicalCamera()
        {
            return _gameData.ReadPhysicalCamera();
        }
        
        public Camera GetUICamera()
        {
            return _gameData.ReadUICamera();
        }

        public void SetCurtain(LoadingCurtain loadingCurtain)
        {
            _loadingCurtain=loadingCurtain;
        }
        
        public LoadingCurtain GetCurtain( )
        {
           return  _loadingCurtain;
        }
        
       

        public int GetDayNumberKilledEnemies() => 
            _gameData.ReadDayNumberKilledEnemies();

        public int GetAllAmountMoney() => 
            _gameData.ReadAllAmountMoney();


        private void OnGameStart() => 
            _gameData.OnGameStart();

        private void OnGameEnd() => 
            _gameData.OnGameEnd();

        public void GetTotalPlayTime() => 
            _gameData.ReadTotalPlayTime();
        
        public void GetPlayTimeToday()=> 
            _gameData.ReadPlayTimeToday();
        
        protected override void OnDisabled()
        {
            OnGameEnd();
            Save();
        }

        public void SaveData()
        {
            throw new NotImplementedException();
        }

        public void ClearData()
        {
            _gameData.ClearSpawnLocationData();
        }

        public GameData LoadData()
        {
            return _gameData;
        }
        
        
        
        
        
        public void LocationCompleted()
        {
            _gameData.LocationCompleted();
            OnLocationCompleted?.Invoke();
        }
        
        public List<int> GetCompletedLocationId()
        {
            return _gameData.GetCompletedLocations();
        }
        
        public void SetLocationsDatas(List<Location> locationDatas)
        {
            _gameData.ChangeLocationsDatas(locationDatas);
        }
        
        public void SetSelectedLocationId( int  id)
        {
            _gameData.SetSelectedLocationId(id);
            _isSelectContinueGame = false;
        }

        public int GetSelectedLocationId() => _gameData.SelectedLocationId;

        // private SwitchLocation()
        // {
        //     List< LocationData> data = _saveLoadService.GetLocationsDatas();
        //     int id = _saveLoadService.GetSelectedLocation().Id;
        //     id++;
        //     _saveLoadService.SetSelectedLocation();
        //     _stateMachine.Enter<LoadLevelState,string>(ConstantsData.Level); 
        // }
        
        
        
        
        
        public void SetNumberKilledEnemies()
        {
            
        }
        public int GetNumberKilledEnemies() => 
            _gameData.PersonalAchievements.NumberKilledEnemies;

        public int GetAllNumberKilledEnemies() => 
            _gameData.ReadAllNumberKilledEnemies();
        
        public void ClearNumberKilledEnemies()
        {
            _gameData.ClearNumberKilledEnemies();
        }


        public int GetSurvivalsCount()
        {
            _gameData.ChangeSurvivalCount();
            return _gameData.PersonalAchievements.NumberSurvivals;
        }

        public int GetDeadMercenaryCount()
        {
            _gameData.ChangeDeadMercenaryCount();
            return _gameData.PersonalAchievements.NumberDeadMercenary;;
        }

        public void SetGameBootstrapper(GameBootstrapper gameBootstrapper)
        {
            _gameBootstrapper=gameBootstrapper;
        }

        public GameBootstrapper GetGameBootstrapper()
        {
            return _gameBootstrapper;
        }

        public void ClearSpawnData()
        {
            _gameData.ClearSpawnLocationData();
            Save();
        }


        public void OnLastHumanoidDie()
        {
            LastHumanoidDie?.Invoke();
        }

        public void SetMaxEnemyOnWave(int number)
        {
            _maxEnemiesOnWave = number;
        }

        public void SetKilledEnemiesOnWave()
        {
            OnChangeEnemiesCountOnWave?.Invoke(--_maxEnemiesOnWave);
        }

        public void SetRaycasterPanel(GraphicRaycaster buttonPanel)
        {
            _raycastPanel = buttonPanel;
        }

        public GraphicRaycaster GetRaycasterPanel() => _raycastPanel;

        public void SetEvenSystem(EventSystem eventSystem)
        {
            _eventSystem=eventSystem;
        }

        public EventSystem GetEventSystem()=> _eventSystem;
        

        public int  FixMoneyState()
        {
           return _gameData.MoneyData.FixTempMoneyState();
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


        public void SetInfoDay(bool isDay)
        {
            _isDay=isDay;
        }

        public void SetMaxEnemyOnScene(int count)
        {
            _maxEnemiesOnScene = count;
        }

        public void ExitFromLocation(bool isExit)
        {
            _isExitFromLocation=isExit;
        }
    }
}