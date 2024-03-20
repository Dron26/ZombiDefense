using System;
using Data;
using Data.Upgrades;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using Service.Audio;
using UnityEngine;

namespace Characters.Humanoids.AbstractLevel
{
    [RequireComponent(typeof(HumanoidWeaponController))]
    [RequireComponent(typeof(PlayerCharactersStateMachine))]
    public abstract class Humanoid : Character
{
    protected AudioManager _audioManager;
    public Vector3 StartPosition;
    public Action<Humanoid> OnDataLoad;
    protected Animator _animator;
    protected PlayerCharacterAnimController _playerCharacterAnimController;
    protected FXController _fxController;
    protected int _currentHealth;
    protected bool _isTakeDamagePlay;
    protected int _minHealth = 0;
    protected bool _isBuyed = false;
    public bool IsBuyed => _isBuyed;
    public Action OnMove;
    public Action<Humanoid> OnInitialize;

    public  override void ApplyDamage(int getDamage)
    {
        Debug.Log(_currentHealth);

        if (_currentHealth <= 0)
        {
            _animator.SetTrigger(_playerCharacterAnimController.Die);
            IsLife = false;
            Die();
        }
        else
        {
            if (!_isTakeDamagePlay)
            {
                _isTakeDamagePlay = true;
                _animator.SetTrigger(_playerCharacterAnimController.IsHit);
            }

            _fxController.OnHitFX();
            _currentHealth -= Mathf.Clamp(getDamage, _minHealth, _maxHealth);
        }
    }
    public override void SetUpgrade(UpgradeData upgrade, int level)
    {
        _maxHealth += upgrade.Health;
        _currentHealth += _maxHealth; 
    }
    private void Die()
    {
        PlayerCharactersStateMachine stateMachine = GetComponent<PlayerCharactersStateMachine>();
        stateMachine.EnterBehavior<DieState>();
    }

    public void Initialize(AudioManager audioManager)
    {
        _audioManager = audioManager;
        _currentHealth = _maxHealth;
        _animator = GetComponent<Animator>();
        _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
        _fxController = GetComponent<FXController>();
        OnInitialize?.Invoke(this);
    }

    public AudioManager GetAudioController()
    {
        return _audioManager;
    }


    private void SetUpgradeFromPoint(int upPrecent)
    {
        _maxHealth += (_maxHealth * upPrecent) / 100;
    }

    public void SetPontInfo()
    {
    }

    public void IsMoving(bool isMoving)
    {
        _isMoving = isMoving;
        if (isMoving)
        {
            OnMove?.Invoke();
        }
    }
    
    public void SetAvailable(bool isBuyed)
    {
        isBuyed = isBuyed;
    }
    
    public void UIInitialize()
    {
        _currentHealth = _maxHealth;
    }

    protected void SetPoint(WorkPoint workPoint)
    {
        if(workPoint.IsHaveMedicineBox && _currentHealth < _maxHealth)
        {
            OpenMedicineBox(workPoint.GetMedicineBox());
        }
    }

    

    private void OpenMedicineBox(MedicineBox medicineBox)
    {
        AddHealth(((_maxHealth * medicineBox.GetRecoveryRate()) / 100));
    }

    private void AddHealth(int health)
    {
        _currentHealth += health;
    }
}


    public abstract class Character : MonoCache
    {
        [SerializeField] protected int _maxHealth;
        [SerializeField] protected ParticleSystem _ring;
        [SerializeField] protected Sprite _sprite;
        [SerializeField] protected int _price;
        public bool IsMove => _isMoving;
        protected bool _isMoving;
        public int Price => _price;
        [SerializeField] protected int _id;
        public int ID => _id;

        
        public bool IsLife { get; protected set; }
        public Sprite Sprite => _sprite;
        public bool CanMove => _canMove;
        public bool _canMove=false;
        protected virtual void Awake()
        {
            InitializeCharacter();
        }

        protected virtual void InitializeCharacter()
        {
            // Общая инициализация для всех персонажей
            IsLife = true;

            if (transform.TryGetComponent(out Humanoid _))
            {
                _canMove = true;
            }

        }
        public int GetMaxHealth()
        {
            return _maxHealth;
        }
        public abstract void ApplyDamage(int damage);

       
        public string GetName() => ConstantsData.GetName(_id);
        protected virtual void Die()
        {
            IsLife = false;
            // Логика смерти персонажа
        }

        public abstract void SetUpgrade(UpgradeData upgrade, int level);
        // Другие общие методы и свойства
        
        public virtual void SetPoint(WorkPoint workPoint)
        {}
    }
}