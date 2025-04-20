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
        int WaveComplatedCount { get; }
        void ClearKilledEnemies();
        void SetSurvivalCount(int count);
        void SetDeadMercenaryCount(int count);
        void AddKilledCharacter(Character character);
        void SetWaveComplatedCount(int count);

        void Reset();
    }
}