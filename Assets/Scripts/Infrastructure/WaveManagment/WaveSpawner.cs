using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audio;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Service.SaveLoadService;
using UnityEngine;
using UnityEngine.Events;
using GameObject = UnityEngine.GameObject;

namespace Infrastructure.WaveManagment
{
    public class WaveSpawner : MonoCache
    {
        [SerializeField] private WaveManager _waveManager;
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private GameObject _createdEnemy;
        [SerializeField] private GameObject _spawnPointGroup;
        public List<float> DelayTimes => _waveData.DelayTimes;

        private List<SpawnPoint> _spawnPoints = new();
        private List<Enemy> _enemys = new();
        private List<Enemy> _activeEnemys = new();
        private List<Enemy> _inactiveEnemys = new();
        private List<int> enemyCounts = new();
        private List<WaveQueue> _groupWaveQueue = new();
        private WaveQueue _waveQueue = new();
        private IEnumerator _spawnCoroutine;
        public UnityAction SpawningCompleted;
        private int _totalNumber;
        private SaveLoad _saveLoad;
        private WaveData _waveData;
        //private float _cycleTimer;
        private float _cycleDuration;
        private AudioController _audioController;
        
        public void Initialize(AudioController audioController)
        {
            _audioController=audioController;
            
            if (_saveLoad==null)
            {
                _saveLoad=_waveManager.GetSaveLoad();
            }
            _enemyFactory.CreatedEnemy += OnCreatedEnemy;
        }

        public void CreateWave(WaveData waveData)
        {
            CreateWaveQueue(waveData);
            InitializeSpawnPoint();
            FillQueue();
            StartTimer();
            OnStartSpawn();
        }

        private void CreateWaveQueue(WaveData waveData)
        {
            _waveData=waveData;
            KeyValuePair<List<Enemy>, List<int>> pair = waveData.GetParticipatingEnemy().FirstOrDefault();

            WaveQueue waveQueue = new WaveQueue();
            waveQueue.SetTime(waveData.DelayTimes[0]);
            _groupWaveQueue.Add(waveQueue);
            _enemys.Add(pair.Key[0]);

            for (int i = 1; i < pair.Key.Count; i++)
            {
                    _enemys.Add(pair.Key[i]);
                    waveQueue = new WaveQueue();
                    waveQueue.SetTime(waveData.DelayTimes[i]);
                    _groupWaveQueue.Add(waveQueue);
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
                point.Initialize(i, 0,_saveLoad);
                _spawnPoints.Add(point);
                i++;
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

        private async void FillQueue()
        {
            foreach (WaveQueue waveQueue in _groupWaveQueue)
            {
                for (int i = 0; i < _enemys.Count; i++)
                {
                    int j = 0;
                    for (; j < enemyCounts[i]; j++)
                    {
                        for (int k = 0; k < _spawnPoints.Count; k++)
                        {
                            GameObject newEnemy = await CreateEnemyAsync(_enemys[i].gameObject);
                            Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
                            waveQueue.Enqueue(enemyComponent);
                        }
                    }
                }
            }
        }

        private async Task<GameObject> CreateEnemyAsync(GameObject enemy)
        {
            await Task.Run(() => CreateEnemy(enemy));
            return await _enemyFactory.Create(enemy);
        }


        private  void CreateEnemy(GameObject enemy)
        {
             _enemyFactory.Create(enemy);
        }


        private void OnCreatedEnemy(Enemy enemy)
        {
            enemy.SetAudioController(_audioController);
            enemy.gameObject.SetActive(false);
            enemy.OnDataLoad += OnCreatedEnemy;
            enemy.OnDeath += OnDeath;
            enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
            enemy.transform.parent = _createdEnemy.transform;
            _activeEnemys.Add(enemy);

            if (_activeEnemys.Count == _totalNumber)
            {
                OnStartSpawn();
            }
        }

        private void OnDeath(Enemy enemy)
        {
             _waveQueue.Enqueue(enemy);
        }
        
        public void OnStartSpawn()
        {
            int nemberQueue = 0;

            foreach (WaveQueue waveQueue in _groupWaveQueue)
            {
                foreach (SpawnPoint spawnPoint in _spawnPoints)
                {
                    spawnPoint.SetQueue(waveQueue);
                }
            }
        }

        public async Task StartTimer()
        {
            // Перед началом спауна очередей запускаем таймер
            _cycleDuration = CalculateCycleDuration(); // Рассчитываем длительность цикла спауна

            await Task.Delay(TimeSpan.FromSeconds(_cycleDuration)); // Асинхронная задержка на длительность цикла
    
            // Таймер завершился, вызываем событие завершения спауна
            SpawningCompleted?.Invoke();
        }
        
        private float CalculateCycleDuration()
        {
            // Рассчитываем общую длительность цикла спауна на основе задержек каждой очереди
            float cycleDuration = 0;
    
            foreach (WaveQueue waveQueue in _groupWaveQueue)
            {
                float queueDelay = waveQueue.DelayTime; // Получаем задержку для текущей очереди
                cycleDuration += queueDelay; // Добавляем задержку к общей длительности цикла
            }
    
            return cycleDuration;
        }
        
        public List<Enemy> GetEnemyInWaveQueue()
        {
            return _activeEnemys;
        }

        private void OnDisable()
        {
            foreach (Enemy enemy in _activeEnemys)
            {
                enemy.OnDeath -= OnDeath;
            }
        }
        
        // private void SetLocalParametrs()
        // {
        //     _saveLoad.SetActiveEnemy(_activeEnemys);
        //     _saveLoad.SetInactiveEnemy(_inactiveEnemys);
        // }
    }
}