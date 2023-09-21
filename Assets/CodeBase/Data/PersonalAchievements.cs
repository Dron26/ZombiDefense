using System;

namespace Data
{
    [Serializable]
    public class PersonalAchievements
    {
        public int NumberKilledEnemies=>_numberKilledEnemies;
        public int DayNumberKilledEnemies=>_dayNumberKilledEnemies;
        public int NumberSurvivals=>_numberSurvivable;
        public int NumberDeadMercenary=>_numberDeadMercenary;
        
        private int _numberKilledEnemies;
        private int _dayNumberKilledEnemies;
        private int _numberSurvivable;
        private int _numberDeadMercenary;
        
        public void AddKilledEnemy()
        {
            _numberKilledEnemies++;
            _dayNumberKilledEnemies++;
        }

        public void SetSurvival(int coiuntSurvivals)
        {
            _numberSurvivable=coiuntSurvivals;
        }
        public void SetDeadMercenary(int countDeadMercenary)
        {
            _numberDeadMercenary=countDeadMercenary;
        }
        
        public void ClearDayInfo()
        {
            _dayNumberKilledEnemies = 0;
        }
    }
}