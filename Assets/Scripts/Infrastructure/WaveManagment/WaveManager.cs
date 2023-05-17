using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.FactoryWarriors.Enemies;
using Service.SaveLoadService;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.WaveManagment
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private List<WaveData> _waveDatas;
        [SerializeField] private WaveSpawner _waveSpawner;
        [SerializeField] private float _timeBetweenWaves = 1f;
        private List<Enemy> enemies = new List<Enemy>();
        private int currentWaveIndex = 0;
        private bool isSpawningWave = false;
        private bool isWaitingForNextWave = false;

        public WaveData CurrentWave => _waveDatas[currentWaveIndex];
        public int CurrentWaveIndex => currentWaveIndex;
        public int TotalWaves => _waveDatas.Count;
        public UnityAction SpawningCompleted;
        private SaveLoad _saveLoad;
        
        public void Initialize(SaveLoad saveLoad)
        {
            _saveLoad=saveLoad;
            InitializeWaveData();
            _waveSpawner.SpawningCompleted += OnWaveSpawningCompleted;
            StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            yield return new WaitForSeconds(_timeBetweenWaves);

            while (currentWaveIndex < _waveDatas.Count)
            {
                if (!isSpawningWave && !isWaitingForNextWave)
                {
                    isSpawningWave = true;
                    _waveSpawner.Initialize(CurrentWave);
                }

                yield return null;
            }
        }

        private void InitializeWaveData()
        {
            foreach (WaveData waveData in _waveDatas)
            {
                waveData.Initialize();
            }
        }
        
        private void OnWaveSpawningCompleted()
        {
            SpawningCompleted?.Invoke();
            
            isSpawningWave = false;
            currentWaveIndex++;

            if (currentWaveIndex >= _waveDatas.Count)
            {
                Debug.Log("All waves completed!");
            }
            else
            {
                isWaitingForNextWave = true;
                StartCoroutine(WaitForNextWave());
            }
        }

        private IEnumerator WaitForNextWave()
        {
            yield return new WaitForSeconds(_timeBetweenWaves);
            isWaitingForNextWave = false;
        }

        public WaveSpawner GetWaveSpawner() => _waveSpawner;

        public void StopSpawn()
        {
            _waveSpawner.StopSpawn();
        }

        public void StartSpawn()
        {
            _waveSpawner.OnStartSpawn();
        }

        public SaveLoad GetSaveLoad()
        {
            return _saveLoad;
        }
    }
}