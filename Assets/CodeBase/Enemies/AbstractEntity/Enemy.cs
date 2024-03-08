using System;
using System.Collections.Generic;
using Animation;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.EnemyAI;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Service.Audio;
using Service.SaveLoad;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemies.AbstractEntity
{
    [RequireComponent(typeof(EnemyStateMachine))]
    public abstract class Enemy : MonoCache, IDamageable
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _rangeAttack = 1.2f;
        [SerializeField] private int _damage;
        [SerializeField] private int _level;
        [SerializeField] private int _price;

        public event Action<EnemyEventType,WeaponType> OnEnemyEvent;
        public event Action OnTakeGranadeDamage;
        public Action<Enemy> OnInitialized;
        public Action<Enemy> OnDeath; 
        //public Action<WeaponType> OnTakeDamage;
        public Animator Animator=>_animator;
        public EnemyAnimController EnemyAnimController=>_enemyAnimController;
        public Vector3 StartPosition;
        public float MaxHealth => _maxHealth;
        public float Health => _health;
        public int Level => _level;
        public int IndexInWave => _indexInWave;
        
        private List<SkinGroup> _skinGroups = new();
        private AudioManager _audioManager;
        private Animator _animator;
        private EnemyAnimController _enemyAnimController;
        private EnemyFXController _fxController;
        private SaveLoadService _saveLoadService;
        private NavMeshAgent _agent;
        private EnemyDieState _enemyDieState;
        
        private readonly float _minHealth = 0;
        private float _health;
        private bool _isLife = true;
        private int _indexInWave;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _enemyAnimController = GetComponent<EnemyAnimController>();
            _fxController = GetComponent<EnemyFXController>();
            _enemyDieState = GetComponent<EnemyDieState>();
        }
        
        public void Initialize(SaveLoadService saveLoadService, AudioManager audioManager)
        {
            _saveLoadService=saveLoadService;
            _audioManager = audioManager;
            _enemyDieState.OnRevival += OnRevival;

            SetStartData();
        }
        
        private void SetStartData()
        {
            _isLife = true;
            _health = _maxHealth;
           
            SetSkin();
            SetNavMeshSpeed();

            OnInitialized?.Invoke(this);
        }
        
        public float GetRangeAttack() => _rangeAttack;

        public int GetDamage() => _damage;

        public bool IsLife() => _isLife;

        public AudioManager GetAudioController() => _audioManager;
        
        private void OnRevival(Enemy enemy) => SetStartData();

        public int GetPrice() => _price;
        
        private void SetSkin()
        {
            SkinGroup[] skinsGroup = GetComponentsInChildren<SkinGroup>();

            foreach (SkinGroup group in skinsGroup)
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

        public abstract void PushForGranade();

        public abstract void AdditionalDamage(float getDamage,WeaponType weaponWeaponType);

        private void Die( WeaponType weaponWeaponType)
        {
            OnDeath?.Invoke(this);
            OnAction(EnemyEventType.Death, weaponWeaponType);
            EnemyStateMachine stateMachine = GetComponent<EnemyStateMachine>();
            stateMachine.EnterBehavior<EnemyDieState>();
        }

        public void OnAction(EnemyEventType action,WeaponType weaponType)
        {
            OnEnemyEvent?.Invoke(action,weaponType);
        }

        public void SetIndex(int index)
        {
            _indexInWave = index;
        }


        public void ApplyDamage(float damage, WeaponType weaponType)
        {
            // if (weaponType==WeaponType.Grenade)
            // {
            //     OnTakeGranadeDamage?.Invoke();
            //     PushForGranade();
            // }
            
            if (_health >= 0)
            {
                AdditionalDamage(damage, weaponType);
                
                _health -= Mathf.Clamp(damage, _minHealth, MaxHealth);
            }
        
            if (_health <= 0)
            {
                _saveLoadService.SetInactiveEnemy(this);
                _isLife = false;
                Die(weaponType);
            }
        }
    }
}

public enum EnemyEventType
{
    TakeDamage,
    Death,
    TakeSmokerDamage,
    TakeSimpleWalkerDamage,
    TakeGranadeDamage
}