using System;
using System.Collections.Generic;
using Data;
using Enemies.AbstractEntity;
using Interface;

namespace Services.SaveLoad
{
    public class EnemyHandler:IEnemyHandler
    {
        private readonly EnemyData _enemyData;
        private AchievementsHandler _achievementsHandler;
        private GameEventBroadcaster _eventBroadcaster;
        public EnemyHandler(EnemyData enemyData)
        {
            _enemyData = enemyData;
            _achievementsHandler = AllServices.Container.Single<AchievementsHandler>();
            _eventBroadcaster= AllServices.Container.Single<GameEventBroadcaster>();
        }

        public void SetActiveEnemy(Enemy enemy) => 
            _enemyData.AddActiveEnemy(enemy);

        public List<Entity> GetActiveEnemy() => 
            _enemyData.GetActiveEnemy();

        public void SetInactiveEnemy(Enemy enemy) => 
            _enemyData.RemoveActiveEnemy(enemy);

        public void EnemyDeath(Enemy enemy)
        {
            if (_enemyData.GetActiveEnemyCount() == 1)
            {
                _eventBroadcaster.InvokeLastEnemyRemained();
            }

            _achievementsHandler.AddKilledEnemy();
            _eventBroadcaster.InvokeOnEnemyDeath(enemy);
            SetInactiveEnemy(enemy);
        }

        public int GetEnemyCount() =>
            _enemyData.GetActiveEnemyCount();

        public void Reset()
        {
            _enemyData.ClearEnemies();
        }
    }
}