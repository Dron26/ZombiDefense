using System;
using System.Collections.Generic;
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
        private List<Wave> _groupWave = new();
        private EnemyFactory _enemyFactory;
        private List<List<Enemy>> _createdEnemies = new();
        private List<int> _activatedEnemies = new();
        private int _maxEnemyOnLocation = new();
        private int _currentIndexEnemyNumber;

        private List<SpawnPoint> _spawnPoints = new();
        private List<Enemy> _inactiveEnemys = new();
        private int NumberKilledEnemies => _saveLoadService.GetNumberKilledEnemies();
        private int _numberKilledEnemies;

        //private float _cycleTimer;
        // private float _cycleDuration;
        private int _countActivatedEnemiesWave;
        private int _maxEnemyOnWave;
        private int _currentIndexWave;
        private float _stepDelayTime;
        private bool _isStopSpawning;
        private bool _isStopRevival = false;
        
        public void Initialize(AudioManager audioManager, EnemyFactory enemyFactory, WaveManager waveManager)
        {
            _enemyFactory = enemyFactory;
            _audioManager = audioManager;
            _waveManager = waveManager;
            _saveLoadService = _waveManager.GetSaveLoad();
            _stepDelayTime = 0.73f;
            AddListener();
        }

        private void SetData(Wave waveData)
        {
            Debug.Log("SetWaveData");
            ClearData();
            SetSpawnPoint();
            SetWave(waveData);
        }

        private void SetWave(Wave wave)
        {
            _groupWave.Add(wave);
            _currentIndexWave++ ;
            StartFillPool(wave);
        }

        private void SetSpawnPoint()
        {
            foreach (SpawnPoint point in _spawnPointGroup.transform.GetComponentsInChildren<SpawnPoint>())
            {
                _spawnPoints.Add(point);
            }
        }

        private void StartFillPool(Wave wave)
        {
            Debug.Log("StartFillPool");

            int x = wave.GetEnemies().Count;
            _maxEnemyOnWave = 0;
            _createdEnemies.Add(new List<Enemy>());
            _currentIndexEnemyNumber = _createdEnemies.Count-1;
            _activatedEnemies.Add(_createdEnemies.Count-1);
            for (int i = 0; i < x; i++)
            {
                
                
                
                int count = 0;


                for (int j = 0; j < _spawnPoints.Count; j++)
                {
                    Enemy newEnemy = _enemyFactory.Create(wave.GetEnemies()[i].gameObject);
                    PreparEnemy(newEnemy, _spawnPoints[j]);
                    _createdEnemies[_currentIndexEnemyNumber].Add(newEnemy);

                    count++;

                    if (count == wave.GetEnemyCount()[i])
                    {
                        _maxEnemyOnWave += count;
                        break;
                    }
                }
            }

            _numberKilledEnemies = 0;
            OnSpawnPointsReady?.Invoke();
        }

        private void PreparEnemy(Enemy enemy, SpawnPoint spawnPoint)
        {
            enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
            enemy.GetComponent<EnemyDieState>().OnRevival += OnEnemyRevival;
            enemy.gameObject.transform.parent = spawnPoint.transform;
            enemy.StartPosition = spawnPoint.transform.position;
            enemy.transform.position = enemy.StartPosition;
            enemy.OnDeath += OnEnemyDeath;
            enemy.SetIndex(_currentIndexEnemyNumber);
            enemy.gameObject.SetActive(false);
        }

        public void OnStartSpawn()
        {
            int number = 0;

            _isStopSpawning = false;
            _countActivatedEnemiesWave = 0;
            
           
                foreach (Enemy enemy in _createdEnemies[_createdEnemies.Count-1])
                {
                    if (_isStopSpawning == false)
                    {
                        enemy.transform.localPosition = enemy.StartPosition;

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
        }

        
        //     if (!enemy.IsLife()&&!enemy.GetComponent<EnemyDieState>().IsFalled)
        //     {
        //         enemy.transform.localPosition = enemy.StartPosition;
        //     }
        // else
        // {
        //     Activated(enemy);
        // }
        
        private void Activated(Enemy enemy)
        {
            if (_isStopSpawning == false)
            {
                enemy.transform.position = enemy.StartPosition;
                enemy.gameObject.SetActive(true);
                _saveLoadService.SetActiveEnemy(enemy);

                int index = enemy.IndexInWave;
                
                _activatedEnemies[index]++;

               
                
                if (_activatedEnemies[index] == _groupWave[_activatedEnemies.Count-1].GetEnemyCount()[index])
                {
                    StopRevival(index);
                }
            }
        }

        public void StopSpawn()
        {
            _isStopSpawning = true;
            
                StopRevival(_createdEnemies.Count-1);
        }

        private void OnEnemyDeath(Enemy enemy)
        {
            _saveLoadService.EnemyDeath(enemy);
        }

        private void OnEnemyRevival(Enemy enemy)
        {
            Activated(enemy);
        }

        private void StopRevival(int index)
        {
            _isStopRevival = true;

            foreach (Enemy enemy in _createdEnemies[index])
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
            foreach (var enemies in _createdEnemies)
            {
                foreach (var enemy in enemies)
                {
                    enemy.GetComponent<EnemyDieState>().SetDestroyed();
                }
            }
        }

        private void OnSetInactiveEnemy()
        {
            _numberKilledEnemies++;

            _saveLoadService.SetKilledEnemiesOnWave();

            if (_numberKilledEnemies == _maxEnemyOnLocation)
            {
                EndWave();
            }
        }

        private void AddListener()
        {
            _waveManager.OnSetWave += SetData;
            _waveManager.StartSpawn += OnStartSpawn;
            _saveLoadService.OnSetInactiveEnemy += OnSetInactiveEnemy;
        }
    }
}