using System;
using Data;
using Interface;

namespace Services.SaveLoad
{
    public class AchievementsHandler : IAchievementsHandler
    {
        private readonly AchievementsData _achievementsData;

        public AchievementsHandler(AchievementsData achievementsData)
        {
            _achievementsData = achievementsData ?? throw new ArgumentNullException(nameof(achievementsData));
        }

        public int KilledEnemies => _achievementsData.NumberKilledEnemies;
        public int DailyKilledEnemies => _achievementsData.DayNumberKilledEnemies;
        public int SurvivalCount => _achievementsData.SurvivalCount;
        public int DeadMercenaryCount => _achievementsData.DeadMercenaryCount;

        public void AddKilledEnemy()
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
            _achievementsData.DeadMercenaryCount = count;
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
            _achievementsData.DeadMercenaryCount = 0;
        }
    }
}