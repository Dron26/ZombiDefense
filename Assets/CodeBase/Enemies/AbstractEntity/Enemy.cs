using System;
using System.Collections.Generic;
using Animation;
using Data;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.EnemyAI;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Service.Audio;
using Service.SaveLoad;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemies.AbstractEntity
{
    [RequireComponent(typeof(EnemyStateMachine))]
    public abstract class Enemy : MonoCache, IDamageable
    {
        private float _maxHealth;
         private float _rangeAttack;
         private int _damage;
         private int _level;
         private int _price;

        public event Action<EnemyEventType,ItemType> OnEnemyEvent;
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
        private NavMeshAgent _agent;
        private EnemyDieState _enemyDieState;
        private List<GameObject> _prefabEnemyItems; 
        private readonly float _minHealth = 0;
        private float _health;
        private bool _isLife = true;
        private int _indexInWave;
private  EnemyData _data;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _enemyAnimController = GetComponent<EnemyAnimController>();
            _fxController = GetComponent<EnemyFXController>();
            _enemyDieState = GetComponent<EnemyDieState>();
        }
        
        public void Initialize( AudioManager audioManager,EnemyData data)
        {
            _audioManager = audioManager;
            _enemyDieState.OnRevival += OnRevival;
            _data = data;
            SetData();
        }
        
        private void SetData()
        {
            _isLife = true;
            _maxHealth = _data.MaxHealth;
            _rangeAttack = _data.RangeAttack;
            _health = _maxHealth;
            _damage=_data.Damage;
            _price = _data.Price;
            _level=_data.Level;
            
            SetRandomSkin();
            SetRandomNavMeshSpeed();

            OnInitialized?.Invoke(this);
        }
        
        public float GetRangeAttack() => _rangeAttack;

        public int GetDamage() => _damage;

        public bool IsLife() => _isLife;

        public AudioManager GetAudioController() => _audioManager;
        
        private void OnRevival(Enemy enemy) => SetData();

        public int GetPrice() => _price;
        
        private void SetRandomSkin()
        {
            SkinGroup[] skinsGroup = GetComponentsInChildren<SkinGroup>();

            foreach (SkinGroup group in skinsGroup)
            {
                group.Initialize();
            }
        }

        private void SetRandomNavMeshSpeed()
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

        public abstract void AdditionalDamage(float getDamage,ItemType itemItemType);

        private void Die( ItemType itemItemType)
        {
            _isLife = false;
            OnDeath?.Invoke(this);
            OnAction(EnemyEventType.Death, itemItemType);
            EnemyStateMachine stateMachine = GetComponent<EnemyStateMachine>();
            stateMachine.EnterBehavior<EnemyDieState>();
        }

        public void OnAction(EnemyEventType action,ItemType itemType)
        {
            OnEnemyEvent?.Invoke(action,itemType);
        }

        public void SetIndex(int index)
        {
            _indexInWave = index;
        }


        public void ApplyDamage(float damage, ItemType itemType)
        {
            // if (weaponType==WeaponType.Grenade)
            // {
            //     OnTakeGranadeDamage?.Invoke();
            //     PushForGranade();
            // }
            if (_isLife)
            {
                if (_health >= 0)
                {
                    AdditionalDamage(damage, itemType);
                
                    _health -= Mathf.Clamp(damage, _minHealth, MaxHealth);
                }
        
                if (_health <= 0)
                {
                    Die(itemType);
                }
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