using System.Collections;
using System.Collections.Generic;
using Data;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryWarriors.Enemies;
using Service.Audio;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Logic.WaveManagment
{
    [RequireComponent(typeof(WaveManager))]
    [RequireComponent(typeof(WaveSpawner))]
    [RequireComponent(typeof(EnemyFactory))]
    public class WaveManager : MonoCache
    {
        private WaveSpawner _waveSpawner;
        [SerializeField] List<Wave> _waves = new();
        private int currentWaveIndex = 0;
        private bool isSpawningWave = false;
        private bool isWaitingForNextWave = false;
        private bool canStartNextWave = true;

        [SerializeField] public float TimeBetweenWaves;

        private List<Enemy> enemies = new List<Enemy>();

        // Флаг, разрешающий начало новой волны
        public Wave CurrentWave => _waves[currentWaveIndex];
        public int CurrentWaveIndex => currentWaveIndex;
        public int TotalWaves => _waves.Count;
        public UnityAction OnReadySpawning;
        private SaveLoadService _saveLoadService;


        public void Initialize(SaveLoadService saveLoadService, AudioManager audioManager)
        {
            _saveLoadService = saveLoadService;
            
            _waveSpawner = GetComponent<WaveSpawner>();
            _waveSpawner.Initialize(audioManager);
            _waveSpawner.OnSpawnPointsReady += OnWaveSpawningCompleted;
            _waveSpawner.OnSpawnPointsReady += OnWaveSpawnerReady;
            _saveLoadService.OnClearSpawnData += ClearData;
        }
        
        public IEnumerator SetWaveData()
        {
            yield return new WaitForSeconds(TimeBetweenWaves);

            while (currentWaveIndex <= _waves.Count)
            {
                if (!isSpawningWave && !isWaitingForNextWave && canStartNextWave)
                {
                    isSpawningWave = true;
                    _waveSpawner.SetWaveData(CurrentWave);
                    canStartNextWave = false;
                }

                yield return null;
            }
        }

        public void StartSpawn()
        {
            _waveSpawner.OnStartSpawn();
        }


        private void OnWaveSpawnerReady()
        {
            OnReadySpawning?.Invoke();
        }


        private void OnWaveSpawningCompleted()
        {
            isSpawningWave = false;
            currentWaveIndex++;
            
            Debug.Log(currentWaveIndex);
            if (currentWaveIndex >= _waves.Count)
            {
                Debug.Log("All in Queue completed!");
            }
            else
            {
                isWaitingForNextWave = true;
                StartCoroutine(WaitForNextWave());
            }
        }

        private IEnumerator WaitForNextWave()
        {
            yield return new WaitForSeconds(TimeBetweenWaves);
            isWaitingForNextWave = false;
            canStartNextWave = true;
        }

        public WaveSpawner GetWaveSpawner() => _waveSpawner;

        public void StopSpawn()
        {
            _waveSpawner.StopSpawn();
        }


        public SaveLoadService GetSaveLoad()
        {
            return _saveLoadService;
        }

        private void ClearData()
        {
            canStartNextWave = true;
            currentWaveIndex = 0;
            isWaitingForNextWave = false;
            isSpawningWave = false;
            List<int> countEnemies = new List<int>(){200};
            CurrentWave.AddData(CurrentWave.GetEnemies(),countEnemies);
        }
    }
}