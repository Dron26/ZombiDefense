using System;
using Characters.Humanoids.AbstractLevel;
using Data;
using Enemies.AbstractEntity;
using Interface;

namespace Services.SaveLoad
{
    public class AchievementsHandler : IAchievementsHandler
    {
        private readonly AchievementsData _achievementsData;
        private readonly IGameEventBroadcaster _gameEvent;

        public AchievementsHandler(AchievementsData achievementsData)
        {
            _achievementsData = achievementsData ?? throw new ArgumentNullException(nameof(achievementsData));
            _gameEvent = AllServices.Container.Single<IGameEventBroadcaster>();
            AddListener();
        }

        public int KilledEnemies => _achievementsData.NumberKilledEnemies;
        public int DailyKilledEnemies => _achievementsData.DayNumberKilledEnemies;
        public int SurvivalCount => _achievementsData.SurvivalCount;
        public int DeadMercenaryCount => _achievementsData.CountDeadCharacter;

        public int WaveComplatedCount => _achievementsData.WaveComplatedCount;
        private void AddKilledEnemy( Enemy enemy)
        {
            _achievementsData.NumberKilledEnemies++;
            _achievementsData.DayNumberKilledEnemies++;
        }

        public void ClearKilledEnemies()
        {
            _achievementsData.NumberKilledEnemies = 0;
            _achievementsData.DayNumberKilledEnemies = 0;
        }

        public void SetSurvivalCount(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Survival count cannot be negative.");
            _achievementsData.SurvivalCount = count;
        }

        public void SetDeadMercenaryCount(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Dead mercenary count cannot be negative.");
            _achievementsData.CountDeadCharacter = count;
        }
        public void SetWaveComplatedCount(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Wave completed count cannot be negative.");
            _achievementsData.WaveComplatedCount = count;
        }

        public void ResetDailyAchievements()
        {
            _achievementsData.DayNumberKilledEnemies = 0;
        }
        
        public void Reset()
        {
            ResetDailyAchievements();
            ClearKilledEnemies();
            _achievementsData.SurvivalCount = 0;
            _achievementsData.CountDeadCharacter = 0;
        }
        
        private void AddListener()
        {
            _gameEvent.OnEnemyDeath+= AddKilledEnemy;
            _gameEvent.OnCharacterDie += AddKilledCharacter;
        }

        public void AddKilledCharacter(Character character)
        {
            _achievementsData.CountDeadCharacter++;
        }
    }
}