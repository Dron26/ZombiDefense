namespace Services.SaveLoad
{
    public class AchievementsData
    {
        public int NumberKilledEnemies { get; private set; }
        public int DayNumberKilledEnemies { get; private set; }
        public int SurvivalCount { get; private set; }
        public int DeadMercenaryCount { get; private set; }

        public void AddKilledEnemy() => NumberKilledEnemies++;
        public void ClearKilledEnemies() => NumberKilledEnemies = DayNumberKilledEnemies = 0;
        public void SetSurvivalCount(int count) => SurvivalCount = count;
        public void SetDeadMercenaryCount(int count) => DeadMercenaryCount = count;
    }
}