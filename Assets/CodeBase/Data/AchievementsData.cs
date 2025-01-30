using System;

namespace Data
{
    [Serializable]
    public class AchievementsData
    {
        public int NumberKilledEnemies { get; set; }
        public int DayNumberKilledEnemies { get; set; }

        public int SurvivalCount { get; set; }
        public int DeadMercenaryCount { get; set; }
    }
}