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
        
        public int WaveComplatedCount{ get; set;}
    }
}