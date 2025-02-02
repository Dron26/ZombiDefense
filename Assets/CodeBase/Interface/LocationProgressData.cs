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
        
        public int BaseZombieHealth{ get; set; } 
        public int BaseReward{ get; set; }
        public int WaveCount{ get; set; } 
        public int CurrentWaveLevel{ get; set; } 
        

        public LocationProgressData(int id, bool isTutorial, bool isLocked, bool isCompleted, int baseZombieHealth, int baseReward, int waveCount, int currentWaveLevel)
        {
            Id = id;
            IsTutorial = isTutorial;
            IsLocked = isLocked;
            IsCompleted = isCompleted;
            BaseZombieHealth = baseZombieHealth;
            BaseReward = baseReward;
            WaveCount = waveCount;
            CurrentWaveLevel = currentWaveLevel;
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