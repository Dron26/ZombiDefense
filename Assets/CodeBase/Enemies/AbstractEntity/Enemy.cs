using System;
using System.Collections.Generic;
using Animation;
using Data;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.StateMachines.EnemyAI;
using Infrastructure.AIBattle.StateMachines.EnemyAI.States;
using Infrastructure.Logic.Inits;
using Infrastructure.Logic.WeaponManagment;
using Services.Audio;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemies.AbstractEntity
{
    [RequireComponent(typeof(EnemyStateMachine))]
    public abstract class Enemy : Entity
    {
        [SerializeField] private GameObject _shield;
        private float _maxHealth;
        private float _rangeAttack;
        private float _throwerRangeAttack;
        private int _damage;
        private int _level;
        private int _price;

        public event Action<EnemyEventType, ItemType> OnEnemyEvent;
        public event Action OnTakeGranadeDamage;
        public Action OnInitialized;

        //public Action<WeaponType> OnTakeDamage;
        public Vector3 StartPosition;
        public float MaxHealth => _maxHealth;
        public float Health => _health;
        public int Level => _level;
        public int IndexInWave => _indexInWave;
        public EnemyData Data => _data;

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
        private EnemyData _data;
        private EnemyStateMachine _stateMachine;
        private ObjectThrower _objectThrower;
        private SaveLoadService _saveLoadService;
        private bool _isShieldbearer;
        private float _shieldHealth;
        private float _shieldMaxHealth;
        public NavMeshAgent NavMeshAgent => _agent;
        public Animator Animator => _animator;
        public EnemyAnimController EnemyAnimController => _enemyAnimController;
        public EnemyFXController FXController => _fxController;
        public SaveLoadService SaveLoadService => _saveLoadService;

        private void Awake()
        {
            _stateMachine = GetComponent<EnemyStateMachine>();
            _enemyDieState = GetComponent<EnemyDieState>();
            _animator = GetComponent<Animator>();
            _enemyAnimController = GetComponent<EnemyAnimController>();
            _fxController = GetComponent<EnemyFXController>();
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
        }

        public void Initialize(AudioManager audioManager, EnemyData data, SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _audioManager = audioManager;
            _enemyDieState.OnRevival += OnRevival;
            _data = data;
            SetData();
            _stateMachine.Initialize(this);
        }

        private void SetData()
        {
            _isLife = true;
            _maxHealth = _data.MaxHealth;
            _rangeAttack = _data.RangeAttack;
            _throwerRangeAttack = _data.ThrowRangeAttack;
            _health = _maxHealth;
            _damage = _data.Damage;
            _price = _data.Price;
            _level = _data.Level;
            _isShieldbearer = _data.HasShield;

            _shield.gameObject.SetActive(_isShieldbearer);

            _shieldHealth = _data.ShieldHealth;
            _shieldMaxHealth = _shieldHealth;

            //  SetRandomSkin();
            SetRandomNavMeshSpeed();

            if (_data.IsThrower)
            {
                _objectThrower = GetComponent<ObjectThrower>();
                // _objectThrower.OnThrowed += OnThrowedGranade;
            }

            OnInitialized?.Invoke();
        }

        public float GetRangeAttack() => _rangeAttack;
        public float GetThrowerRangeAttack() => _throwerRangeAttack;

        public int GetDamage() => _damage;

        public override bool IsLife() => _isLife;

        public AudioManager GetAudioController() => _audioManager;

        public void OnRevival(Enemy enemy) => SetData();

        public int GetPrice() => _price;


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

        public abstract void AdditionalDamage(float getDamage, ItemType itemItemType);

        private void Die(ItemType itemItemType)
        {
            _isLife = false;
            _stateMachine.EnterBehavior<EnemyDieState>();
            OnAction(EnemyEventType.Death, itemItemType);
            RaiseEntityEvent();
        }

        public void OnAction(EnemyEventType action, ItemType itemType)
        {
            OnEnemyEvent?.Invoke(action, itemType);
        }

        public void SetIndex(int index)
        {
            _indexInWave = index;
        }

        public override void ApplyDamage(float damage, ItemType itemType)
        {
            if (itemType == ItemType.EnemyExplosion && _data.Type == EnemyType.Smoker)
            {
                _health = 0;
            }


            if (_isLife)
            {
                if (!_isShieldbearer)
                {
                    if (_health > 0)
                    {
                        AdditionalDamage(damage, itemType);
                        OnAction(EnemyEventType.TakeDamage, itemType);
                        _health -= Mathf.Clamp(damage, _minHealth, MaxHealth);
                    }

                    if (_health <= 0)
                    {
                        Die(itemType);
                    }
                }
                else
                {
                    if (_shieldHealth > 0)
                    {
                        _shieldHealth -= Mathf.Clamp(damage, 0, _shieldMaxHealth);
                        _fxController.ShieldDamage();
                    }
                    else
                    {
                        _isShieldbearer = false;
                        _shieldHealth = 0;
                        WasShieldShattered();
                    }
                }
            }
        }

        public void WasShieldShattered()
        {
            _shield.gameObject.SetActive(false);
            _enemyAnimController.WasShieldShattered();
        }
    }
}