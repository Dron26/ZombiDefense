using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryWarriors.Enemies;
using Service.Audio;
using Service.SaveLoad;
using UI.Levels;
using UnityEngine;
using UnityEngine.Events;


namespace Infrastructure.Logic.WaveManagment
{
    public class WaveSpawner : MonoCache
    {
        [SerializeField] private GameObject _spawnPointGroup;

        public Action OnSpawnPointsReady;
        public Action OnStartedWave;
        public Action OnCompletedWave;

        private WaveManager _waveManager;
        private AudioManager _audioManager;
        private SaveLoadService _saveLoadService;
        private List<List<Enemy>> _createdWave = new();
        private List<int> _activatedWavesNumber = new();
        private List<SpawnPoint> _spawnPoints = new();
        private EnemyFactory _enemyFactory;
        private int _maxEnemyOnLocation = new();
        private int _currentIndexEnemyOnWave;
        private List<Enemy> _inactiveEnemys = new();
        private int NumberKilledEnemies => _saveLoadService.GetNumberKilledEnemies();
        private int _numberKilledEnemies;
        public List<int> _enemyCount;
        //private float _cycleTimer;
        // private float _cycleDuration;
        private int _countActivatedEnemiesWave;
        private int _maxEnemyOnWave;
        private int _currentIndexWave;
        private float _stepDelayTime;
        private bool _isStopSpawning;
        private bool _isStopRevival = false;
        private int _spawnPointsCount ;
        private List<float> _spawnPointDelayTime = new();
        private int DelayTimeCount=4;
        System.Random _systemRandom = new System.Random();
        public void Initialize(AudioManager audioManager, EnemyFactory enemyFactory, WaveManager waveManager)
        {
            _enemyFactory = enemyFactory;
            _audioManager = audioManager;
            _waveManager = waveManager;
            _saveLoadService = _waveManager.GetSaveLoad();
            _stepDelayTime = 0.73f;
            AddListener();
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
            
            for (int i = 0; i < DelayTimeCount; i++)
            {
                float j = _systemRandom.Next(0, DelayTimeCount) / 2.0f; 
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
            _currentIndexEnemyOnWave = _createdWave.Count-1;
            _activatedWavesNumber.Add(_createdWave.Count-1);
            List<EnemyData> enemies = waveData.Enemies;
            
            for (int i = 0; i < enemies.Count; i++)
            {
                int count = 0;
                
                for (int j = 0; j < _spawnPointsCount; j++)
                {
                    Enemy newEnemy = _enemyFactory.Create(enemies[i].Type);
                    newEnemy.OnDeath += OnEnemyDeath;
                    PreparEnemy(newEnemy, _spawnPoints[j]);
                    _createdWave[_currentIndexEnemyOnWave].Add(newEnemy);

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

            _numberKilledEnemies = 0;
            OnSpawnPointsReady?.Invoke();
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
            
           
                foreach (Enemy enemy in _createdWave[_createdWave.Count-1])
                {
                    if (_isStopSpawning == false)
                    {
                        enemy.transform.localPosition = enemy.StartPosition;
                        float i = _systemRandom.Next(0, DelayTimeCount) / 2.0f; 
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
            _maxEnemyOnLocation = _saveLoadService.GetActiveEnemy().Count;
            _numberKilledEnemies = 0;
            StopCoroutine(StartSpawn());
        }

     
        private void Activated(Enemy enemy)
        {
            if (_isStopSpawning == false)
            {
                enemy.transform.position = enemy.StartPosition;
                enemy.gameObject.SetActive(true);
                _saveLoadService.SetActiveEnemy(enemy);

                int index = enemy.IndexInWave;
                
                _activatedWavesNumber[index]++;

               
                
                if (_activatedWavesNumber[index] == _createdWave[index].Count)
                {
                    StopRevival(index);
                }
            }
        }

        public void StopSpawn()
        {
            _isStopSpawning = true;
            StopCoroutine(StartSpawn());
                StopRevival(_createdWave.Count-1);
        }

        private void OnEnemyDeath(Enemy enemy)
        {
            _saveLoadService.EnemyDeath(enemy);
            _numberKilledEnemies++;
            
            if (_numberKilledEnemies == _maxEnemyOnLocation)
            {
                EndWave();
            }
        }

        private void OnEnemyRevival(Enemy enemy)
        {
            Activated(enemy);
        }

        private void StopRevival(int index)
        {
            _isStopRevival = true;

            foreach (Enemy enemy in _createdWave[index])
            {
                EnemyDieState enemyDieState = enemy.GetComponent<EnemyDieState>();
                enemyDieState.StopRevival(_isStopRevival);
            }
        }

        private void EndWave()
        {
            
            if (_waveManager.CurrentFilledWave == _waveManager.TotalWaves) 
                _saveLoadService.SetCompletedLocationId();
            
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
            foreach (var enemies in _createdWave)
            {
                foreach (var enemy in enemies)
                {
                    enemy.GetComponent<EnemyDieState>().SetDestroyed();
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