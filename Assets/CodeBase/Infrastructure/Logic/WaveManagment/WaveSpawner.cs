using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.StateMachines.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryWarriors.Enemies;
using Interface;
using Services;
using Services.Audio;
using UnityEngine;
using EnemyData = Enemies.EnemyData;

namespace Infrastructure.Logic.WaveManagment
{
    public class WaveSpawner : MonoCache
    {
        [SerializeField] private GameObject _spawnPointGroup;

        public event Action OnSpawnPointsReady;
        public event Action OnStartedWave;
        public event Action OnCompletedWave;

        private WaveManager _waveManager;
        private Dictionary<int, List<Enemy>> _createdWaveDict;
        private List<int> _activatedWavesNumber = new();
        private List<SpawnPoint> _spawnPoints = new();
        private EnemyFactory _enemyFactory;
        private int _currentIndexEnemyOnWave;
        private int _numberKilledEnemies;
        public List<int> _enemyCount;
        //private float _cycleTimer;
        //private float _cycleDuration;
        private int _countActivatedEnemiesWave;
        private int _maxEnemyOnWave;
        private bool _isStopSpawning;
        private bool _isStopRevival = false;
        private int _spawnPointsCount ;
        private int _delayTimeCount=4;
        public int TimeTimeBeforeNextWave => _delayTimeCount;
        private IEnemyHandler _enemyHandler;
        private ISearchService _searchService;
        private ILocationHandler _locationHandler;
        private IGameEventBroadcaster _gameEvent;
        private Coroutine _spawnCoroutine;
        public void Initialize(AudioManager audioManager, EnemyFactory enemyFactory, WaveManager waveManager)
        {
            _enemyFactory = enemyFactory;
            _waveManager = waveManager;
            _enemyHandler  = AllServices.Container.Single<IEnemyHandler>();
            _searchService  = AllServices.Container.Single<ISearchService>();
            _locationHandler= AllServices.Container.Single<ILocationHandler>();
            _gameEvent= AllServices.Container.Single<IGameEventBroadcaster>();
            AddListener();
            
            _createdWaveDict = new Dictionary<int, List<Enemy>>();
        }
        
        private void SetData(WaveData waveData)
        {
            Debug.Log("SetWaveData");
            ClearData();
            InitializeSpawnPoints();
            SetWave(waveData);
        }
        
        private void SetWave(WaveData waveData)
        {
            _enemyCount= waveData.EnemyCount;
            _spawnPointsCount = _spawnPoints.Count;
            StartFillPool(waveData);
        }

        private void InitializeSpawnPoints()
        {
            _spawnPoints = new List<SpawnPoint>(_spawnPointGroup.GetComponentsInChildren<SpawnPoint>());
        }

        private void StartFillPool(WaveData waveData)
        {
            Debug.Log("StartFillPool");
            _maxEnemyOnWave = 0;
    
            int waveIndex = _createdWaveDict.Count;
            _createdWaveDict[waveIndex] = new List<Enemy>();
            _activatedWavesNumber.Add(0);

            for (int i = 0; i < waveData.Enemies.Count; i++)
            {
                EnemyData enemyData = waveData.Enemies[i];
                int spawnCount = waveData.EnemyCount[i];
                int count = 0;
        
                for (int j = 0; j < _spawnPoints.Count && count < spawnCount; j++)
                {
                    Enemy newEnemy = _enemyFactory.Create(enemyData.Type);
                    newEnemy.OnEntityDeath += OnEntityDeath;
                    PreparEnemy(newEnemy, _spawnPoints[j]);

                    _createdWaveDict[waveIndex].Add(newEnemy);
                    count++;

                    if (count == spawnCount)
                        break;
                }

                _maxEnemyOnWave += spawnCount;
            }

            _enemyHandler.SetMaxEnemyOnWave(_maxEnemyOnWave);
            _numberKilledEnemies = 0;
            OnSpawnPointsReady?.Invoke();
        }

        private void OnEntityDeath(Entity entity)
        {
            Enemy enemy = entity.GetComponent<Enemy>();
            _enemyHandler.EnemyDeath(enemy);
            enemy.GetComponent<EnemyDieState>().OnRevival -= OnEnemyRevival; 

            _numberKilledEnemies++;
            if (_numberKilledEnemies == _maxEnemyOnWave)
            {
                EndWave();
            }
        }


