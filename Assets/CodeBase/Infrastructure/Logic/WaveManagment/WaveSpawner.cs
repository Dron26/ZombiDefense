using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryWarriors.Enemies;
using Service.Audio;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.Events;
using GameObject = UnityEngine.GameObject;

namespace Infrastructure.Logic.WaveManagment
{
    public class WaveSpawner : MonoCache
    {
        private WaveManager _waveManager;
        private AudioManager _audioManager;
        private SaveLoadService _saveLoadService;
        private List<Wave> _groupWave = new();
        [SerializeField] private int _timeBetweenWaves;

        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private GameObject _spawnPointGroup;

        private List<SpawnPoint> _spawnPoints = new();
        private List<Enemy> _activeEnemys = new();
        private List<Enemy> _inactiveEnemys = new();
        private IEnumerator _spawnCoroutine;
        public UnityAction SpawningCompleted;
        private int _countActivPoint;
        private int NumberKilledEnemies=>_saveLoadService.GetNumberKilledEnemies();
        private int _numberKilledEnemies;
        
        //private float _cycleTimer;
       // private float _cycleDuration;
        private int _countCompleted;
        private int _maxEnemyOnWave;
        private int _createdEnemiesOnLevel;
        public UnityAction OnSpawnPointsReady;
        private bool _isStopSpawn;
        private int _currentNumberWave;
        private int numberEnemies;
        public void Initialize(AudioManager audioManager)
        {
            _audioManager=audioManager;
            _waveManager= GetComponent<WaveManager>();
            
            if (_saveLoadService==null)
            {
                _saveLoadService=_waveManager.GetSaveLoad();
            }
            
            _saveLoadService.OnSetInactiveEnemy+= OnSetInactiveEnemy;
            _saveLoadService.OnClearSpawnData+= ClearData;
        }

        public  void SetWaveData(Wave waveData)
        {
            AddWave(waveData);
            InitializeSpawnPoint();
            SetMaxEnemyOnWave();
            SetDataToPoint();
            
           
        }

        private void SetMaxEnemyOnWave()
        {
            Wave wave= _groupWave[_currentNumberWave];

            _maxEnemyOnWave = wave.GetEnemyCount()[_currentNumberWave];
        }

        private void AddWave(Wave wave)
        {
            Wave newWave = new();
            newWave.AddData(wave.GetEnemies(),wave.GetEnemyCount());
            newWave.SetTime(_timeBetweenWaves);
            _groupWave.Add(newWave);
           
        }
        
        private void InitializeSpawnPoint()
        {
            int i = 0;
            
            foreach (SpawnPoint point in _spawnPointGroup.transform.GetComponentsInChildren<SpawnPoint>())
            {
                point.FillCompleted+=OnFillCompleted;
                point.OnEnemyCreated += OnEnemyCreated;
                point.Initialize(i, 0,_saveLoadService,_audioManager);
                
               // point.OnEnemyStarted+=OnEnemyStarted;
               
                _spawnPoints.Add(point);
                i++;
            }
        }

        private void SetDataToPoint()
        {
            foreach (Wave wave in _groupWave)
            {
                foreach (SpawnPoint point in _spawnPoints)
                {
                    point.SetWave(wave);
                    _countActivPoint++;
                }
            }
        }

        private void OnFillCompleted()
        {
            _countCompleted++;
            
            if (_countCompleted==_countActivPoint)
            {
                _currentNumberWave++;
                OnSpawnPointsReady?.Invoke();
            }
        }
        
        public void OnStartSpawn()
        {
            foreach (Wave wave in _groupWave)
            {
                foreach (SpawnPoint spawnPoint in _spawnPoints)
                {
                    spawnPoint.OnStartSpawn();
                }
            }
        }
        
        private void OnSetInactiveEnemy()
        {
            _numberKilledEnemies++;
            
            if (_numberKilledEnemies==_maxEnemyOnWave)
            {
                StartCoroutine(WhaitBeforeFinishWave());
            }
        }

        private void OnEnemyCreated()
        {
            _createdEnemiesOnLevel++;

            Wave wave = _groupWave[_currentNumberWave];
            List<int> enemies = wave.GetEnemyCount();
            int i = enemies[numberEnemies];

            
            if (_createdEnemiesOnLevel==i)
            {
                numberEnemies++;
                
                if (numberEnemies == _groupWave[_currentNumberWave].GetEnemyCount().Count)
                {
                    OnSpawnPointsReady?.Invoke();
                    StopSpawn();
                    _isStopSpawn=true;
                }
                else
                {
                    foreach ( SpawnPoint spawnPoint in _spawnPoints)
                    {
                        spawnPoint.SetNextEnemy(true);
                    }
                }
                
            }
        }

       

        public void StopSpawn()
        {
            foreach ( SpawnPoint spawnPoint in _spawnPoints)
            {
                spawnPoint.StopSpawn();
            }
            
            if (_spawnCoroutine != null)
                StopCoroutine(_spawnCoroutine);
        }
        
        private void ClearData()
        {
            numberEnemies = 0;
            _spawnPoints.Clear();
            _groupWave.Clear();
            _countActivPoint=0;
            _countCompleted = 0;
            _isStopSpawn=false;
            _currentNumberWave = 0;
            _createdEnemiesOnLevel = 0;
        }

        private IEnumerator WhaitBeforeFinishWave()
        {
            yield return new WaitForSeconds(2);
            _saveLoadService.OnCompleteLocation();
        }
    }
}