using System;
using UnityEngine;

namespace Interface
{
    [Serializable]
    public class LocationProgressData
    {
        public int Id { get; set; }
        public bool IsTutorial{ get; set; }
        public bool IsLocked{ get; set; }
        public bool IsCompleted{ get; set; }
        
        public int BaseReward{ get; set; }
        public int WaveCount{ get; set; } 
        public int EnemyCount{ get; set; } 
        public int CurrentWaveLevel{ get; set; } 
        public bool IsAdditional;
        public int UnlockedId;

        public LocationProgressData(int id, bool isTutorial, bool isLocked, bool isCompleted, int baseReward, int waveCount,int enemyCount, int currentWaveLevel,bool isAdditional,int unlockedId)
        {
            Id = id;
            IsTutorial = isTutorial;
            IsLocked = isLocked;
            IsCompleted = isCompleted;
            BaseReward = baseReward;
            WaveCount = waveCount;
            EnemyCount = enemyCount;
            CurrentWaveLevel = currentWaveLevel;
            IsAdditional=isAdditional;
            UnlockedId=unlockedId;
        }

        public void SetLock(bool isLocked)
        {
            IsLocked = isLocked;
        }

        public void SetCompleted(bool isCompleted)
        {
            IsCompleted = isCompleted;
        }
        public void SetCurrentWaveLevel(int currentWaveLevel)
        {
            CurrentWaveLevel = currentWaveLevel;
        }
    }
}