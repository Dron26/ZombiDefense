
using System.Collections.Generic;
using System.Linq;
using Data;
using Enemies.AbstractEntity;
using Interface;

namespace Services.SaveLoad
{
    public class EnemyHandler:IEnemyHandler
    {
        private readonly EnemyData _enemyData;

        public EnemyHandler(EnemyData enemyData)
        {
            _enemyData = enemyData;
        }

        public void SetActiveEnemy(Enemy enemy) => 
            _enemyData.AddActiveEnemy(enemy);

        public List<Entity> GetActiveEnemy() => 
            _enemyData.GetActiveEnemy();

        public void SetInactiveEnemy(Enemy enemy) => 
            _enemyData.RemoveActiveEnemy(enemy);

        public void EnemyDeath(Enemy enemy)
        {
            _enemyData.ChangeNumberKilledEnemies();
            SetInactiveEnemy(enemy);
        }

        public int GetEnemyCount() =>
            _enemyData.GetActiveEnemyCount();
    }
}