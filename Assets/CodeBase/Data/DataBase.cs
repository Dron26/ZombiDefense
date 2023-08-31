using System;
using System.Collections.Generic;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.Location;
using Service;
using Service.Audio;
using Service.PlayerAuthorization;
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
        public int Points;
        public AudioData AudioData = new AudioData();
        public List<int> LevelHumanoid = new List<int>();
        public List<int> AmountHumanoids = new List<int>();
        [NonSerialized]
        public bool IsFirstStart = true;
        [NonSerialized]
        public LevelData LevelData=new LevelData();
        public bool IsAuthorized => _isAuthorized;
        [NonSerialized] 
        public Camera CameraUI;
        [NonSerialized] 
        public Camera CameraPhysical;
        
        private bool _isAuthorized = false;
        [NonSerialized] 
        public List<WaveData> _waveDatas;
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

        public List<Enemy> ReadInactiveEnemy() =>
            new List<Enemy>(InactiveEnemy);
        
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
            PersonalAchievements.AllNumberKilledEnemies;

        public int ReadDayNumberKilledEnemies() => 
            PersonalAchievements.DayNumberKilledEnemies;

        public int ReadAllAmountMoney() => 
            MoneyData.AllAmountMoney;

        public int ReadAmountMoneyPerDay() => 
            MoneyData.AmountMoneyPerDay;

        public void OnGameStart() => 
            TimeStatistics.OnGameStart();

        public void OnGameEnd() => 
            TimeStatistics.OnGameEnd();

        public void ReadTotalPlayTime() => 
            TimeStatistics.GetPlayTimeToday();
        
        public void ReadPlayTimeToday()=> 
            TimeStatistics.GetPlayTimeToday();

        public void ChangeLevelData(Level level)
        {
            LevelData.Number=level.Number;
            LevelData.Path=level.Path;
            LevelData.WaveDatas=level.GetWaveDataInfo();
        }

        public LevelData ReadLevelData() => 
            LevelData;
    }
}