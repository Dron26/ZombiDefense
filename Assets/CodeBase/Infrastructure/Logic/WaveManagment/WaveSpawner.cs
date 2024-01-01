using System.Collections;
using System.Collections.Generic;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryWarriors.Enemies;
using Service;
using Service.Audio;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Logic.WaveManagment
{
    public class WaveSpawner : MonoCache
    {
        [SerializeField] private int _timeBetweenWaves;

        [SerializeField] private GameObject _spawnPointGroup;
        
        public UnityAction OnSpawnPointsReady;
        public UnityAction SpawningCompleted;

        private WaveManager _waveManager;
        private AudioManager _audioManager;
        private SaveLoadService _saveLoadService;
        private MoneyData _moneyData;
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
        private int _maxEnemyOnWave;
        private int numberEnemies;
        
        private float _stepDelayTime;
        private bool _isStopSpawning;
        private bool _isStopRevival = false;
        private Wave _newWave;

        public void Initialize(AudioManager audioManager, EnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
            _audioManager = audioManager;
            _waveManager = GetComponent<WaveManager>();

            if (_saveLoadService == null)
            {
                _saveLoadService = _waveManager.GetSaveLoad();
            }

            _saveLoadService.OnSetInactiveEnemy += OnSetInactiveEnemy;
            _saveLoadService.OnClearSpawnData += ClearData;

            _stepDelayTime = 0.73f;
            _moneyData = _saveLoadService.MoneyData;
        }

        public void SetWaveData(Wave waveData)
        {
            Debug.Log("SetWaveData");
            SetWave(waveData);
            SetSpawnPoint();
            SetMaxEnemyOnWave();
            StartFillPool(_newWave);
        }

        private void SetWave(Wave wave)
        {
            _newWave = new();
            _newWave.AddData(wave.GetEnemies(), wave.GetEnemyCount());
            _newWave.SetTime(_timeBetweenWaves);
            _groupWave.Add(_newWave);
        }

        private void SetSpawnPoint()
        {
            foreach (SpawnPoint point in _spawnPointGroup.transform.GetComponentsInChildren<SpawnPoint>())
            {
                _spawnPoints.Add(point);
            }
        }

        private void SetMaxEnemyOnWave()
        {
            int count=0;
            
            for (int i = 0; i < _newWave.GetEnemyCount().Count; i++)
            {
                count+=_newWave.GetEnemyCount()[i];
            }
                
            _maxEnemyOnWave=count;

            _saveLoadService.SetMaxEnemyOnWave(_maxEnemyOnWave);
            
        }

        private void StartFillPool(Wave wave)
        {
         
            Debug.Log("StartFillPool");
            
            for (int i = 0; i < wave.GetEnemies().Count; i++)
            {
                _currentIndexEnemyNumber = i;
                _createdEnemies.Add(new List<Enemy>());
                _activatedEnemies.Add(0);
                int count = 0;

                while (count < _maxEnemyOnWave)
                {
                    for (int j = 0; j < _spawnPoints.Count; j++)
                    {
                        Enemy newEnemy = _enemyFactory.Create(wave.GetEnemies()[i].gameObject);
                        PreparEnemy(newEnemy,_spawnPoints[j]);
                        _createdEnemies[_currentIndexEnemyNumber].Add(newEnemy);

                        count++;
                        if (count==wave.GetEnemyCount()[i])
                        {
                            break;
                        }
                    }
                }
            }

            OnSpawnPointsReady?.Invoke();
        }

        private void PreparEnemy(Enemy enemy, SpawnPoint spawnPoint)
        {
            Debug.Log("CreatedEnemy");
            enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
            enemy.GetComponent<EnemyDieState>().OnRevival += OnEnemyRevival;
            enemy.gameObject.transform.parent = spawnPoint.transform;
            enemy.StartPosition=spawnPoint.transform.position;
            enemy.transform.position =enemy.StartPosition;
            enemy.OnDeath += OnEnemyDeath;
            enemy.SetIndex(_currentIndexEnemyNumber);
            enemy.gameObject.SetActive(false);
        }

        public void OnStartSpawn()
        {
            int number = 0;

            for (int i = 0; i < _createdEnemies.Count; i++)
            {
                foreach (Enemy enemy in _createdEnemies[i])
                {
                    if (_isStopSpawning==false)
                    {
                        enemy.transform.localPosition = enemy.StartPosition;

                        Activated(enemy);

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
            if (_isStopSpawning==false)
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

                _countActivatedEnemies++;

                if (_countActivatedEnemies == _maxEnemyOnWave)
                {
                    StopSpawn();
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
            _moneyData.AddMoneyForKilledEnemy(enemy.GetPrice());
            _saveLoadService.SetNumberKilledEnemies();
        }


        private void OnEnemyRevival(Enemy enemy)
        {
            Activated(enemy);
        }

        private void StopRevival(int index)
        {
            _isStopRevival=true;

            foreach (Enemy enemy in _createdEnemies[index])
            {
                EnemyDieState enemyDieState = enemy.GetComponent<EnemyDieState>();
                enemyDieState.StopRevival(_isStopRevival);
            }
        }


        private IEnumerator EndWave()
        {
            yield return new WaitForSeconds(2);
            _saveLoadService.SetCompletedLocation();
        }

        private void ClearData()
        {
            _createdEnemies = new();;
            numberEnemies = 0;
            _spawnPoints.Clear();
            _groupWave.Clear();
            _countActivatedEnemies = 0;
            _countActivPoint = 0;
            _isStopSpawning = false;
        }

        private void  OnSetInactiveEnemy()
        {
            _numberKilledEnemies++;

            _saveLoadService.SetKilledEnemiesOnWave(_numberKilledEnemies);
            
            if (_numberKilledEnemies == _maxEnemyOnWave)
            {
                StartCoroutine(EndWave());
            }
        }
    }
}