using System;

namespace Data
{
    [Serializable]
    public class AchievementsData
    {
        public int KilledEnemies { get; set; }
        public int AllKilledEnemies { get; set; }
        public int SurvivalCount { get; set; }
        public int CountDeadCharacter { get; set; }
        public int WaveComplatedCount { get; set; }
        
        public long TotalPlayTimeMs;
        public long LastSessionDurationMs;
        public long LastSessionTimestamp;
        public long LastLoginTimeBinary;
        public string MoneyLeaderboardTableName = "Money";
        public string DeadZombiesLeaderboardTableName = "DeadZombies";
        public bool isTutorialEnd{ get; set; }
        public long LastDailyRewardTimeBinary;
        public bool IsRewardClaimedToday;
        public int StreakLoginDay;
        public bool IsLocationPassed{ get; set; }

    }
}