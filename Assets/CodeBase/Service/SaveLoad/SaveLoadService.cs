using System;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Data;
using Enemies.AbstractEntity;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Newtonsoft.Json;
using Service.Audio;
using Service.PlayerAuthorization;
using UI.HUD.StorePanel;
using UI.Levels;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Service.SaveLoad
{
    public class SaveLoadService :MonoCache,ISaveLoadService
    {
        private const string Key = "Key";
        private DataBase _dataBase;
        public Action OnSetActiveHumanoid;
        public Action OnCompleteLocation;
        public Action LastHumanoidDie;
        public Action OnSetInactiveEnemy;
        public Action<WorkPoint> OnSelectedNewPoint;
        public Action<Character> OnSelectedNewCharacter;
        public Action<int> OnChangeEnemiesCountOnWave;
        public Action<Enemy> OnEnemyDeath;
        private LoadingCurtain _loadingCurtain;
        public int MaxEnemiesOnWave=>_maxEnemiesOnWave;
        private YandexAuthorization _authorization=new();
        public event Action OnClearSpawnData;
        private GameBootstrapper _gameBootstrapper;
        
        private bool IsAuthorized => _authorization.IsAuthorized();
        public bool IsSelectContinueGame => _isSelectContinueGame;
        private bool _isSelectContinueGame;
        private int _maxEnemiesOnWave;
        private GraphicRaycaster _raycastPanel;
        private EventSystem _eventSystem;
        
        private void Awake()
        {
            
                
            if (!PlayerPrefs.HasKey(Key))
            {
                _dataBase = new DataBase();
                SetStartParametrs();
            }
            else
            {
                _dataBase = JsonConvert.DeserializeObject<DataBase>(PlayerPrefs.GetString(Key));
            }

            _authorization.OnAuthorizeSuccessCallback += OnAuthorizeSuccess;
            OnGameStart();
        }
        
        private void OnAuthorizeSuccess()
        {
            _dataBase.SetStatusAuthorization(IsAuthorized);
        }

        public void GetMoneyData()
        {
            
        }

        private void SetStartParametrs()
        {
            int Money = 300000;
            _dataBase.MoneyData.AddMoney(Money); 
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
            PlayerPrefs.SetString(Key, JsonConvert.SerializeObject(_dataBase,Formatting.Indented, settings));
            PlayerPrefs.Save();
        }

        public int ReadAmountMoney() =>
            _dataBase.ReadAmountMoney;
        
        public void SetAudioData( AudioData parametrs)
        {
            _dataBase.ChangeAudioData(parametrs);
        }
        
        public AudioData  GetAudioData( ) => 
            _dataBase.ReadAudioData();
        
        public void SetFirstStart()
        {
            _dataBase.ChangeIsFirstStart();
            Save();
        }

        public void ResetProgress()
        {
            _dataBase = new DataBase();
            SetStartParametrs();
        }

        public void SetSelectedPoint(WorkPoint point)
        {
            _dataBase.ChangeSelectedPoint(point);
            OnSelectedNewPoint?.Invoke(point);
        }
        
        public void SetSelectedCharacter(Character character)
        {
            // if (GetSelectedCharacter() != null&&character!=GetSelectedCharacter())
            // {
            //     IWeaponController weaponController = (IWeaponController)character.GetComponent(typeof(IWeaponController));
            //     weaponController.SetSelected(false);
            //     
            // }
            
            _dataBase.ChangeSelectedCharacters(character);
            OnSelectedNewCharacter?.Invoke(character);
        }

        public Character GetSelectedCharacter()=>
            _dataBase.ReadSelectedCharacter();

        public void SetActiveCharacters(List<Character> activeHumanoids)
        {
            _dataBase.ChangeActiveCharacter(activeHumanoids);
            OnSetActiveHumanoid?.Invoke();
        }

        public List<Character> GetActiveCharacters( ) => 
            _dataBase.ReadActiveCharacter();

        public void SetInactiveHumanoids(List<Character> inactiveHumanoids) => 
            _dataBase.ChangeInactiveCharacter( inactiveHumanoids);

        public List<Character> GetInactiveHumanoids( ) => 
            _dataBase.ReadInactiveCharacter();
        
        public void SetAvailableCharacters(List<Character> avaibelCharacters) => 
            _dataBase.ChangeAvailableCharacters( avaibelCharacters);

        public List <Character> GetAvailableCharacters( ) => 
            _dataBase.ReadAvailableCharacters();
        
        public void SetActiveEnemy(Enemy activeEnemy)
        {
            _dataBase.ChangeActiveEnemy(activeEnemy);
        }

        public List<Enemy> GetActiveEnemy( ) => 
            _dataBase.ReadActiveEnemy();

        public void SetInactiveEnemy(Enemy inactiveEnemy)
        {
            _dataBase.ChangeInactiveEnemy(inactiveEnemy);
            OnSetInactiveEnemy?.Invoke();
        }
        
        public int GetCountEnemy() => 
            _dataBase.ReadCountEnemy();

        public void SetCameras(Camera cameraPhysical, Camera cameraUI)
        {
            _dataBase.ChangeCameras(cameraPhysical,cameraUI);
        }
        
        public Camera GetPhysicalCamera()
        {
            return _dataBase.ReadPhysicalCamera();
        }
        
        public Camera GetUICamera()
        {
            return _dataBase.ReadUICamera();
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
            _dataBase.ReadDayNumberKilledEnemies();

        public int GetAllAmountMoney() => 
            _dataBase.ReadAllAmountMoney();


        private void OnGameStart() => 
            _dataBase.OnGameStart();

        private void OnGameEnd() => 
            _dataBase.OnGameEnd();

        public void GetTotalPlayTime() => 
            _dataBase.ReadTotalPlayTime();
        
        public void GetPlayTimeToday()=> 
            _dataBase.ReadPlayTimeToday();
        
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
            throw new NotImplementedException();
        }

        public DataBase LoadData()
        {
            return _dataBase;
        }
        
        
        public void SetCompletedLocation()
        {
            _dataBase.SetCompletedLevel();
            OnCompleteLocation?.Invoke();
        }
        
        public void SetLocationsDatas(List<LocationData> locationDatas)
        {
           
            _dataBase.ChangeLocationsDatas(locationDatas);
        }
        public List<LocationData> GetLocationsDatas()
        {
            return _dataBase.LocationsDatas;
        }
        
        public void SetSelectedLocation( LocationDataUI locationDataUI)
        {
            _dataBase.SetSelectedLocation(locationDataUI);
            _isSelectContinueGame = false;
        }

        public LocationData GetSelectedLocation() =>
            _dataBase.SelectedLocation;
        
        public void SetNumberKilledEnemies()
        {
            
        }
        public int GetNumberKilledEnemies() => 
            _dataBase.PersonalAchievements.NumberKilledEnemies;

        public int GetAllNumberKilledEnemies() => 
            _dataBase.ReadAllNumberKilledEnemies();
        
        public void ClearNumberKilledEnemies()
        {
            _dataBase.ClearNumberKilledEnemies();
        }


        public int GetSurvivalsCount()
        {
            _dataBase.ChangeSurvivalCount();
            return _dataBase.PersonalAchievements.NumberSurvivals;
        }

        public int GetDeadMercenaryCount()
        {
            _dataBase.ChangeDeadMercenaryCount();
            return _dataBase.PersonalAchievements.NumberDeadMercenary;;
        }

        public void SetGameBootstrapper(GameBootstrapper gameBootstrapper)
        {
            _gameBootstrapper=gameBootstrapper;
        }

        public GameBootstrapper GetGameBootstrapper()
        {
            return _gameBootstrapper;
        }
        
        public void ChangeMaxEnemyOnLevel(int number)
        {
            _dataBase.ChangeMaxEnemyOnLevel(number);
            _isSelectContinueGame=true;
        }

        public void ClearSpawnData()
        {
            OnClearSpawnData?.Invoke();
            _dataBase.ClearSpawnLocationData();
        }


        public void OnLastHumanoidDie()
        {
            LastHumanoidDie?.Invoke();
        }

        public void SetMaxEnemyOnWave(int number)
        {
            _maxEnemiesOnWave = number;
            OnChangeEnemiesCountOnWave?.Invoke(_maxEnemiesOnWave);
        }

        public void SetKilledEnemiesOnWave(int number)
        {
            int count=_maxEnemiesOnWave-number;
            OnChangeEnemiesCountOnWave?.Invoke(count);
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
           return _dataBase.MoneyData.FixTempMoneyState();
        }

        public void EnemyDeath(Enemy enemy)
        {
            _dataBase.ChangeNumberKilledEnemies();
            OnEnemyDeath?.Invoke(enemy);
        }
        
    }
}