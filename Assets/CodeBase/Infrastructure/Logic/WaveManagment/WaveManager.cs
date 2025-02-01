using System;
using System.Collections.Generic;
using Data;
using Enemies.AbstractEntity;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryWarriors.Enemies;
using Infrastructure.Logic.Inits;
using Interface;
using Services;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Logic.WaveManagment
{
    [RequireComponent(typeof(WaveManager))]
    [RequireComponent(typeof(WaveSpawner))]
    [RequireComponent(typeof(EnemyFactory))]
    public class WaveManager : MonoCache
    {
        public Action<WaveData> OnSetWave;
        public Action StartSpawn;
        private EnemyFactory _enemyFactory;
        private WaveSpawner _waveSpawner;
        private int _currentFilledWave = 0;
        private bool isWaitingForNextWave = false;
        public WaveSpawner WaveSpawner => _waveSpawner;
        private List<Enemy> enemies = new List<Enemy>();
        private bool _isContinueGame;
        private bool _isStartedWave;
        private bool _canStartWave;
        private bool _canFillWave;
        public int CurrentFilledWave => _currentFilledWave;
        public int CurrentStartedWave => _currentStartedWave;
        public int TotalWaves => _wavesContainerData.GroupWaveData.Count;
        private int _currentStartedWave;
        public UnityAction OnReadySpawning;
        private SceneInitializer _sceneInitializer;
        private WavesContainerData _wavesContainerData;
        private ILocationHandler _locationHandler;
        public List<WaveData> _groupWaveData;
        public List<List<int>> _groupEnemyCount;
        private IGameEventBroadcaster _eventBroadcaster;

        private void InitializeContainer()
        {
            _locationHandler = AllServices.Container.Single<ILocationHandler>();
            string path =AssetPaths.WavesContainerData + _locationHandler.SelectedLocationId;
            _wavesContainerData = Resources.Load<WavesContainerData>(path);
            _groupWaveData=_wavesContainerData.GroupWaveData;;
        }

        private void Awake()
        {
            _waveSpawner = GetComponent<WaveSpawner>();
            _enemyFactory = GetComponent<EnemyFactory>();
        }

        public void Initialize(SceneInitializer sceneInitializer)
        {
            _sceneInitializer = sceneInitializer;
            _eventBroadcaster=AllServices.Container.Single<IGameEventBroadcaster>(); 

            _enemyFactory.Initialize( _sceneInitializer.GetAudioController());
            _waveSpawner.Initialize(_sceneInitializer.GetAudioController(), _enemyFactory, this);
            _canFillWave = true;
            
            AddListener();
            
            InitializeContainer();
            SetMaxCountEnemy();
        }
        
        public void SetWaveData()
        {
            
            if (_canFillWave)
            {
                OnSetWave?.Invoke(_groupWaveData[_currentFilledWave]);
            }
        }

        public void Spawn()
        {
            if (!_isStartedWave&_canStartWave)
            {
                _canStartWave = false;
                StartSpawn?.Invoke();
            }
        }

        private void SetMaxCountEnemy()
        {
            int count = 0;
            foreach (var group in _groupWaveData)
            {
                foreach (int countEnemy in group.EnemyCount)
                {
                    count+= countEnemy;
                }
            }
            
            _locationHandler.SetMaxEnemyOnScene(count);
        }
        
        public void StopSpawn()
        {
            _waveSpawner.StopSpawn();
        }

        private void OnWaveFilled()
        {
            _currentFilledWave++;
            if (_currentFilledWave == 1) OnReadySpawning?.Invoke();

            SetPossibilityFillWave();
            _isStartedWave = false;
            _canStartWave = true;
        }

       

        private void SetPossibilityFillWave()
        {
            _canFillWave = _currentFilledWave != _groupWaveData.Count;
        }

       

        private void AddListener()
        {
            _waveSpawner.OnSpawnPointsReady += OnWaveFilled;
            _waveSpawner.OnCompletedWave += CompletedWave;
            _waveSpawner.OnStartedWave += OnStartedWave;
            _sceneInitializer.OnClickContinue += SetContinue;
            _eventBroadcaster.LastEnemyRemained += SetWaveData;
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
            // if (_currentStartedWave!<_currentFilledWave)
            // {
            //     Spawn();
            // }
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
            _eventBroadcaster.LastEnemyRemained -= SetWaveData;
        }
    }
}