using System;
using System.Collections.Generic;
using Interface;
using Services;
using Services.SaveLoad;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public MoneyData Money { get; private set; } = new();
        public AchievementsData AchievementsData { get; private set; } = new();
        public EnemyData EnemyData { get; private set; } = new();
        public CharacterData Characters { get; private set; } = new();
        public Location Location { get; private set; } = new();
        public CameraState CameraState { get; private set; } = new();

        public AudioData AudioData { get; private set; } = new AudioData();
        public TimeStatistics TimeStatistics { get; private set; } = new TimeStatistics();

        public bool IsFirstStart { get; private set; } = true;
        
        public  List<LocationProgressData> LocationProgressData { get; private set; } = new();

        public void ChangeIsFirstStart()
        {
            IsFirstStart = false;
        }

        public void AddInitialMoney()
        {
            if (IsFirstStart)
            {
                Money.Money = 100; 
                ChangeIsFirstStart();
            }
        }

        public void UpdateAudioSettings(AudioData audioData) => AudioData = audioData;
        public void OnGameStart() => TimeStatistics.OnGameStart();
        public void OnGameEnd() => TimeStatistics.OnGameEnd();
        
        [Header("Scaling")]
        public float ZombieHealthMultiplier = 1.2f; 
        public float RewardMultiplier = 1.1f; 
    }
}