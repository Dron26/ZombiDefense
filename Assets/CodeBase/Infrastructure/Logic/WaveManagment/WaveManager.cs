using System.Collections;
using System.Collections.Generic;
using Data;
using Data.Settings.Audio;
using Enemies.AbstractEntity;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Logic.WaveManagment
{
    public class WaveManager : MonoBehaviour
    {
        private List<WaveData> _waveDatas=new();
        [SerializeField] private WaveSpawner _waveSpawner;
        [SerializeField] public float TimeBetweenWaves;
        
        private List<Enemy> enemies = new List<Enemy>();
        private int currentWaveIndex = 0;
        private bool isSpawningWave = false;
        private bool isWaitingForNextWave = false;
        private bool canStartNextWave = true; // Флаг, разрешающий начало новой волны
        public WaveData CurrentWave => _waveDatas[currentWaveIndex];
        public int CurrentWaveIndex => currentWaveIndex;
        public int TotalWaves => _waveDatas.Count;
        public UnityAction OnReadySpawning;
        public UnityAction SpawningCompleted;
        private SaveLoadService _saveLoadService;
        
        public void Initialize(SaveLoadService saveLoadService,AudioManager audioManager)
        {
            _saveLoadService=saveLoadService;
            ReadWaveDatas();
            InitializeWaveData();
            _waveSpawner.Initialize(audioManager);
            _waveSpawner.OnSpawnPointsReady += OnWaveSpawningCompleted;
            _waveSpawner.OnSpawnPointsReady+= OnWaveSpawnerReady;
            StartCoroutine(SpawnWaves());
        }

        private void OnWaveSpawnerReady()
        {
            OnReadySpawning?.Invoke();
        }

        private IEnumerator SpawnWaves()
        {
            yield return new WaitForSeconds(TimeBetweenWaves);

            while (currentWaveIndex <= _waveDatas.Count)
            {
                if (!isSpawningWave && !isWaitingForNextWave && canStartNextWave) // Добавлено условие canStartNextWave
                {
                    isSpawningWave = true;
                    _waveSpawner.CreateWave(CurrentWave);
                    canStartNextWave = false; // Сбрасываем флаг canStartNextWave
                }

                yield return null;
            }
        }

        private void ReadWaveDatas()
        {
            foreach (var wave in _saveLoadService.GetLevelPoint())
            {
                _waveDatas.Add(wave);
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
            Debug.Log(currentWaveIndex);
            if (currentWaveIndex >= _waveDatas.Count)
            {
                Debug.Log("All in Queue completed!");
            }
            else
            {
                isWaitingForNextWave = true;
                StartCoroutine(WaitForNextWave());
            }
        }

        private IEnumerator WaitForNextWave()
        {
            yield return new WaitForSeconds(TimeBetweenWaves);
            isWaitingForNextWave = false;
            canStartNextWave = true; // Устанавливаем флаг canStartNextWave, чтобы разрешить начало новой волны
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
        
        public SaveLoadService GetSaveLoad()
        {
            return _saveLoadService;
        }
    }
}