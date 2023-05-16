using System.Collections.Generic;
using Audio;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Newtonsoft.Json;
using Service.SaveLoadService;
using UnityEngine;

namespace Service.SaveLoadService
{
    public class SaveLoad : MonoCache
    {
        private const string Key = "Key";
        private DataBase _dataBase=new DataBase();
        private bool _isFirstStart=true;
         
        public int ReadPointsDamage => _dataBase.ReadPointsDamage;
     
        private void Awake()
        {

           
            
            if (!PlayerPrefs.HasKey(Key))
            {
                _dataBase = new DataBase();
                print("FirstStart");
                SetStartParametrs();
                
                print("AddMoney(100)");
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
            _dataBase.AddMoney(100); 
            AudioSettingsParameters parametrs = new AudioSettingsParameters();
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

        public void ApplyMoney(int amountMoney)
        {
            _dataBase.AddMoney(amountMoney);
            Save();
        }

        public void SpendMoney(int amountSpendMoney)
        {
            _dataBase.SpendMoney(amountSpendMoney);
            Save();
        }

        public int ReadAmountHumanoids(int levelHumanoid) => 
            _dataBase.ReadHumanoid(levelHumanoid);

        public int ReadAmountMoney() =>
            _dataBase.ReadAmountMoney;
        
        public void ApplyTotalPoints(int totalPoints)
        {
            _dataBase.AddPoints(totalPoints);
            Save();
        }

        public int GetCountSpins() => 
            _dataBase.ReadCountSpins();
        
        public void SaveCountSpins(int counterSpins) => 
            _dataBase.ChangeCountSpins(counterSpins);

        public void SetAudioSettings( AudioSettingsParameters parametrs)
        {
            _dataBase.ChangeAudioSettings(parametrs);
        }
        
        public AudioSettingsParameters  GetAudioSettings( ) => 
            _dataBase.ReadAudioSettings();

        public void SetMergedSlots(List<GameObject> slots)
        {
            _dataBase.ChangeMergeSlots(slots);
            Save();
        }

        public List<GameObject> GetMergedSlots()=>
            _dataBase.ReadMergeSlots();

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

        public void SetSelectedPOoint(WorkPoint point) => 
            _dataBase.ChangeSelectedPoint(point);

        public WorkPoint GetSelectedPOoint()=>
            _dataBase.ReadSelectedPoint();
        
        public void SetSelectedHumanoid(Humanoid humanoid) => 
            _dataBase.ChangeSelectedHumanoid(humanoid);

        public Humanoid GetSelectedHumanoid()=>
            _dataBase.ReadSelectedHumanoid();

        public void SetActiveHumanoids(List<Humanoid> activeHumanoids) => 
            _dataBase.ChangeActiveHumanoid( activeHumanoids);

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
    }
}