using System.Collections.Generic;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using UnityEngine;

namespace Infrastructure.WaveManagment
{
    [System.Serializable]
    public class WaveData:MonoCache
    {
        [SerializeField] private List<Enemy> _enemyDatas;
        [SerializeField] private List<int> enemyCounts;

        public List<Enemy> EnemyDatas => _enemyDatas;
        public List<int> EnemyCounts => enemyCounts;

        public int Count => _enemyDatas.Count;

        public WaveData(List<Enemy> enemyDatas, List<int> enemyCounts)
        {
            this._enemyDatas = enemyDatas;
            this.enemyCounts = enemyCounts;
        }
    }
}