        private void PreparEnemy(Enemy enemy, SpawnPoint spawnPoint)
        {
            enemy.gameObject.layer = LayerMask.NameToLayer("Character");
            enemy.GetComponent<EnemyDieState>().OnRevival += OnEnemyRevival;
            Transform spawnPointTransform = spawnPoint.transform;
            Transform enemyTransform = enemy.transform;
            enemyTransform.SetParent(spawnPoint.transform);
            enemy.StartPosition = spawnPointTransform.position;
            enemyTransform.position = enemy.StartPosition;
            enemy.SetIndex(_currentIndexEnemyOnWave);
            enemy.gameObject.SetActive(false);
        }

        public void OnStartSpawn()
        {
            _spawnCoroutine = StartCoroutine(StartSpawn());
        }
        public IEnumerator StartSpawn()
        {
            int number = 0;

            _isStopSpawning = false;
            _countActivatedEnemiesWave = 0;
            
           
            foreach (Enemy enemy in _createdWaveDict[_createdWaveDict.Count - 1])
            {
                if (_isStopSpawning == false)
                {
                    enemy.transform.localPosition = enemy.StartPosition;
                    
                    float delay = UnityEngine.Random.Range(0f, _delayTimeCount / 2f);
                    yield return new WaitForSeconds(delay);
                    Activated(enemy);
                    _countActivatedEnemiesWave++;

                    if (_countActivatedEnemiesWave == _maxEnemyOnWave)
                    {
                        StopSpawn();
                    }

                    number++;

                    if (number == _spawnPoints.Count)
                    {
                        number = 0;
                    }
                }
                else
                {
                    break;
                }
                    
                    
            }

            OnStartedWave?.Invoke();
            _numberKilledEnemies = 0;
            StopCoroutine(StartSpawn());
        }

     
        private void Activated(Enemy enemy)
        {
            if (_isStopSpawning == false)
            {
                enemy.transform.position = enemy.StartPosition;
                enemy.gameObject.SetActive(true);
                _enemyHandler.SetActiveEnemy(enemy);
                int index = enemy.IndexInWave;
                
                _activatedWavesNumber[index]++;
                _searchService.AddEntity(enemy);
                
                if (_activatedWavesNumber[index] == _createdWaveDict[index].Count)
                {
                    StopRevival(index);
                }
            }
        }

        public void StopSpawn()
        {
            _isStopSpawning = true;
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
            StopRevival(_createdWaveDict.Count - 1);
        }

        private void OnEnemyRevival(Enemy enemy)
        {
            Activated(enemy);
        }

        private void StopRevival(int index)
        {
            _isStopRevival = true;

            foreach (Enemy enemy in _createdWaveDict[index])
            {
                EnemyDieState enemyDieState = enemy.GetComponent<EnemyDieState>();
                enemyDieState.StopRevival(_isStopRevival);
            }
        }

        private void EndWave()
        {

            if (_waveManager.CurrentStartedWave == _waveManager.TotalWaves)
            {
                _locationHandler.LocationCompleted();
                _gameEvent.InvokeOnLocationCompleted(); 
            }
            
            if (_waveManager.CurrentFilledWave < _waveManager.TotalWaves)
                OnCompletedWave?.Invoke();
        }

        private void ClearData()
        {
            ClearEnemies();
            _spawnPoints.Clear();
            _isStopSpawning = false;
        }

        private void ClearEnemies()
        {
            for (int i = 0; i < _createdWaveDict.Count; i++)
            {
                if (_createdWaveDict[i] != null)
                {
                    for (int j = 0; j <  _createdWaveDict[i].Count; j++)
                    {
                        _createdWaveDict[i][j].GetComponent<EnemyDieState>().SetDestroyed();
                    }
                
                    _createdWaveDict[i].Clear();
                }
            }
            
        }

        private void AddListener()
        {
            _waveManager.OnSetWave += SetData;
            _waveManager.StartSpawn += OnStartSpawn;
            // _saveLoadService.OnSetInactiveEnemy += OnSetInactiveEnemy;
        }
    }
}