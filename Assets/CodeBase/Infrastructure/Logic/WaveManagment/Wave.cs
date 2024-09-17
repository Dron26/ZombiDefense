using System.Collections.Generic;
using Data;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.Logic.WaveManagment
{
    public class Wave : MonoCache
    {
        [SerializeField] private List<Enemy> _enemies;
        [SerializeField] private List<int> _enemyCount;
        [SerializeField] private int _timeBetweenWaves;
        public int TimeBetweenWaves => _timeBetweenWaves;
        public void AddData(List<Enemy> enemies, List<int> enemyCount)
        {
            _enemies = new List<Enemy>();
            _enemyCount = new List<int>();
            
            foreach (var value in enemies)
            {
                _enemies.Add(value);
            }

            foreach (var value in enemyCount)
            {
                _enemyCount.Add(value);
            }
        }
        
        public List<Enemy> GetEnemies()
        {
            return new List<Enemy>(_enemies);
        }

        public List<int> GetEnemyCount()
        {
            return new List<int>(_enemyCount);
        }
        
        private int _delayTimes;
        public int Level => _level;
        private int _level;

        public void SetTime(int delayTime)
        {
            _delayTimes=delayTime;
        }

        public int GetTime(int index)
        {
            return _delayTimes;
        }
    }
}