using System.Collections.Generic;
using System.Linq;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors;
using Infrastructure.FactoryWarriors.Enemies;
using UnityEngine;

namespace Infrastructure.WaveManagment
{
    [System.Serializable]
    public class WaveData:MonoCache
    {
        [SerializeField] private List<Enemy> _enemys;
        [SerializeField] private List<int> enemyCounts;
        [SerializeField] private  List<float> _delayTimes;
        public List<float> DelayTimes=>_delayTimes;
        public List<Enemy> Enemys => _enemys;
        public List<int> EnemyCounts => enemyCounts;
        Dictionary<List<Enemy>,List<int>> _info=new Dictionary<List<Enemy>, List<int>>();

        public int Count => _enemys.Count;

        private void SetParticipatingEnemy()
        {
            Dictionary<List<Enemy>,List<int>> info=new Dictionary<List<Enemy>, List<int>>();
            info.Add(_enemys,EnemyCounts);
            
            _info = info.OrderBy(x => 
                x.Key.OrderBy(e => e.Level)).ToDictionary(x =>x.Key, x => x.Value);
        }

        public Dictionary<List<Enemy>,List<int>> GetParticipatingEnemy()
        {
            return _info;
        }

        public void Initialize()
        {
            SetParticipatingEnemy();
        }
    }
}