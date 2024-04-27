using System.Collections.Generic;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryWarriors.Enemies;
using Service.Audio;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Logic.WaveManagment
{
    public class WaveSpawner : MonoCache
    {
        [SerializeField] private GameObject _spawnPointGroup;

        public UnityAction OnSpawnPointsReady;
        public UnityAction SpawningCompleted;
        public UnityAction OnCompletedWave;

        private WaveManager _waveManager;
        private AudioManager _audioManager;
        private SaveLoadService _saveLoadService;
        private List<Wave> _groupWave = new();
        private EnemyFactory _enemyFactory;
        private List<List<Enemy>> _createdEnemies = new();
        private List<int> _activatedEnemies = new();
        private int _currentIndexEnemyNumber;

        private List<SpawnPoint> _spawnPoints = new();
        private List<Enemy> _inactiveEnemys = new();
        private int _countActivPoint;
        private int NumberKilledEnemies => _saveLoadService.GetNumberKilledEnemies();
        private int _numberKilledEnemies;

        //private float _cycleTimer;
        // private float _cycleDuration;
        private int _countActivatedEnemies;
        private int _maxEnemyOnLocation;
        private int numberEnemies;

        private float _stepDelayTime;
        private bool _isStopSpawning;
        private bool _isStopRevival = false;
        private Wave _newWave;
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
            SetWave(waveData);
            SetSpawnPoint();
            StartFillPool(_newWave);
        }

        private void SetWave(Wave wave)
        {
            _newWave = new();
            _newWave.AddData(wave.GetEnemies(), wave.GetEnemyCount());
            _groupWave.Add(_newWave);
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
            _maxEnemyOnLocation = 0;
            
            for (int i = 0; i < x; i++)
            {
                _currentIndexEnemyNumber = i;
                _createdEnemies.Add(new List<Enemy>());
                _activatedEnemies.Add(0);
                
                int count = 0;


                for (int j = 0; j < _spawnPoints.Count; j++)
                {
                    Enemy newEnemy = _enemyFactory.Create(wave.GetEnemies()[i].gameObject);
                    PreparEnemy(newEnemy, _spawnPoints[j]);
                    _createdEnemies[_currentIndexEnemyNumber].Add(newEnemy);

                    count++;

                    if (count == wave.GetEnemyCount()[i])
                    {
                        _maxEnemyOnLocation += count;
                        break;
                    }
                }
            }

            _saveLoadService.SetMaxEnemyOnWave(_maxEnemyOnLocation);
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
            _countActivatedEnemies = 0;
            
            for (int i = 0; i < _createdEnemies.Count; i++)
            {
                foreach (Enemy enemy in _createdEnemies[i])
                {
                    if (_isStopSpawning == false)
                    {
                        enemy.transform.localPosition = enemy.StartPosition;

                        Activated(enemy);

                        _countActivatedEnemies++;

                        if (_countActivatedEnemies == _maxEnemyOnLocation)
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
            }
        }

        private void Activated(Enemy enemy)
        {
            if (_isStopSpawning == false)
            {
                enemy.transform.position = enemy.StartPosition;
                enemy.gameObject.SetActive(true);
                _saveLoadService.SetActiveEnemy(enemy);

                int index = enemy.IndexInWave;
                int count = _activatedEnemies[index];
                count++;
                _activatedEnemies[index] = count;

                if (_activatedEnemies[index] == _newWave.GetEnemyCount()[index])
                {
                    StopRevival(index);
                }
            }
        }

        public void StopSpawn()
        {
            _isStopSpawning = true;

            for (int i = 0; i < _createdEnemies.Count; i++)
            {
                StopRevival(i);
            }
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
            
            if (_waveManager.CurrentWaveIndex == _waveManager.TotalWaves) 
                _saveLoadService.SetCompletedLocation();
            
            if (_waveManager.CurrentWaveIndex < _waveManager.TotalWaves)
                OnCompletedWave?.Invoke();

            
        }

        private void ClearData()
        {
            ClearEnemies();
            _saveLoadService.ClearData();
            _createdEnemies = new();
            numberEnemies = 0;
            _spawnPoints.Clear();
            _groupWave.Clear();
            _countActivatedEnemies = 0;
            _countActivPoint = 0;
            _activatedEnemies.Clear();
            _isStopSpawning = false;
        }

        private void ClearEnemies()
        {
            foreach (var enemies in _createdEnemies)
            {
                foreach (var enemy in enemies)
                {
                    Destroy(enemy.gameObject);
                }
            }
        }

        private void OnSetInactiveEnemy()
        {
            _numberKilledEnemies++;

            _saveLoadService.SetKilledEnemiesOnWave();

            if (_numberKilledEnemies == _maxEnemyOnLocation)
            {
                _numberKilledEnemies = 0;
               EndWave();
            }
        }

        private void AddListener()
        {
            _waveManager.OnSetWave += SetData;
            _waveManager.OnStartSpawn += OnStartSpawn;
            _saveLoadService.OnSetInactiveEnemy += OnSetInactiveEnemy;
        }
    }
}