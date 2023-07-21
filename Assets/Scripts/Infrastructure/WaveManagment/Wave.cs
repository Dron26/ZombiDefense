using System.Collections.Generic;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;

namespace Infrastructure.WaveManagment
{
    public class Wave:MonoCache
    {
        private List<Enemy> _enemiesToSpawn = new ();
        public int  Count=>_enemiesToSpawn.Count;
        public float DelayTime;
        public int Level=>_level;
        private int _level;
        
        public void AddEnemy(Enemy enemy)
        {
            _enemiesToSpawn.Add(enemy);
        }

        public void SetTime(float delayTime)
        {
            DelayTime = delayTime;
        }

        public Enemy GetEnemy(int index)
        {
            if (index >= 0 && index < _enemiesToSpawn.Count)
            {
                return _enemiesToSpawn[index];
            }
            
            return null;
        }

        public bool IsEmpty()
        {
            return _enemiesToSpawn.Count == 0;
        }
    }
}