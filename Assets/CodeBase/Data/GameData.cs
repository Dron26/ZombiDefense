using System;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using Infrastructure.Location;
using Service;
using Service.Audio;
using UI.Levels;
using UnityEngine;

namespace Data
{
    [Serializable] 
    public class GameData
    {
        public MoneyData  MoneyData= new MoneyData();
        public PersonalAchievements PersonalAchievements = new PersonalAchievements();
        public TimeStatistics TimeStatistics = new TimeStatistics();
        public AudioData AudioData = new AudioData();
      //  public int NumberKilledEnemies;
      //  public int AllNumberKilledEnemies;
        
        
        public int TotalPlayTime;
        public int PlayTimeToday;
        public int CompletedLevel;
        
        public int Points;
        public List<int> LevelCharacters = new List<int>();
        public List<int> AmountCharacters = new List<int>();
        public bool IsAuthorized => _isAuthorized;
        public List<int> _passedLevels;
        [NonSerialized]
        public List<Location> LocationGroup=new();

        [NonSerialized]
        public bool IsFirstStart = true;
        
        [NonSerialized] 
        public Camera CameraUI;
        [NonSerialized] 
        public Camera CameraPhysical;
        [NonSerialized] 
        public int  SelectedLocationId;
        
        private bool _isAuthorized = false;
        [NonSerialized] 
        private List<Character> AvaibelCharacters;
        [NonSerialized] 
        private WorkPoint SelectedPoint;
        [NonSerialized] 
        private Character SelectedCharacter;
        [NonSerialized] 
        private List<Character> ActiveCharacters = new();
        [NonSerialized] 
        private List<Entity> ActiveEnemy = new();
        [NonSerialized] 
        private List<Character> InactiveCharacters = new();

        public GameData()
        {
            _passedLevels = new List<int>();
        }
        
        public void AddHumanoidAndCount(List<int> levels, List<int> amount)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                LevelCharacters.Add(levels[i]);
                AmountCharacters.Add(amount[i]);
            }
        }

        public int ReadCharacters(int levelCharacters)
        {
            int number = 0;

            for (int i = 0; i < LevelCharacters.Count; i++)
            {
                if (levelCharacters == LevelCharacters[i])
                {
                    number = AmountCharacters[i];
                }
            }

            return number;
        }

        public int ReadAmountMoney =>
            MoneyData.TempMoney;

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

        public Character ChangeSelectedCharacters(Character character) =>
            SelectedCharacter = character;

        public Character ReadSelectedCharacter() =>
            SelectedCharacter;


        public void ChangeActiveCharacter(List<Character> activeCharacters) =>
            ActiveCharacters = new List<Character>(activeCharacters);

        public List<Character> ReadActiveCharacter() =>
            new List<Character>(ActiveCharacters);

        public void ChangeInactiveCharacter(List<Character> inactiveCharacters) =>
            InactiveCharacters = new List<Character>(inactiveCharacters);

        public List<Character> ReadInactiveCharacter() =>
            new List<Character>(InactiveCharacters);

        public void ChangeAvailableCharacters(List<Character> avaibelCharacters) =>
            AvaibelCharacters = new List<Character>(avaibelCharacters);

        public List<Character> ReadAvailableCharacters() =>
            new List<Character>(AvaibelCharacters);

        public void ChangeActiveEnemy(Enemy activeEnemy)
        {
            ActiveEnemy.Add(activeEnemy);
        }

        public List<Entity> ReadActiveEnemy() =>
            new List<Entity>(ActiveEnemy);

        public void ChangeInactiveEnemy(Entity entity)
        {
            ActiveEnemy.Remove(entity);
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

        public void SetSelectedLocationId(int id)
        {
            SelectedLocationId=id;
        }

        public void SetCompletedLocationId()
        {
            _passedLevels.Add(SelectedLocationId);
        }
        
        public List<int> GetCompletedLocationId()
        {
            return _passedLevels;
        }

        public void ChangeLocationsDatas(List<Location> locationsDatas)
        {
            LocationGroup = new List<Location>(locationsDatas);
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
            PersonalAchievements.SetSurvival(ActiveCharacters.Count);
        }
        
        public void ChangeDeadMercenaryCount()
        {
            PersonalAchievements.SetDeadMercenary(InactiveCharacters.Count);
        }

        public void ClearSpawnLocationData()
        {
            ActiveEnemy = new();
        }
    }
}
