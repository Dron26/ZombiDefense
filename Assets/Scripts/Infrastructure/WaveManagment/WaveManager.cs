using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.WaveManagment
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private List<WaveData> waves;
        [SerializeField] private WaveSpawner waveSpawner;
        [SerializeField] private float timeBetweenWaves = 1f;

        private int currentWaveIndex = 0;
        private bool isSpawningWave = false;
        private bool isWaitingForNextWave = false;

        public WaveData CurrentWave => waves[currentWaveIndex];
        public int CurrentWaveIndex => currentWaveIndex;
        public int TotalWaves => waves.Count;

        private void Start()
        {
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
                    waveSpawner.StartSpawn();
                }

                yield return null;
            }
        }

        private void OnWaveSpawningCompleted()
        {
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
    }
}