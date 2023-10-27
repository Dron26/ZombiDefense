using System;
using System.Collections.Generic;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.Location;
using Infrastructure.Logic.WaveManagment;
using Service;
using Service.Audio;
using UI.Levels;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class DataBase
    {
        public MoneyData MoneyData = new MoneyData();
        public PersonalAchievements PersonalAchievements = new PersonalAchievements();
        public TimeStatistics TimeStatistics = new TimeStatistics();
        public AudioData AudioData = new AudioData();
      //  public int NumberKilledEnemies;
      //  public int AllNumberKilledEnemies;
        
        
        public int TotalPlayTime;
        public int PlayTimeToday;
        public int CompletedLevel;
        
        public int Points;
        public List<int> LevelHumanoid = new List<int>();
        public List<int> AmountHumanoids = new List<int>();

        [NonSerialized]
        public bool IsFirstStart = true;
        
        public List<LocationData> LocationsDatas=new List<LocationData>();
        public bool IsAuthorized => _isAuthorized;
        [NonSerialized] 
        public Camera CameraUI;
        [NonSerialized] 
        public Camera CameraPhysical;
        [NonSerialized] 
        public LocationData SelectedLocation=new LocationData();
        
        private bool _isAuthorized = false;
        [NonSerialized] 
        public List<Wave> _waves;
        [NonSerialized] 
        private List<Humanoid> AvaibelCharacters;
        [NonSerialized] 
        private WorkPoint SelectedPoint;
        [NonSerialized] 
        private Humanoid SelectedHumanoid;
        [NonSerialized] 
        private List<Humanoid> ActiveHumanoids = new();
        [NonSerialized] 
        private List<Enemy> ActiveEnemy = new();
        [NonSerialized] 
        private List<Humanoid> InactiveHumanoids = new();
        [NonSerialized] 
        private List<Enemy> InactiveEnemy = new();

        private int CountSpins { get;  set; }

        public void AddHumanoidAndCount(List<int> levels, List<int> amount)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                LevelHumanoid.Add(levels[i]);
                AmountHumanoids.Add(amount[i]);
            }
        }

        public int ReadHumanoid(int levelHumanoid)
        {
            int number = 0;

            for (int i = 0; i < LevelHumanoid.Count; i++)
            {
                if (levelHumanoid == LevelHumanoid[i])
                {
                    number = AmountHumanoids[i];
                }
            }

            return number;
        }

        public int ReadAmountMoney =>
            MoneyData.Money;
        
        public void AddPoints(int totalPoints) =>
            Points += totalPoints;

        public void ChangeCountSpins(int counterSpins) =>
            CountSpins = counterSpins;

        public int ReadCountSpins() =>
            CountSpins;

        public void ChangeAudioData(AudioData parametrs) =>
            AudioData = parametrs;

        public AudioData ReadAudioData() =>
            AudioData;

        public void ChangeIsFirstStart()
        {
            IsFirstStart = false;
        }

        public bool ReadIsFirstStart() =>
            IsFirstStart;

        public WorkPoint ChangeSelectedPoint(WorkPoint point) =>
            SelectedPoint = point;

        public WorkPoint ReadSelectedPoint() =>
            SelectedPoint;

        public Humanoid ChangeSelectedHumanoid(Humanoid humanoid) =>
            SelectedHumanoid = humanoid;

        public Humanoid ReadSelectedHumanoid() =>
            SelectedHumanoid;


        public void ChangeActiveHumanoid(List<Humanoid> activeHumanoids) =>
            ActiveHumanoids = new List<Humanoid>(activeHumanoids);

        public List<Humanoid> ReadActiveHumanoid() =>
            new List<Humanoid>(ActiveHumanoids);

        public void ChangeInactiveHumanoid(List<Humanoid> inactiveHumanoids) =>
            InactiveHumanoids = new List<Humanoid>(inactiveHumanoids);

        public List<Humanoid> ReadInactiveHumanoid() =>
            new List<Humanoid>(InactiveHumanoids);

        public void ChangeAvailableCharacters(List<Humanoid> avaibelCharacters) =>
            AvaibelCharacters = new List<Humanoid>(avaibelCharacters);

        public List<Humanoid> ReadAvailableCharacters() =>
            new List<Humanoid>(AvaibelCharacters);

        public void ChangeActiveEnemy(Enemy activeEnemy)
        {
            InactiveEnemy.Remove(activeEnemy);
            ActiveEnemy.Add(activeEnemy);
        }

        public List<Enemy> ReadActiveEnemy() =>
            new List<Enemy>(ActiveEnemy);

        public void ChangeInactiveEnemy(Enemy inactiveEnemy)
        {
            ActiveEnemy.Remove(inactiveEnemy);
            InactiveEnemy.Add(inactiveEnemy);
            MoneyData.AllAmountMoney++;
        }

        public int ReadCountEnemy() =>
            ActiveEnemy.Count;
        
        public void ChangeCameras(Camera cameraPhysical, Camera cameraUI)
        {
            CameraPhysical = cameraPhysical;
            CameraUI = cameraUI;
        }

        public Camera ReadPhysicalCamera()
        {
           return  CameraPhysical;
        }
        
        public Camera ReadUICamera()
        {
            return  CameraUI;
        }

        public void SetStatusAuthorization(bool isAuthorized)
        {
           _isAuthorized = isAuthorized;
        }

        
        
        public int ReadAllNumberKilledEnemies() => 
            PersonalAchievements.NumberKilledEnemies;

        public int ReadDayNumberKilledEnemies() => 
            PersonalAchievements.DayNumberKilledEnemies;

        public int ReadAllAmountMoney() => 
            MoneyData.AllAmountMoney;

        public void OnGameStart() => 
            TimeStatistics.OnGameStart();

        public void OnGameEnd() => 
            TimeStatistics.OnGameEnd();

        public void ReadTotalPlayTime() => 
            TimeStatistics.GetPlayTimeToday();
        
        public void ReadPlayTimeToday()=> 
            TimeStatistics.GetPlayTimeToday();

        public void SetSelectedLocation(LocationDataUI locationDataUI)
        {
            SelectedLocation.Id = locationDataUI.Id;
            SelectedLocation.Path = locationDataUI.Path;
            SelectedLocation.IsCompleted = locationDataUI.IsCompleted;
            SelectedLocation.IsLocked = locationDataUI.IsLocked;
           // SelectedLocation.MaxEnemyOnLevel = locationDataUI.MaxEnemyOnLevel;
           // SelectedLocation.Waves=locationDataUI.GetWaveDataInfo();
        }

        public void SetCompletedLevel()
        {
            SelectedLocation.IsCompleted=true;
            LocationsDatas[SelectedLocation.Id].IsCompleted = true;
        }

        public void ChangeLocationsDatas(List<LocationData> locationsDatas)
        {
            LocationsDatas = new List<LocationData>(locationsDatas);
        }
        
        public void ChangeNumberKilledEnemies()
        {
            PersonalAchievements.AddKilledEnemy();
        }
        
        public void ClearNumberKilledEnemies()
        {
            PersonalAchievements=new PersonalAchievements();
        }
        
        public void ChangeSurvivalCount()
        {
            PersonalAchievements.SetSurvival(ActiveHumanoids.Count);
        }
        
        public void ChangeDeadMercenaryCount()
        {
            PersonalAchievements.SetDeadMercenary(InactiveHumanoids.Count);
        }

        public LocationData GetLocation()
        {
            return SelectedLocation;
        }

        public void ChangeMaxEnemyOnLevel(int number)
        {
            SelectedLocation.MaxEnemyOnLevel = number;
        }

        public void ClearSpawnLocationData()
        {
        ActiveEnemy= new();
        InactiveEnemy= new();
        }
    }
}

public class StatisticsData
{
    
}
