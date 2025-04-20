using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using Services;

namespace Interface
{
    public interface IAchievementsHandler:IService
    {
        int KilledEnemies { get; }
        int DailyKilledEnemies { get; }
        int SurvivalCount { get; }
        int DeadMercenaryCount { get; }
        void ClearKilledEnemies();
        void SetSurvivalCount(int count);
        void SetDeadMercenaryCount(int count);
        void AddKilledCharacter(Character character);
        
        void Reset();
    }
}