using UnityEngine;

namespace UI.Locations
{
    [CreateAssetMenu(fileName = "LocationData", menuName = "Locations/LocationData")]
    public class LocationData : ScriptableObject
    {
        public int Id;
        public bool IsTutorial;
        public bool IsLocked;
        public bool IsCompleted;

        [Header("Wave Settings")]
        public int CurrentWaveLevel; // Текущий уровень сложности (сколько раз локация пройдена)
        public int BaseZombieHealth; // Базовая прочность зомби
        public int BaseReward; // Базовая награда за локацию
        public int WaveCount; // Количество волн в локации

        [Header("Scaling")]
        public float ZombieHealthMultiplier = 1.2f; // Множитель прочности зомби
        public float RewardMultiplier = 1.1f; // Множитель награды

        public int GetCurrentZombieHealth()
        {
            return (int)(BaseZombieHealth * Mathf.Pow(ZombieHealthMultiplier, CurrentWaveLevel));
        }

        public int GetCurrentReward()
        {
            return (int)(BaseReward * Mathf.Pow(RewardMultiplier, CurrentWaveLevel));
        }

        public void SetLock(bool isLocked)
        {
            IsLocked = isLocked;
        }

        public void SetCompleted(bool isCompleted)
        {
            IsCompleted = isCompleted;
        }

        public void IncreaseWaveLevel()
        {
            CurrentWaveLevel++;
        }
    }
}