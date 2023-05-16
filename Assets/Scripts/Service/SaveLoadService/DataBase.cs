using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Humanoids.AbstractLevel;
using Infrastructure.Location;
using UnityEngine;

namespace Service.SaveLoadService
{
    [Serializable]
    public class DataBase
    {
        public int Money;
        public int Points;
        public AudioSettingsParameters AudioParametrs;
        public List<MergeSceneData> MergeSceneDatas=new();
        public List<GameObject> Slots=new();
        public Humanoid SelectedHumanoid;
        public List<int> _levelHumanoid=new();
        public List<int> _amountHumanoids=new();
        public   List<Humanoid> ActiveHumanoids = new();
        public   List<Humanoid> InactiveHumanoids = new();
        public List<Humanoid> AvaibelCharacters;
        public bool _isFirstStart=true;
        public bool _isBattleStart=false;
        private WorkPoint SelectedPoint;
        
        public int CountSpins { get; private set; }
        
        public void AddHumanoidAndCount( List<int> levels,List<int> amount)
        {
            for (int i = 0; i<levels.Count;i++)
            {
                _levelHumanoid.Add(levels[i]);
                _amountHumanoids.Add(amount[i]);
            }
        }

        public int ReadHumanoid(int levelHumanoid)
        {
            int number=0;

            for (int i = 0; i < _levelHumanoid.Count; i++)
            {
                if (levelHumanoid==_levelHumanoid[i])
                {
                    number = _amountHumanoids[i];
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
            foreach (GameObject slot in slots)
            {
                Slots.Clear();
                Slots.Add(slot);
            }
        }

        public List<GameObject> ReadMergeSlots()
        {
            List<GameObject> tempSlots = new();

            foreach (GameObject slot in Slots)
            {
                tempSlots.Add(slot);
            }

            return tempSlots;
        }

        public void ChangeIsFirstStart()
        {
            _isFirstStart = false;
        }

        public bool ReadIsFirstStart() => 
            _isFirstStart;

        public void ChangeIsStartBattle() => 
            _isBattleStart = true;

        public bool ReadIsStartBattle()=> 
            _isBattleStart;

        public WorkPoint ChangeSelectedPoint(WorkPoint point) => SelectedPoint = point;
        public WorkPoint ReadSelectedPoint() => SelectedPoint;
        
        public Humanoid  ChangeSelectedHumanoid(Humanoid humanoid) => SelectedHumanoid = humanoid;

        public Humanoid  ReadSelectedHumanoid() => SelectedHumanoid;


        public void ChangeActiveHumanoid(List<Humanoid> activeHumanoids)
        {
            ActiveHumanoids=new List<Humanoid>(activeHumanoids);
        }
        
        public List<Humanoid> ReadActiveHumanoid( )
        {
            return   new List<Humanoid>( ActiveHumanoids);
        }
        
        public void ChangeInactiveHumanoid(List<Humanoid> inactiveHumanoids)
        {
            InactiveHumanoids=new List<Humanoid>(inactiveHumanoids);
        }
        
        public List<Humanoid> ReadInactiveHumanoid( )
        {
            return  new List<Humanoid>( InactiveHumanoids);
        }

        public void ChangeAvailableCharacters(List<Humanoid> avaibelCharacters)
        {
            AvaibelCharacters = new List<Humanoid>(avaibelCharacters);
        }

        public List<Humanoid> ReadAvailableCharacters()
        {
            return  new List<Humanoid>( AvaibelCharacters);
        }
    }
}