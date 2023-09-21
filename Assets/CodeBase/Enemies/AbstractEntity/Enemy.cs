using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.AIBattle.EnemyAI;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Infrastructure.Observer;
using Service.Audio;
using Service.SaveLoad;
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
        [SerializeField] private float _maxHealth = 40f;
        [SerializeField] private float _rangeAttack = 1.2f;
        [SerializeField] private int _damage = 15;
        [SerializeField] private int _level = 1;
        [SerializeField] private int _minLevelForHumanoid = 0;

        
        public int MinLevelForHumanoid =>_minLevelForHumanoid;

        
        
        private List<IObserverByHumanoid> observers = new List<IObserverByHumanoid>();
        private List<SkinGroup> _skinGroups = new();

        private AudioManager _audioManager;
        public delegate void EnemyDeathHandler(Enemy enemy);

        public event EnemyDeathHandler OnDeath;


            // public int MinLevelForHumanoid => enemyData.MinLevelForHumanoid;
        
        public float MaxHealth => _maxHealth;
        public float RangeAttack => _rangeAttack;
        public int Damage => _damage;
        public int Level => _level;
        public abstract int GetLevel();
        public abstract float GetRangeAttack();
        public abstract int GetDamage();
        public abstract float GetHealth();
        public abstract bool IsLife();
        public abstract int GetPrice();
        public UnityAction<Enemy> OnDataLoad;

        private NavMeshAgent _agent;
        public Vector3 StartPosition;

        public abstract void ApplyDamage(float getDamage, WeaponType weaponWeaponType);

        public abstract void SetAttacments();

        public float GetRadiusSearch() => 25f;

        protected virtual void Die()
        {
            OnDeath?.Invoke(this);
            EnemyStateMachine stateMachine = GetComponent<EnemyStateMachine>();
            stateMachine.EnterBehavior<EnemyDieState>();
        }
        
        public void LoadPrefab()
        {
            Initialize();
                    SetSkin();
                    SetNavMeshSpeed();
                    OnDataLoad?.Invoke(this);
        }


        public abstract void SetSaveLoad(SaveLoadService saveLoadService);


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

    }
}