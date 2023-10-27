using System;
using System.Collections.Generic;
using Data;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Newtonsoft.Json;
using Service.Audio;
using Service.PlayerAuthorization;
using UI.Levels;
using UnityEngine;

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
        public Action<int> OnChangeEnemiesCountOnWave;
        private LoadingCurtain _loadingCurtain;
        public MoneyData MoneyData => _dataBase.MoneyData; 
        public int MaxEnemiesOnWave=>_maxEnemiesOnWave;
        private YandexAuthorization _authorization=new();
        public event Action OnClearSpawnData;
        private GameBootstrapper _gameBootstrapper;
        
        private bool IsAuthorized => _authorization.IsAuthorized();
        public bool IsSelectContinueGame => _isSelectContinueGame;
        private bool _isSelectContinueGame;
        private int _maxEnemiesOnWave;


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
        
        public void SetSelectedHumanoid(Humanoid humanoid)
        {
            if (GetSelectedHumanoid() != null&&humanoid!=GetSelectedHumanoid())
            {
                GetSelectedHumanoid().SetSelected(false);
            }
            
            _dataBase.ChangeSelectedHumanoid(humanoid);
        }

        public Humanoid GetSelectedHumanoid()=>
            _dataBase.ReadSelectedHumanoid();

        public void SetActiveHumanoids(List<Humanoid> activeHumanoids)
        {
            _dataBase.ChangeActiveHumanoid(activeHumanoids);
            OnSetActiveHumanoid?.Invoke();
        }

        public List<Humanoid> GetActiveHumanoids( ) => 
            _dataBase.ReadActiveHumanoid();

        public void SetInactiveHumanoids(List<Humanoid> inactiveHumanoids) => 
            _dataBase.ChangeInactiveHumanoid( inactiveHumanoids);

        public List<Humanoid> GetInactiveHumanoids( ) => 
            _dataBase.ReadInactiveHumanoid();
        
        public void SetAvailableCharacters(List<Humanoid> avaibelCharacters) => 
            _dataBase.ChangeAvailableCharacters( avaibelCharacters);

        public List <Humanoid> GetAvailableCharacters( ) => 
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
            _dataBase.ChangeNumberKilledEnemies() ;
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
    }
}