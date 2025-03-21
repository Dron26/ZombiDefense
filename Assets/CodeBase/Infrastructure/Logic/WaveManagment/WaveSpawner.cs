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
using Services.SaveLoad;
using UnityEngine;
using EnemyData = Enemies.EnemyData;
using Random = System.Random;

namespace Infrastructure.Logic.WaveManagment
{
    public class WaveSpawner : MonoCache
    {
        [SerializeField] private GameObject _spawnPointGroup;

        public Action OnSpawnPointsReady;
        public Action OnStartedWave;
        public Action OnCompletedWave;
        public Action<Enemy> OnActivedCharacter;

        private WaveManager _waveManager;
        private AudioManager _audioManager;
        private Dictionary<int, List<Enemy>> CreatedWave;
        private List<List<Enemy>> _createdWave = new();
        private List<int> _activatedWavesNumber = new();
        private List<SpawnPoint> _spawnPoints = new();
        private EnemyFactory _enemyFactory;
        private int _maxEnemyOnLocation = new();
        private int _currentIndexEnemyOnWave;
        private List<Enemy> _inactiveEnemys = new();
        private int _numberKilledEnemies;
        public List<int> _enemyCount;
        //private float _cycleTimer;
        //private float _cycleDuration;
        private int _countActivatedEnemiesWave;
        private int _maxEnemyOnWave;
        private int _currentIndexWave;
        private float _stepDelayTime;
        private bool _isStopSpawning;
        private bool _isStopRevival = false;
        private int _spawnPointsCount ;
        private List<float> _spawnPointDelayTime = new();
        private int _delayTimeCount=4;
        public int TimeTimeBeforeNextWave => _delayTimeCount;
        Random _systemRandom = new Random();
        private IEnemyHandler _enemyHandler;
        private ISearchService _searchService;
        private ILocationHandler _locationHandler;
        private IGameEventBroadcaster _gameEvent;

        public void Initialize(AudioManager audioManager, EnemyFactory enemyFactory, WaveManager waveManager)
        {
            _enemyFactory = enemyFactory;
            _audioManager = audioManager;
            _waveManager = waveManager;
            _stepDelayTime = 0.73f;
            _enemyHandler  = AllServices.Container.Single<IEnemyHandler>();
            _searchService  = AllServices.Container.Single<ISearchService>();
            _locationHandler= AllServices.Container.Single<ILocationHandler>();
            _gameEvent= AllServices.Container.Single<IGameEventBroadcaster>();
            AddListener();
            
            CreatedWave = new Dictionary<int, List<Enemy>>();
        }

        

        private void SetData(WaveData waveData)
        {
            Debug.Log("SetWaveData");
            ClearData();
            SetSpawnPoint();
            SetWave(waveData);
        }
        
        private void SetWave(WaveData waveData)
        {
            _enemyCount= waveData.EnemyCount;
            _spawnPointsCount = _spawnPoints.Count;
            StartFillPool(waveData);
            FillTimeQueue();
            _currentIndexWave++ ;
        }

        private void FillTimeQueue()
        {
            
            for (int i = 0; i < _delayTimeCount; i++)
            {
                float j = _systemRandom.Next(0, _delayTimeCount) / 2.0f; 
                _spawnPointDelayTime.Add(_systemRandom.Next(0, _spawnPoints.Count)+j);
            }
        }

        private void SetSpawnPoint()
        {
            foreach (SpawnPoint point in _spawnPointGroup.transform.GetComponentsInChildren<SpawnPoint>())
            {
                _spawnPoints.Add(point);
            }
        }

        private void StartFillPool(WaveData waveData)
        {
            Debug.Log("StartFillPool");
            _maxEnemyOnWave = 0;
            _createdWave.Add(new List<Enemy>());
            CreatedWave.Add(CreatedWave.Count,new List<Enemy>());
            _currentIndexEnemyOnWave = CreatedWave.Count-1;
            _activatedWavesNumber.Add(CreatedWave.Count-1);
            List<EnemyData> enemies = waveData.Enemies;
            
            for (int i = 0; i < enemies.Count; i++)
            {
                int count = 0;
                
                for (int j = 0; j < _spawnPointsCount; j++)
                {
                    Enemy newEnemy = _enemyFactory.Create(enemies[i].Type);
                    newEnemy.OnEntityDeath += OnEntityDeath;
                    PreparEnemy(newEnemy, _spawnPoints[j]);
                    CreatedWave[_currentIndexEnemyOnWave].Add(newEnemy);
                    count++;

                    if (count == waveData.EnemyCount[i])
                    {
                        
                        _maxEnemyOnWave += count;
                        break;
                    }

                    if (count== _spawnPointsCount)
                    {
                        j = 0;
                    }
                }
            }

            _enemyHandler.SetMaxEnemyOnWave(_maxEnemyOnWave);
            _numberKilledEnemies = 0;
            OnSpawnPointsReady?.Invoke();
        }

        private void OnEntityDeath(Entity entity)
        { 
            Enemy enemy =entity.GetComponent<Enemy>();
            _enemyHandler.EnemyDeath(enemy);
            _numberKilledEnemies++;
            
            if (_numberKilledEnemies == _maxEnemyOnLocation)
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
            StartCoroutine(StartSpawn());
        }
        public IEnumerator StartSpawn()
        {
            int number = 0;

            _isStopSpawning = false;
            _countActivatedEnemiesWave = 0;
            
           
            foreach (Enemy enemy in CreatedWave[_createdWave.Count-1])
            {
                if (_isStopSpawning == false)
                {
                    enemy.transform.localPosition = enemy.StartPosition;
                    float i = _systemRandom.Next(0, _delayTimeCount) / 2.0f; 
                    yield return new WaitForSeconds(i);
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
            _maxEnemyOnLocation = _enemyHandler.GetActiveEnemy().Count;
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
                OnActivedCharacter?.Invoke(enemy);
                
                if (_activatedWavesNumber[index] == CreatedWave[index].Count)
                {
                    StopRevival(index);
                }
            }
        }

        public void StopSpawn()
        {
            _isStopSpawning = true;
            StopCoroutine(StartSpawn());
            StopRevival(CreatedWave.Count-1);
        }

        private void OnEnemyRevival(Enemy enemy)
        {
            Activated(enemy);
        }

        private void StopRevival(int index)
        {
            _isStopRevival = true;

            foreach (Enemy enemy in CreatedWave[index])
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
            for (int i = 0; i < CreatedWave.Count; i++)
            {
                if (CreatedWave[i] != null)
                {
                    for (int j = 0; j <  CreatedWave[i].Count; j++)
                    {
                        CreatedWave[i][j].GetComponent<EnemyDieState>().SetDestroyed();
                    }
                
                    CreatedWave[i].Clear();
                }
            }
            
            // foreach (var enemies in CreatedWave)
            // {
            //     if (enemies.Count > 0)
            //     {
            //         foreach (var enemy in enemies)
            //         {
            //             enemy.GetComponent<EnemyDieState>().SetDestroyed();
            //         }
            //     }
            // }
            
            // _createdWave = new();
        }

        private void AddListener()
        {
            _waveManager.OnSetWave += SetData;
            _waveManager.StartSpawn += OnStartSpawn;
            // _saveLoadService.OnSetInactiveEnemy += OnSetInactiveEnemy;
        }
    }
}