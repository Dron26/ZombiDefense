using System.Collections.Generic;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;

namespace Infrastructure.WaveManagment
{
    public class WaveQueue:MonoCache
    {
        private Queue<Enemy> _enemiesToSpawn = new Queue<Enemy>();

        public void Enqueue(Enemy enemyData)
        {
            _enemiesToSpawn.Enqueue(enemyData);
        }

        public Enemy Dequeue()
        {
            if (_enemiesToSpawn.Count > 0)
                return _enemiesToSpawn.Dequeue();

            return null;
        }

        public bool IsEmpty()
        {
            return _enemiesToSpawn.Count == 0;
        }
    }
}