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
        private IGameEventBroadcaster _eventBroadcaster;
        private int _maxEnemyOnWave;
        private bool _isSpawnEnd=false;
        public EnemyHandler(IGameEventBroadcaster eventBroadcaster)
        {
            _enemyData = new EnemyData();
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
            if (_enemyData.GetActiveEnemyCount() == 1&&_isSpawnEnd)
            {
                _eventBroadcaster.InvokeLastEnemyRemained();
            }

            _eventBroadcaster.InvokeOnEnemyDeath(enemy);
            SetInactiveEnemy(enemy);
        }

        public int GetEnemyCount() =>
            _enemyData.GetActiveEnemyCount();

        public void Reset()
        {
            _enemyData.ClearEnemies();
        }

        public void SetMaxEnemyOnWave(int maxEnemyOnWave)
        {
            _eventBroadcaster.InvokeOnSetMaxEnemy(maxEnemyOnWave);
            _maxEnemyOnWave= maxEnemyOnWave;
        }
        
        public int GetMaxEnemyOnWave() => _maxEnemyOnWave;

        public void SetEndSpawn(bool isSpawnEnd)
        {
            _isSpawnEnd = isSpawnEnd;
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