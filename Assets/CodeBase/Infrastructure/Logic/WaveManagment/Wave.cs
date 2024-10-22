using System.Collections.Generic;
using Data;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;

namespace Infrastructure.Logic.WaveManagment
{
    public class Waveп : MonoCache
    {
        [SerializeField] private List<EnemyData> _enemies= new List<EnemyData>();
        [SerializeField] private List<int> _enemyCount;
        [SerializeField] private int _timeBetweenWaves;
         private WaveData _waveData;
        public int TimeBetweenWaves => _timeBetweenWaves;
        public void SetData(WaveData waveData)
        {
            _enemies = new List<EnemyData>();
            _enemyCount = new List<int>();
            
            foreach (var value in waveData.Enemies)
            {
                _enemies.Add(value);
            }

            foreach (var value in waveData.EnemyCount)
            {
                _enemyCount.Add(value);
            }
        }
        
        public List<EnemyData> GetEnemies()
        {
            return new List<EnemyData>(_enemies);
        }

        public List<int> GetEnemyCount()
        {
            return new List<int>(_enemyCount);
        }
    }
}