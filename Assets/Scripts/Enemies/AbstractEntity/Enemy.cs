using System.Collections.Generic;
using System.Threading.Tasks;
using Audio;
using Infrastructure.AIBattle.EnemyAI;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Observer;
using Service.SaveLoadService;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Enemies.AbstractEntity
{
    [RequireComponent(typeof(EnemyStateMachine))]
    public abstract class Enemy : MonoCache, IObservableHumanoid
    {
        private List<IObserverByHumanoid> observers = new List<IObserverByHumanoid>();
        [SerializeField] private AssetReferenceT<EnemyData> enemyDataReference;
        private List<SkinGroup> _skinGroups = new();

        private AudioManager _audioManager;
        public delegate void EnemyDeathHandler(Enemy enemy);

        public event EnemyDeathHandler OnDeath;


            // public int MinLevelForHumanoid => enemyData.MinLevelForHumanoid;
        
        protected EnemyData enemyData;
        public float MaxHealth => _maxHealth;
        public float RangeAttack => _rangeAttack;
        public int Damage => _damage;
        public int Level => _level;
        
        public float _maxHealth =0;
        public float _rangeAttack =0;
        public int _damage =0;
        public int _level =0;// Добавлено поле Level
        public GameObject GetPrefab() => enemyData.PrefabCharacter;
        public abstract int GetLevel();
        public abstract float GetRangeAttack();
        public abstract int GetDamage();
        public abstract float GetHealth();
        public abstract bool IsLife();
        public abstract int GetPrice();
        public UnityAction<Enemy> OnDataLoad;

        private NavMeshAgent _agent;
        public Vector3 StartPosition;

        public abstract void ApplyDamage(float getDamage, string weaponName);

        public abstract void SetAttacments();

        public float GetRadiusSearch() => 25f;

        protected virtual void Die()
        {
            OnDeath?.Invoke(this);
            EnemyStateMachine stateMachine = GetComponent<EnemyStateMachine>();
            stateMachine.EnterBehavior<EnemyDieState>();
        }

        public Task LoadPrefab()
        {
            var tcs = new TaskCompletionSource<bool>();

            if (enemyDataReference.Asset != null)
            {
                enemyData = (EnemyData)enemyDataReference.Asset;
            }

            enemyDataReference.LoadAssetAsync().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    enemyData = (Infrastructure.FactoryWarriors.Enemies.EnemyData)handle.Result;
                    tcs.TrySetResult(true);
                    Initialize();
                    SetSkin();
                    SetNavMeshSpeed();
                    SetStartParametr();
                    OnDataLoad?.Invoke(this);
                }
                else
                {
                    Debug.LogError($"Failed to load enemy data: {handle.OperationException}");
                    tcs.TrySetException(handle.OperationException);
                }
            };

            return tcs.Task;
        }

        public abstract void SetSaveLoad(SaveLoad saveLoad);


        public abstract void Initialize();

        private void SetSkin()
        {
            foreach (SkinGroup group in transform.GetComponentsInChildren<SkinGroup>())
            {
                group.Initialize();
                group.SetMesh(Random.Range(0, group.GetCountMeshes()));
            }
        }

        private void SetNavMeshSpeed()
        {
            _agent = GetComponent<NavMeshAgent>();
            float minSpeed = 0.6f;
            float maxSpeed = 1.2f;

            if (Level == 4)
            {
                _agent.speed = 0.6f;
            }
            else
            {
                _agent.speed = Random.Range(minSpeed, maxSpeed);
            }
        }

        public void AddObserver(IObserverByHumanoid observerByHumanoid)
        {
            observers.Add(observerByHumanoid);
        }

        public void RemoveObserver(IObserverByHumanoid observerByHumanoid)
        {
            observers.Remove(observerByHumanoid);
        }

        public void NotifyObservers(object data)
        {
            foreach (var observer in observers)
            {
                observer.NotifyFromHumanoid(data);
            }
        }
        
        public void SetAudioController(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        public AudioManager GetAudioController()
        {
            return _audioManager;
        }
        
        private void SetStartParametr()
        {
              _maxHealth = enemyData.MaxHealth;
              _rangeAttack = enemyData.RangeAttack;
              _damage = enemyData.Damage;
              _level = enemyData.Level;
        }
        
    }
}