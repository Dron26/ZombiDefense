using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Infrastructure.WaveManagment
{
    public class SpawnPoint : MonoCache
    {
        private int _number;
        private int _priority;
        private float _delayTime;
        private float _stepDelayTime;
        private float _stepTime;
        public List<WaveQueue> _groupWaveQueue = new();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private bool isStopSpawn;
        
        private List<Enemy> _activeEnemys = new();
        private List<Enemy> _inactiveEnemys = new();

        public void Initialize(int number, int priority)
        {
            _number = number;
            _priority = priority;
            _stepDelayTime = 0.73f;
            _delayTime = 7f;
            _stepTime = _stepDelayTime;
        }

        
        
        public void SetQueue(WaveQueue queue)
        {
            _groupWaveQueue.Add(queue);
            isStopSpawn = false;
        }

        

        private async Task StartSpawn()
        {
            float delayTime = 5.0f; 
            await Task.Delay(TimeSpan.FromSeconds(Random.Range(0.57f, 5.33f)));
            
            foreach (WaveQueue queue in _groupWaveQueue)
            {
                int count =queue.Count;
                
                for (int i = 0; i < count; i++)
                {
                    if (isStopSpawn)
                        break;
                    Enemy enemy = queue.Dequeue();
                    enemy.gameObject.transform.parent = transform;
                    enemy.StartPosition=transform.position;
                    Activated(enemy);
                    
                    float randomDelayTime = Random.Range(0.57f, 5.33f);
                    await Task.Delay(TimeSpan.FromSeconds(delayTime + randomDelayTime), _cancellationTokenSource.Token);
                }
            }
        }

        private void Activated( Enemy enemy)
        {
            enemy.gameObject.transform.position = transform.position;
            enemy.GetComponent<EnemyDieState>().OnDeath += OnEnemyDeath;
            enemy.gameObject.SetActive(true);
            _activeEnemys.Add(enemy);
        }

        public void StopSpawn()
        {
            isStopSpawn = true;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void OnEnemyDeath(Enemy enemy)
        {
            Activated(enemy);
        }
    }
}