using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Settings.Audio;
using Enemies.AbstractEntity;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryWarriors.Enemies;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.Events;
using GameObject = UnityEngine.GameObject;

namespace Infrastructure.Logic.WaveManagment
{
    public class WaveSpawner : MonoCache
    {
        [SerializeField] private WaveManager _waveManager;
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private GameObject _spawnPointGroup;
        public List<float> DelayTimes => _waveData.DelayTimes;

        private List<SpawnPoint> _spawnPoints = new();
        private List<Enemy> _enemys = new();
        private List<Enemy> _activeEnemys = new();
        private List<Enemy> _inactiveEnemys = new();
        private List<int> enemyCounts = new();
        private List<Wave> _groupWave = new();
        private Wave _wave = new();
        private IEnumerator _spawnCoroutine;
        public UnityAction SpawningCompleted;
        private int _totalNumber;
        private SaveLoadService _saveLoadService;
        private WaveData _waveData;
        //private float _cycleTimer;
       // private float _cycleDuration;
        private AudioManager _audioManager;
        private int _countCompleted;

        public UnityAction OnSpawnPointsReady;
        
        public void Initialize(AudioManager audioManager)
        {
            _audioManager=audioManager;
            
            if (_saveLoadService==null)
            {
                _saveLoadService=_waveManager.GetSaveLoad();
            }
        }

        public  void CreateWave(WaveData waveData)
        {
            Create(waveData);
            InitializeSpawnPoint();
            FillWave();
        }

        private void Create(WaveData waveData)
        {
            _waveData=waveData;
            KeyValuePair<List<Enemy>, List<int>> pair = waveData.GetParticipatingEnemy().FirstOrDefault();

            Wave wave = new Wave();
            wave.SetTime(waveData.DelayTimes[0]);
            _groupWave.Add(wave);
            _enemys.Add(pair.Key[0]);

            for (int i = 1; i < pair.Key.Count; i++)
            {
                    _enemys.Add(pair.Key[i]);
                    wave = new Wave();
                    wave.SetTime(waveData.DelayTimes[i]);
                    _groupWave.Add(wave);
            }
            
            foreach (int count in pair.Value)
            {
                enemyCounts.Add(count);
                _totalNumber += count;
            }
        }
        
        private void InitializeSpawnPoint()
        {
            int i = 0;
            foreach (SpawnPoint point in _spawnPointGroup.transform.GetComponentsInChildren<SpawnPoint>())
            {
                point.Initialize(i, 0,_saveLoadService,_audioManager);
                point.FillCompleted+=OnFillCompleted;
                _spawnPoints.Add(point);
                i++;
            }
        }

        private void OnFillCompleted()
        {
            _countCompleted++;
            
            if (_countCompleted==_groupWave.Count*_spawnPoints.Count)
            {
                OnSpawnPointsReady?.Invoke();
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

        private void FillWave()
        {
            foreach (SpawnPoint point in _spawnPoints)
            {
                Wave wave = gameObject.AddComponent<Wave>();
                
                for (int i = 0; i < _enemys.Count; i++)
                {
                    int j = 0;
                    for (; j < enemyCounts[i]; j++)
                    {
                            Enemy newEnemy =  _enemys[i];
                            wave.AddEnemy(newEnemy);
                    }
                }
                
                point.SetWave(wave);
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

        // public async Task StartTimer()
        // {
        //     // Перед началом спауна очередей запускаем таймер
        //     _cycleDuration = CalculateCycleDuration(); // Рассчитываем длительность цикла спауна
        //
        //     await Task.Delay(TimeSpan.FromSeconds(_cycleDuration)); // Асинхронная задержка на длительность цикла
        //
        //     // Таймер завершился, вызываем событие завершения спауна
        //     SpawningCompleted?.Invoke();
        // }
        //
        // private float CalculateCycleDuration()
        // {
        //     // Рассчитываем общую длительность цикла спауна на основе задержек каждой очереди
        //     float cycleDuration = 0;
        //
        //     foreach (Wave waveQueue in _groupWave)
        //     {
        //         float queueDelay = waveQueue.DelayTime; // Получаем задержку для текущей очереди
        //         cycleDuration += queueDelay; // Добавляем задержку к общей длительности цикла
        //     }
        //
        //     return cycleDuration;
        // }
    }
}