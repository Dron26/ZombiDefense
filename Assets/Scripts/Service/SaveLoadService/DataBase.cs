﻿using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.Location;
using Infrastructure.WaveManagment;
using UI.Levels;
using UnityEngine;

namespace Service.SaveLoadService
{
    [Serializable]
    public class DataBase
    {
      
        public int Money;
      
        public int Points;
       
        public AudioSettingsParameters AudioParametrs;
        
        public List<int> LevelHumanoid = new();
       
        public List<int> AmountHumanoids = new();
         
        public bool IsFirstStart = true;
        [NonSerialized] 
        public Camera CameraUI;
        [NonSerialized] 
        public Camera CameraPhysical;
        
        public bool IsBattleStart = false;
        [NonSerialized] 
        public List<WaveData> _waveDatas;
        [NonSerialized] 
        private List<MergeSceneData> MergeSceneDatas = new();
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
            Money;

        public int ReadPointsDamage => Points;


        public void AddMoney(int amountMoney) =>
            Money += amountMoney;

        public void SpendMoney(int amountSpendMoney) =>
            Money -= Mathf.Clamp(amountSpendMoney, 0, int.MaxValue);

        public void AddPoints(int totalPoints) =>
            Points += totalPoints;

        public void ChangeCountSpins(int counterSpins) =>
            CountSpins = counterSpins;

        public int ReadCountSpins() =>
            CountSpins;

        public void ClearMergeSceneDatas() =>
            MergeSceneDatas.Clear();

        public void ChangeAudioSettings(AudioSettingsParameters parametrs) =>
            AudioParametrs = parametrs;

        public AudioSettingsParameters ReadAudioSettings() =>
            AudioParametrs;

        public void ChangeMergeSlots(List<GameObject> slots)
        {
            
        }

        public List<GameObject> ReadMergeSlots()
        {

            return null;
        }

        public void ChangeIsFirstStart()
        {
            IsFirstStart = false;
        }

        public bool ReadIsFirstStart() =>
            IsFirstStart;

        public void ChangeIsStartBattle() =>
            IsBattleStart = true;

        public bool ReadIsStartBattle() =>
            IsBattleStart;

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
        }

        public List<Enemy> ReadInactiveEnemy() =>
            new List<Enemy>(InactiveEnemy);

        public void ChangeLevelPoint(List<WaveData> waveDatas) => 
            _waveDatas = waveDatas;
        
        public List<WaveData> ReadLevelPoint() => 
            _waveDatas;

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
        
    }
}