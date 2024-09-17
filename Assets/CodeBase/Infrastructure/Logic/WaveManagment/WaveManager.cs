using System;
using System.Collections.Generic;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryWarriors.Enemies;
using Infrastructure.Logic.Inits;
using Service.Audio;
using Service.SaveLoad;
using UI.Levels;
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
        private int _currentFilledWave = 0;
        private bool isWaitingForNextWave = false;

        private int TimeBetweenWaves => _timesBetweenWaves[_currentFilledWave];
        private List<Enemy> enemies = new List<Enemy>();
        private bool _isContinueGame;
        private bool _isStartedWave;
        private bool _canFillWave;
        public Wave CurrentWave => _waves[_currentFilledWave];
        public int CurrentFilledWave => _currentFilledWave;
        public int CurrentStartedWave => _currentStartedWave;
        public int TotalWaves => _waves.Count;
        private int _currentStartedWave;
        public UnityAction OnReadySpawning;
        private SaveLoadService _saveLoadService;
        private SceneInitializer _sceneInitializer;

        public void Initialize(SaveLoadService saveLoadService, SceneInitializer sceneInitializer)
        {
            _sceneInitializer = sceneInitializer;
            _saveLoadService = saveLoadService;
            _saveLoadService.SetTimeBeforeNextWave(TimeBetweenWaves);
            _enemyFactory = GetComponent<EnemyFactory>();
            _enemyFactory.Initialize(_saveLoadService, _sceneInitializer.GetAudioController());
            _waveSpawner = GetComponent<WaveSpawner>();
            _waveSpawner.Initialize(_sceneInitializer.GetAudioController(), _enemyFactory, this);
            _canFillWave = true;
            SetMaxCountEnemy();
            AddListener();
        }

        private void SetMaxCountEnemy()
        {
            int count = 0;
            foreach (var wave in _waves)
            {
                foreach (int countEnemy in wave.GetEnemyCount())
                {
                    count+= countEnemy;
                }
            }
            
            _saveLoadService.SetMaxEnemyOnScene(count);
        }
        
        public void SetWaveData()
        {
            if (_canFillWave)
            {
                OnSetWave?.Invoke(CurrentWave);
            }
        }

        public void StartSpawn()
        {
            if (!_isStartedWave)
            {
                OnStartSpawn?.Invoke();
            }
        }

        private void OnWaveFilled()
        {
           
            
            _currentFilledWave++;
            if (_currentFilledWave == 1) OnReadySpawning?.Invoke();

            SetPossibilityFillWave();
            _isStartedWave = false;
        }

        public void StopSpawn()
        {
            _waveSpawner.StopSpawn();
        }

        private void SetPossibilityFillWave()
        {
            _canFillWave = _currentFilledWave != _waves.Count;
        }

        public SaveLoadService GetSaveLoad()
        {
            return _saveLoadService;
        }

        private void AddListener()
        {
            _waveSpawner.OnSpawnPointsReady += OnWaveFilled;
            _waveSpawner.OnCompletedWave += CompletedWave;
            _waveSpawner.OnStartedWave += OnStartedWave;
            _sceneInitializer.OnClickContinue += SetContinue;
            _saveLoadService.LastEnemyRemained += SetWaveData;
        }

        public void OnStartedWave()
        {
            _currentStartedWave++;
            _isStartedWave = true;
            SetPossibilityFillWave();
        }

        private void SetContinue()
        {
            _currentFilledWave = 0;
            isWaitingForNextWave = false;
            _isContinueGame=true;
            SetWaveData();
        }

        private void CompletedWave()
        {
            if (_currentStartedWave!<_currentFilledWave)
            {
                StartSpawn();
            }
        }


        protected override void OnDisabled()
        {
            RemoveListener();
        }

        private void RemoveListener()
        {
            _waveSpawner.OnSpawnPointsReady -= OnWaveFilled;
            _waveSpawner.OnCompletedWave -= CompletedWave;
            _sceneInitializer.OnClickContinue -= SetContinue;
            _saveLoadService.LastEnemyRemained -= SetWaveData;
        }
    }
}