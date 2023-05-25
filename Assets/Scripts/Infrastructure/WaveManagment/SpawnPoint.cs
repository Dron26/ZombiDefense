using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Infrastructure.WaveManagment
{
    public class SpawnPoint : MonoCache
    {
        public List<WaveQueue> _groupWaveQueue = new();
      
        private int _number;
        private int _priority;
        private float _delayTime;
        private float _stepDelayTime;
        private float _stepTime;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private bool isStopSpawn;
        private List<Enemy> _activeEnemys = new();
        private List<Enemy> _inactiveEnemys = new();
        private SaveLoad _saveLoad;
        
        public void Initialize(int number, int priority, SaveLoad saveLoad)
        {
            _number = number;
            _priority = priority;
            _stepDelayTime = 0.73f;
            _delayTime = 7f;
            _stepTime = _stepDelayTime;
            
            _saveLoad=saveLoad;
        }
        
        public void SetQueue(WaveQueue queue)
        {
            _groupWaveQueue.Add(queue);
            isStopSpawn = false;

            StartCoroutine(StartSpawn());
        }
        
        private IEnumerator StartSpawn()
        {
            float delayTime = 5.0f; 
            yield return new WaitForSeconds(Random.Range(0.57f, 5.33f));
    
            foreach (WaveQueue queue in _groupWaveQueue)
            {
                int count = queue.Count;

                for (int i = 0; i < count; i++)
                {
                    if (isStopSpawn)
                        break;
                    Enemy enemy = queue.Dequeue();
                    enemy.gameObject.transform.parent = transform;
                    enemy.SetSaveLoad(_saveLoad);
                    enemy.StartPosition = transform.position;
                    enemy.GetComponent<EnemyDieState>().OnRevival += OnEnemyRevival;
                    Activated(enemy);

                    float randomDelayTime = Random.Range(0.57f, 5.33f);
                    yield return new WaitForSeconds(delayTime + randomDelayTime);
                }
            }
        }

        private void Activated( Enemy enemy)
        {
            enemy.gameObject.transform.position = transform.position;
            enemy.gameObject.SetActive(true);
            _saveLoad.SetActiveEnemy(enemy);
        }

        
        private void OnEnemyRevival(Enemy enemy)
        {
            Activated(enemy);
        }
        
        public void StopSpawn()
        {
            isStopSpawn = true;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}