using System.Collections;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.WaveManagment
{
    // public class WaveSpawner : MonoCache
    // {
    //     [SerializeField] private WaveManager _waveManager;
    //     [SerializeField] private EnemyFactory _enemyFactory;
    //     [SerializeField] private Transform[] _spawnPoints;
    //
    //     public UnityAction SpawningCompleted;
    //     private IEnumerator _spawnCoroutine;
    //
    //     public void StartSpawn()
    //     {
    //         if (_spawnCoroutine != null)
    //             StopCoroutine(_spawnCoroutine);
    //
    //         _spawnCoroutine = SpawnCoroutine();
    //         StartCoroutine(_spawnCoroutine);
    //     }
    //
    //     public void StopSpawn()
    //     {
    //         if (_spawnCoroutine != null)
    //             StopCoroutine(_spawnCoroutine);
    //     }
    //
    //     private IEnumerator SpawnCoroutine()
    //     {
    //         for (int i = 0; i < _waveManager.CurrentWave.EnemyCounts[_waveManager.CurrentWaveIndex]; i++)
    //         {
    //             var enemyData = _waveManager.CurrentWave.EnemyDatas[_waveManager.CurrentWaveIndex];
    //             var spawnPoint = GetRandomSpawnPoint();
    //             _enemyFactory.CreateEnemy(enemyData, spawnPoint);
    //             yield return new WaitForSeconds(1f);
    //         }
    //
    //         SpawningCompleted.Invoke();
    //     }
    //
    //     private Transform GetRandomSpawnPoint()
    //     {
    //         var index = Random.Range(0, _spawnPoints.Length);
    //         return _spawnPoints[index];
    //     }
    // }
    public class WaveSpawner : MonoCache
        {
            [SerializeField] private WaveManager _waveManager;
            [SerializeField] private EnemyFactory _enemyFactory;
            [SerializeField] private Transform[] _spawnPoints;

            private WaveQueue _waveQueue = new WaveQueue();
            private IEnumerator _spawnCoroutine;

            public UnityAction SpawningCompleted;

            
            public void StartSpawn()
            {
                if (_spawnCoroutine != null)
                    StopCoroutine(_spawnCoroutine);
            
                _spawnCoroutine = SpawnCoroutine();
                StartCoroutine(_spawnCoroutine);
            }

            public void StopSpawn()
            {
                if (_spawnCoroutine != null)
                    StopCoroutine(_spawnCoroutine);
            }

            private IEnumerator SpawnCoroutine()
            {
                for (int i = 0; i < _waveManager.CurrentWave.EnemyCounts[_waveManager.CurrentWaveIndex]; i++)
                {
                    Enemy enemy = _waveManager.CurrentWave.EnemyDatas[_waveManager.CurrentWaveIndex];
                    var spawnPoint = GetRandomSpawnPoint();
                    Enemy newEnemy= _enemyFactory.CreateEnemy(enemy,spawnPoint);
                    _waveQueue.Enqueue(enemy);
                    enemy.OnEnemyDeath += OnEnemyDeath;
                    yield return new WaitForSeconds(1f);
                }
            
                SpawningCompleted.Invoke();
            }
           

            private Transform GetRandomSpawnPoint()
            {
                var index = Random.Range(0, _spawnPoints.Length);
                return _spawnPoints[index];
            }

            private void OnEnemyDeath(Enemy enemy)
            {
                enemy.OnEnemyDeath -= OnEnemyDeath;
                _waveQueue.Enqueue(enemy);
            }
    }

}