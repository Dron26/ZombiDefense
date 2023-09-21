using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Factories.FactoryWarriors.Enemies;
using Service;
using Service.Audio;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Infrastructure.Logic.WaveManagment
{
    public class SpawnPoint : MonoCache
    {
        public List<Wave> _groupWave = new();

        private int _number;
        private int _priority;
        private float _stepDelayTime;
        private float _stepTime;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private bool isStopSpawn;
        private List<Enemy> _activeEnemys = new();
        private List<Enemy> _inactiveEnemys = new();
        private SaveLoadService _saveLoadService;
        private MoneyData _moneyData;
        private EnemyFactory _enemyFactory= new ();
        private AudioManager _audioManager;
        public  UnityAction FillCompleted;
        public Action OnEnemyStarted;
        
        public void Initialize(int number, int priority, SaveLoadService saveLoadService ,AudioManager audioManager)
        {
            _number = number;
            _priority = priority;
            _stepDelayTime = 0.73f;
            _stepTime = _stepDelayTime;
            _saveLoadService = saveLoadService;
            _moneyData=saveLoadService.MoneyData;
            _enemyFactory.CreatedEnemy += OnCreatedEnemy;
            _saveLoadService.OnClearSpawnData+= ClearData;
            _audioManager=audioManager;
        }

        private void ClearData()
        {
            _groupWave.Clear();
            _activeEnemys.Clear();
        }

        public void SetWave(Wave wave)
        {
            _groupWave.Add(wave);
            isStopSpawn = false;
            
            
        }

        public void FillPool()
        {
            StartCoroutine(StartFillPool());
            
        }

        public void OnStartSpawn()
        {
            StartCoroutine(StartSpawn());
        }

        private IEnumerator StartSpawn()
        {
            float delayTime = 5f;
            
            foreach (Enemy enemy in _activeEnemys)
            {
                float randomDelayTime = Random.Range(0.57f, 5.33f);
                yield return new WaitForSeconds(delayTime + randomDelayTime);
                
                Activated(enemy);
            }
        }


        private IEnumerator StartFillPool()
        {
            float delayTime = 0f;

            foreach (Wave wave in _groupWave)
            {
                int count = wave.Count;

                for (int i = 0; i < count; i++)
                {
                    if (isStopSpawn)
                        break;
                    CreateEnemy(wave.GetEnemy(i).gameObject);
                    
                   // float randomDelayTime = Random.Range(0.57f, 5.33f);
                   // yield return new WaitForSeconds(delayTime + randomDelayTime);
                    yield return new WaitForSeconds(delayTime);
                }
            }
            
            FillCompleted?.Invoke();
        }

        private void CreateEnemy(GameObject enemy)
        {
            _enemyFactory.Create(enemy);
        }
        
        private void OnCreatedEnemy(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
            enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
            enemy.gameObject.transform.parent = transform;
            
            enemy.SetSaveLoad(_saveLoadService);
            enemy.SetAudioController(_audioManager);
            enemy.StartPosition = transform.position;
            enemy.transform.localPosition = Vector3.zero;
            
            enemy.GetComponent<EnemyDieState>().OnRevival += OnEnemyRevival;
            enemy.OnDeath += OnEnemyDeath;
            _activeEnemys.Add(enemy);
            OnEnemyStarted?.Invoke();
        }
        
        private void OnEnemyDeath(Enemy enemy)
        {
            _moneyData.AddMoneyForKilledEnemy(enemy.GetPrice());
            _saveLoadService.SetNumberKilledEnemies();
            
            if (isStopSpawn)
            {
                enemy.GetComponent<EnemyDieState>().OnRevival -= OnEnemyRevival;
            }
            else
            {
                enemy.GetComponent<EnemyDieState>().AfterDie();
            }
        }

        private void Activated(Enemy enemy)
        {
            enemy.gameObject.transform.position = transform.position;
            enemy.gameObject.SetActive(true);
            _saveLoadService.SetActiveEnemy(enemy);
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
            StopCoroutine(StartFillPool());
        }

        protected override void OnDisable()
        {
            StopAllCoroutines();
        }
        
    }
}