using System.Collections.Generic;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Unity.VisualScripting;

namespace Infrastructure.WaveManagment
{
    public class WaveQueue:MonoCache
    {
        private Queue<Enemy> _enemiesToSpawn = new Queue<Enemy>();
        public int  Count=>_enemiesToSpawn.Count;
        public float DelayTime;
        public int Level=>_level;
        private int _level;
        
        public void Enqueue(Enemy enemy)
        {
            _enemiesToSpawn.Enqueue(enemy);
        }

        public void SetTime(float delayTime)
        {
            DelayTime = delayTime;
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