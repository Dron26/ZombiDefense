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
        public Wave _wave = new();

        private int _number;
        private int _priority;
        private float _stepDelayTime;
        private float _stepTime;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private bool isStopSpawn;
        private bool _isNextEnemy;
        private List<Enemy> _activeEnemys = new();
        private List<Enemy> _inactiveEnemys = new();
        private SaveLoadService _saveLoadService;
        private MoneyData _moneyData;
        private EnemyFactory _enemyFactory = new();
        private AudioManager _audioManager;
        public UnityAction FillCompleted;
        public Action OnEnemyCreated;
        private bool _isInfinityOn;

        public void Initialize(int number, int priority, SaveLoadService saveLoadService, AudioManager audioManager)
        {
            _number = number;
            _priority = priority;
            _stepDelayTime = 0.73f;
            _stepTime = _stepDelayTime;
            _saveLoadService = saveLoadService;
            _moneyData = saveLoadService.MoneyData;
            _enemyFactory.CreatedEnemy += OnCreatedEnemy;
            _saveLoadService.OnClearSpawnData += ClearData;
            _audioManager = audioManager;
        }

        private void ClearData()
        {
            foreach (var enemy in _activeEnemys)
                Destroy(enemy.gameObject);
            _activeEnemys.Clear();
            _isInfinityOn = true;
        }

        public void SetWave(Wave wave)
        {
            _wave = wave;
            isStopSpawn = false;
            FillPool();
        }

        public void FillPool()
        {
            StartCoroutine(StartFillPool());
        }

        private IEnumerator StartFillPool()
        {
            //
            // foreach (Enemy enemy in _groupWave[index].GetEnemies())
            // {
            //     float randomDelayTime = Random.Range(0.57f, 5.33f);
            //     yield return new WaitForSeconds( randomDelayTime);
            //     
            //     Activated(enemy);
            // }
            yield return null;
            
            for (int i = 0; i < _wave.GetEnemies().Count; i++)
            {
                _isNextEnemy = false;
                
                for (int j = 0; j < _wave.GetEnemyCount()[i]; j++)
                {
                    if (isStopSpawn)
                        break;
                    if (_isNextEnemy)
                    {
                        break;
                    }
                    
                    CreateEnemy(_wave.GetEnemies()[i].gameObject);
                    OnEnemyCreated?.Invoke();
                    yield return new WaitForSeconds(0.2f);
                }
            }

            //  FillCompleted?.Invoke();
        }

        public void SetNextEnemy(bool toSwith)
        {
            _isNextEnemy = toSwith;
        }
        
        public void OnStartSpawn()
        {
            StartCoroutine(StartSpawn());
        }

        private IEnumerator StartSpawn()
        {
           
            foreach (Enemy enemy in _activeEnemys)
            {
                float randomDelayTime = Random.Range(0.57f, 5.33f);
                yield return new WaitForSeconds(randomDelayTime);

                Activated(enemy);
            }

            isStopSpawn = true;
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

            //  
        }

        private void OnEnemyDeath(Enemy enemy)
        {
            _moneyData.AddMoneyForKilledEnemy(enemy.GetPrice());
            _saveLoadService.SetNumberKilledEnemies();


            if (!_isInfinityOn)
                enemy.GetComponent<EnemyDieState>().OnRevival -= OnEnemyRevival;
            else
                enemy.GetComponent<EnemyDieState>().SetInfinity();

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