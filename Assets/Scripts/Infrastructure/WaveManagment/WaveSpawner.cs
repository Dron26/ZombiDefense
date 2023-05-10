using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using GameObject = UnityEngine.GameObject;

namespace Infrastructure.WaveManagment
{
    public class WaveSpawner : MonoCache
    {
        [SerializeField] private WaveManager _waveManager;
        [SerializeField] private EnemyFactory _enemyFactory;
        private List<SpawnPoint> _spawnPoints = new();
        private List<Enemy> _enemys = new();
        private List<Enemy> _activeEnemys = new();
        private List<Enemy> _inactiveEnemys = new();
        private List<int> enemyCounts = new();
        private List<WaveQueue> _groupWaveQueue = new();
        private WaveQueue _waveQueue = new();
        private IEnumerator _spawnCoroutine;
        public UnityAction SpawningCompleted;
        private int _totalNumber;

        public void Initialize(WaveData waveData)
        {
            CreateWaveQueue(waveData);
            InitializeSpawnPoint();
            FillQueue();
        }

        private void CreateWaveQueue(WaveData waveData)
        {
            KeyValuePair<List<Enemy>, List<int>> pair = waveData.GetParticipatingEnemy().FirstOrDefault();

            WaveQueue waveQueue = new WaveQueue();
            _groupWaveQueue.Add(waveQueue);
            _enemys.Add(pair.Key[0]);

            for (int i = 1; i < pair.Key.Count; i++)
            {
                if (pair.Key[i].Level != pair.Key[i - 1].Level)
                {
                    _enemys.Add(pair.Key[i]);
                    waveQueue = new WaveQueue();
                    _groupWaveQueue.Add(waveQueue);
                }
            }
            
            foreach (int count in pair.Value)
            {
                enemyCounts.Add(count);
                _totalNumber += count;
            }
        }
        
        private void InitializeSpawnPoint()
        {
            int i = 0;
            foreach (SpawnPoint point in transform.GetComponentsInChildren<SpawnPoint>())
            {
                point.Initialize(i, 0);
                _spawnPoints.Add(point);
                i++;
            }
        }

        public void StopSpawn()
        {
            foreach ( SpawnPoint spawnPoint in _spawnPoints)
            {
                spawnPoint.StopSpawn();
            }
            
            if (_spawnCoroutine != null)
                StopCoroutine(_spawnCoroutine);
        }

        private void FillQueue()
        {
            foreach (WaveQueue waveQueue in _groupWaveQueue)
            {
                for (int i = 0; i < _enemys.Count; i++)
                {
                    for (int j = 0; j < enemyCounts[i]; j++)
                    {
                        for (int k = 0; k <= _spawnPoints.Count; k++)
                        {
                            Enemy newEnemy = _enemyFactory.Create(_enemys[i].gameObject);

                            if (newEnemy != null)
                            {
                                newEnemy.Load += OnEnemyLoaded;
                                EnemyDieState enemyDieState = newEnemy.GetComponent<EnemyDieState>();
                                enemyDieState.OnDeath += OnDeath;
                                waveQueue.Enqueue(newEnemy);
                            }
                        }
                    }
                }
            }
        }

        private void OnEnemyLoaded(Enemy enemy)
        {
            _activeEnemys.Add(enemy);

            if (_activeEnemys.Count == _totalNumber)
            {
                SpawningCompleted?.Invoke();
                OnStartSpawn();
            }
        }

        private void OnDeath(Enemy enemy)
        {
            _waveQueue.Enqueue(enemy);
            _inactiveEnemys.Add(enemy);
            _activeEnemys.Remove(enemy);
        }

        private void OnStartSpawn()
        {
            int nemberQueue = 0;

            foreach (WaveQueue waveQueue in _groupWaveQueue)
            {
                foreach (SpawnPoint spawnPoint in _spawnPoints)
                {
                    spawnPoint.SetQueue(waveQueue);
                }
            }
        }

       

        public List<Enemy> GetEnemyInWaveQueue()
        {
            return _activeEnemys;
        }


        public void SetHumanoidData(List<Humanoid> humanoid)
        {
            _enemyFactory.SetHumanoidData(humanoid);
        }

        public EnemyFactory GetEnemyFactory() => _enemyFactory;

        private void OnDisable()
        {
            foreach (Enemy enemy in _activeEnemys)
            {
                EnemyDieState enemyDieState = enemy.GetComponent<EnemyDieState>();
                enemyDieState.OnDeath -= OnDeath;
            }
        }
    }
}