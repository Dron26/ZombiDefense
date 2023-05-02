using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
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
            [SerializeField] private List<SpawnPoint> _spawnPoints;
            private List<Enemy> _enemys = new();
            private List<Enemy> _currentEnemys = new();
             private List<int> enemyCounts= new();
            private List<WaveQueue> _groupWaveQueue = new();
            private WaveQueue _waveQueue = new WaveQueue();
            private IEnumerator _spawnCoroutine;
            public UnityAction SpawningCompleted;
            
            
            public void Initialize(WaveData waveData)
            {
                CreateWaveQueue(waveData);
            }
            
            private void CreateWaveQueue(WaveData waveData)
            {
                KeyValuePair<List<Enemy>, List<int>> pair = waveData.GetParticipatingEnemy().FirstOrDefault();
                
                foreach ( Enemy enemy in pair.Key)
                {
                    _enemys.Add(enemy);
                }
                
                foreach ( int  count in pair.Value)
                {
                    enemyCounts.Add(count);
                }

                for (int i = 0; i < _enemys.Count; i++)
                {
                    WaveQueue waveQueue = new WaveQueue();
                    _groupWaveQueue.Add(waveQueue);
                }
            }

            public void StartSpawn()
            {
                if (_spawnCoroutine != null)
                    StopCoroutine(_spawnCoroutine);
            
                _spawnCoroutine = FillQueueCoroutine();
                StartCoroutine(_spawnCoroutine);

                InitializeSpawnPoint();
            }

            public void StopSpawn()
            {
                if (_spawnCoroutine != null)
                    StopCoroutine(_spawnCoroutine);
            }

            private IEnumerator FillQueueCoroutine()
            {
                for (int i = 0; i < _enemys.Count; i++)
                {
                    var spawnPoint = GetRandomSpawnPoint();// добавить точки спавна
                    for (int j = 0; j < enemyCounts.Count; j++)
                    {
                        Enemy newEnemy= _enemyFactory.Create(_enemys[i],spawnPoint);
                        
                        _groupWaveQueue[i].Enqueue(newEnemy);
                        newEnemy.OnEnemyDeath += OnEnemyDeath;
                        _currentEnemys.Add(newEnemy);
                        yield return null;
                    }
                }

                SpawningCompleted.Invoke();
            }
            
           
            private Transform GetRandomSpawnPoint()
            {
                Transform index = _spawnPoints[0].gameObject.transform;
               return index;
            }

            private void OnEnemyDeath(Enemy enemy)
            {
                enemy.OnEnemyDeath -= OnEnemyDeath;
                _waveQueue.Enqueue(enemy);
            }


            
            private void OnStartSpawn()
            {

                int nemberQueue = 0;
                
                for (int i = 0; i < _spawnPoints.Count; i++)
                {
                    _groupWaveQueue[i].Dequeue().gameObject.transform.position = _spawnPoints[i].transform.position;
                }
                
            }

            private void InitializeSpawnPoint()
            {
                for (int i = 0; i < _spawnPoints.Count; i++)
                {
                    _spawnPoints[i].Initialize(i,0);
                }
            }

            public List<Enemy> GetEnemyInWaveQueue( )
            {
                return _currentEnemys;
            }


            public void SetHumanoidData(List<Humanoid> humanoid)
            {
                _enemyFactory.SetHumanoidData(humanoid);
            }

            public EnemyFactory GetEnemyFactory() => _enemyFactory;
        }

}