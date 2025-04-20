using System.Collections.Generic;
using Enemies.AbstractEntity;
using Services;

namespace Interface
{
    public interface  IEnemyHandler:IService
    {
        void SetActiveEnemy(Enemy enemy);
        List<Entity> GetActiveEnemy();
        void SetInactiveEnemy(Enemy enemy);
        void EnemyDeath(Enemy enemy);
        int GetEnemyCount();
        void Reset();
        void SetMaxEnemyOnWave(int maxEnemyOnWave);
        int GetMaxEnemyOnWave();

        void SetEndSpawn(bool isSpawnEnd);

    }
}