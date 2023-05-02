using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.FactoryWarriors.Enemies;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.WaveManagment
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private List<WaveData> waves;
        [SerializeField] private WaveSpawner waveSpawner;
        [SerializeField] private float timeBetweenWaves = 1f;
        private List<Enemy> enemies = new List<Enemy>();
        private int currentWaveIndex = 0;
        private bool isSpawningWave = false;
        private bool isWaitingForNextWave = false;

        public WaveData CurrentWave => waves[currentWaveIndex];
        public int CurrentWaveIndex => currentWaveIndex;
        public int TotalWaves => waves.Count;
        public UnityAction SpawningCompleted;
        
        public void Initialize()
        {
            InitializeWaveData();
            waveSpawner.SpawningCompleted += OnWaveSpawningCompleted;
            StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            while (currentWaveIndex < waves.Count)
            {
                if (!isSpawningWave && !isWaitingForNextWave)
                {
                    isSpawningWave = true;
                    waveSpawner.Initialize(CurrentWave);
                    waveSpawner.StartSpawn();
                }

                yield return null;
            }
        }

        private void InitializeWaveData()
        {
            foreach (WaveData waveData in waves)
            {
                waveData.Initialize();
            }
        }
        
        private void OnWaveSpawningCompleted()
        {
            SpawningCompleted?.Invoke();
            
            isSpawningWave = false;
            currentWaveIndex++;

            if (currentWaveIndex >= waves.Count)
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
            yield return new WaitForSeconds(timeBetweenWaves);
            isWaitingForNextWave = false;
        }

        public List<Enemy> GetEnemyGroup() => waveSpawner.GetEnemyInWaveQueue();

        public void SetHumanoidData(List<Humanoid> humanoid)
        {
            waveSpawner.SetHumanoidData(humanoid);
        }

        public EnemyFactory GetWEnemyFactory() => waveSpawner.GetEnemyFactory();
    }
}