using System.Collections.Generic;
using Data;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Newtonsoft.Json;
using Service.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace Service.SaveLoad
{
    public class SaveLoadService :MonoCache
    {
        private const string Key = "Key";
        private DataBase _dataBase;
        private bool _isFirstStart=true;
        public UnityAction OnSetActiveHumanoid;
        public UnityAction<WorkPoint> OnSelectedNewPoint;
        private LoadingCurtain _loadingCurtain;
        public MoneyData MoneyData => _dataBase.MoneyData;

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
                
                if (_isFirstStart)
                {
                    SetFirstStart();
                }
                
                _isFirstStart = false;
            }
        }

        private void SetStartParametrs()
        {
            _dataBase.MoneyData.AddMoney(10000); 
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

        public void SaveProgress()
        {
            throw new System.NotImplementedException();
        }

        public void ClearProgress()
        {
            throw new System.NotImplementedException();
        }

        public void SetCurtain(LoadingCurtain loadingCurtain)
        {
            _loadingCurtain=loadingCurtain;
        }
        
        public LoadingCurtain GetCurtain( )
        {
           return  _loadingCurtain;
        }
    }
}