using System;
using System.Collections.Generic;
using Data;
using Enemies.AbstractEntity;
using Interface;

namespace Services.SaveLoad
{
    public class EnemyHandler:IEnemyHandler
    {
        private readonly IEnemyData _enemyData;
        private IAchievementsHandler _achievementsHandler;
        private IGameEventBroadcaster _eventBroadcaster;
        public EnemyHandler(IEnemyData enemyData,IAchievementsHandler achievementsHandler, IGameEventBroadcaster eventBroadcaster)
        {
            _enemyData = enemyData;
            _achievementsHandler = achievementsHandler;
            _eventBroadcaster = eventBroadcaster;
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
                _eventBroadcaster.InvokeLastHumanoidDie();
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

public interface IEnemyData
{
    public IReadOnlyList<Enemy> ActiveEnemies { get; }
    public IReadOnlyList<Enemy> InactiveEnemies { get; }
    public void AddActiveEnemy(Enemy enemy);
    public void RemoveActiveEnemy(Enemy enemy);
    public int GetActiveEnemyCount();
    public void ClearEnemies();
    public List<Entity> GetActiveEnemy();
}