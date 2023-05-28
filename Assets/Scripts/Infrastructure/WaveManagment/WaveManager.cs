using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audio;
using Enemies.AbstractEntity;
using Service.SaveLoadService;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.WaveManagment
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private List<WaveData> _waveDatas;
        [SerializeField] private WaveSpawner _waveSpawner;
        [SerializeField] public float TimeBetweenWaves;
        
        private List<Enemy> enemies = new List<Enemy>();
        private int currentWaveIndex = 0;
        private bool isSpawningWave = false;
        private bool isWaitingForNextWave = false;
        private bool canStartNextWave = false; // Флаг, разрешающий начало новой волны
        public WaveData CurrentWave => _waveDatas[currentWaveIndex];
        public int CurrentWaveIndex => currentWaveIndex;
        public int TotalWaves => _waveDatas.Count;
        public UnityAction SpawningCompleted;
        private SaveLoad _saveLoad;
        public void Initialize(SaveLoad saveLoad,AudioManager audioManager)
        {
            _saveLoad=saveLoad;
            InitializeWaveData();
            _waveSpawner.Initialize(audioManager);
            _waveSpawner.SpawningCompleted += OnWaveSpawningCompleted;
            StartCoroutine(SpawnWaves());
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
            canStartNextWave = true; // Устанавливаем флаг canStartNextWave, чтобы разрешить начало новой волны
        }
        
        public SaveLoad GetSaveLoad()
        {
            return _saveLoad;
        }
    }
}