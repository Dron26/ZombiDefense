using System.Collections.Generic;
using System.Linq;
using Enemies.AbstractEntity;

namespace Data
{
    public class EnemyData: IEnemyData
    {
        private readonly List<Enemy> _activeEnemies = new();
        private readonly List<Enemy> _inactiveEnemies = new();

        public IReadOnlyList<Enemy> ActiveEnemies => _activeEnemies.AsReadOnly();
        public IReadOnlyList<Enemy> InactiveEnemies => _inactiveEnemies.AsReadOnly();

        public void AddActiveEnemy(Enemy enemy) => _activeEnemies.Add(enemy);
        public void RemoveActiveEnemy(Enemy enemy)
        {
            _activeEnemies.Remove(enemy);
            _inactiveEnemies.Add(enemy);
        }

        public int GetActiveEnemyCount() => _activeEnemies.Count;
        public void ClearEnemies() => _activeEnemies.Clear();
        
        public List<Entity> GetActiveEnemy() => 
            _activeEnemies.Select(enemy => (Entity)enemy).ToList();
    }
}