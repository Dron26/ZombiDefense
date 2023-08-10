using System.Collections.Generic;
using Audio;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.WaveManagment;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace Service.SaveLoadService
{
    public class SaveLoadService : MonoCache
    {
        private const string Key = "Key";
        private DataBase _dataBase=new DataBase();
        private bool _isFirstStart=true;
        public UnityAction OnSetActiveHumanoid;
        public UnityAction<WorkPoint> OnSelectedNewPoint;
        private MoneyData _moneyData;
        public MoneyData MoneyData => _moneyData;

        private void Awake()
        {
            if (!PlayerPrefs.HasKey(Key))
            {
                _dataBase = new DataBase();
                _moneyData=_dataBase.MoneyData;
                print("FirstStart");
                SetStartParametrs();
                
                print("AddMoney(1000)");
            }
            else
            {
                print("SecondStart");
                //_dataBase =JsonUtility.FromJson<DataBase>(PlayerPrefs.GetString(Key)) ;
                _dataBase = JsonConvert.DeserializeObject<DataBase>(PlayerPrefs.GetString(Key));
                
                if (_isFirstStart)
                {
                    SetFirstStart();
                }
                
                _isFirstStart = false;
            }
        }

        private void SetStartParametrs()
        {
            _moneyData.AddMoney(10000); 
            Audio.Audio parametrs = new Audio.Audio();
            SetAudioSettings(parametrs);
            Save();
        }

        protected override void OnDisabled() => 
            Save();
        
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

        public void SaveHumanoidAndCount( List<int> levels ,List<int> amount )
        {
            _dataBase.AddHumanoidAndCount( levels,amount);
            Save();
        }

        public int ReadAmountHumanoids(int levelHumanoid) => 
            _dataBase.ReadHumanoid(levelHumanoid);

        public int ReadAmountMoney() =>
            _dataBase.ReadAmountMoney;
        
        public void SetAudioSettings( Audio.Audio parametrs)
        {
            _dataBase.ChangeAudioSettings(parametrs);
        }
        
        public Audio.Audio  GetAudioSettings( ) => 
            _dataBase.ReadAudioSettings();
        
        public void SetFirstStart()
        {
            _dataBase.ChangeIsFirstStart();
            Save();
        }

        public bool ReadIsFirstStart()=>
            _dataBase.ReadIsFirstStart();

        public void SetStartBattle()
        {
            _dataBase.ChangeIsStartBattle();
            Save();
        }
        
        public bool GetStartBattle() => 
            _dataBase.ReadIsStartBattle();

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
            print(_dataBase.ReadActiveEnemy().Count);
        }

        public List<Enemy> GetActiveEnemy( ) => 
            _dataBase.ReadActiveEnemy();

        public void SetInactiveEnemy(Enemy inactiveEnemy) => 
            _dataBase.ChangeInactiveEnemy( inactiveEnemy);

        public List<Enemy> GetInactiveEnemy( ) => 
            _dataBase.ReadInactiveEnemy();
        
        public void SetLevel( List<WaveData> waveDatas) => 
            _dataBase.ChangeLevelPoint( waveDatas);
        
        public List<WaveData> GetLevelPoint() => 
            _dataBase.ReadLevelPoint();


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
    }
}