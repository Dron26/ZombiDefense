using System;
using System.Collections.Generic;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryWarriors.Enemies;
using Infrastructure.Logic.Inits;
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
        [SerializeField] private List<int> _timesBetweenWaves;
        [SerializeField] private List<Wave> _waves;
        public Action<Wave> OnSetWave;
        public Action OnStartSpawn;
        private EnemyFactory _enemyFactory;
        private WaveSpawner _waveSpawner;
        private int currentWaveIndex = 0;
        private bool isWaitingForNextWave = false;
        private bool canStartNextWave = true;

        private int TimeBetweenWaves => _timesBetweenWaves[currentWaveIndex];
        private List<Enemy> enemies = new List<Enemy>();
        private bool _isContinueGame;
        public Wave CurrentWave => _waves[currentWaveIndex];
        public int CurrentWaveIndex => currentWaveIndex;
        public int TotalWaves => _waves.Count;
        public UnityAction OnReadySpawning;
        private SaveLoadService _saveLoadService;
        private SceneInitializer _sceneInitializer;

        public void Initialize(SaveLoadService saveLoadService,SceneInitializer sceneInitializer)
        {
            _sceneInitializer = sceneInitializer;
            _saveLoadService = saveLoadService;
            _saveLoadService.SetTimeBeforeNextWave(TimeBetweenWaves);
            _enemyFactory = GetComponent<EnemyFactory>();
            _enemyFactory.Initialize(_saveLoadService, _sceneInitializer.GetAudioController());
            _waveSpawner = GetComponent<WaveSpawner>();
            _waveSpawner.Initialize(_sceneInitializer.GetAudioController(), _enemyFactory, this);
            AddListener();
        }

        public void SetWaveData()
        {
            if (canStartNextWave)
            {
                canStartNextWave = false;
                OnSetWave?.Invoke(CurrentWave);
            }
        }

        public void StartSpawn()
        {
            OnStartSpawn?.Invoke();
        }

        private void OnWaveSpawningCompleted()
        {
            currentWaveIndex++;
            if (currentWaveIndex == 1) OnReadySpawning?.Invoke();

            canStartNextWave = true;
        }

        public void StopSpawn()
        {
            _waveSpawner.StopSpawn();
        }


        public SaveLoadService GetSaveLoad()
        {
            return _saveLoadService;
        }

        private void AddListener()
        {
            _waveSpawner.OnSpawnPointsReady += OnWaveSpawningCompleted;
            _waveSpawner.OnCompletedWave += CompletedWave;
            _sceneInitializer.OnClickContinue += SetContinue;
        }

        private void SetContinue()
        {
            canStartNextWave = true;
            currentWaveIndex = 0;
            isWaitingForNextWave = false;
            _isContinueGame=true;
            SetWaveData();
        }

        private void CompletedWave()
        {
            SetWaveData();
        }
    }
}