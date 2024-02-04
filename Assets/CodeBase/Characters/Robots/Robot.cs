using System;
using Characters.Humanoids.AbstractLevel;
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

namespace Characters.Robots
{
    [RequireComponent(typeof(WeaponController))]
    [RequireComponent(typeof(PlayerCharactersStateMachine))]
    public abstract class Robot : MonoCache
    {
        [SerializeField] private int _id;
        [SerializeField] private int _maxHealth;
        [SerializeField] private ParticleSystem _shotRadius;
        [SerializeField] private Sprite _sprite;
        private AudioManager _audioManager;

        public Vector3 StartPosition;
        public Action<Humanoid> OnDataLoad;
        private Animator _animator;
        private RobotFXController _fxController;
        public int ID => _id;
        private int _currentHealth;
        private bool _isLife = true;
        private bool _isTakeDamagePlay;
        public int MaxHealth => _maxHealth;
        private  int _minHealth = 0;
        private bool _isBuyed = false;
        public Action OnMove;
        public Action <Robot>OnInitialize;
        private bool _isSelected;
        private bool _isMoving;
        public bool IsMove => _isMoving;
        public string GetName() => ConstantsData.GetName(_id);
        public void ApplyDamage(int getDamage)
        {
            Debug.Log(_currentHealth);
            
            if (_currentHealth <= 0)
            {
                _isLife = false;
                Die();
            }
            else
            {
                if (!_isTakeDamagePlay)
                {
                    _isTakeDamagePlay = true;
                }
                
                _fxController.OnHitFX();
                _currentHealth -= Mathf.Clamp(getDamage, _minHealth, _maxHealth);
            }
        }
        
        public void Die()
        {
            PlayerCharactersStateMachine stateMachine = GetComponent<PlayerCharactersStateMachine>();
            stateMachine.EnterBehavior<DieState>();
        }

        public void Initialize(AudioManager audioManager)
        {
            _audioManager = audioManager;
            _currentHealth=_maxHealth;
            _animator = GetComponent<Animator>();
            _fxController=GetComponent<RobotFXController>();
            OnInitialize?.Invoke(this);
        }

        public AudioManager GetAudioController()
        {
            return _audioManager;
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected = isSelected;

            if (_isSelected == true)
            {
                _shotRadius.gameObject.SetActive(true);
            }
            else
            {
                _shotRadius.gameObject.SetActive(false);
            }
        }

        private void SetUpgradeFromPoint(int upPrecent)
        {
            _maxHealth+=(_maxHealth*upPrecent)/100;
        }
        
        public void SetPontInfo()
        {
        }

        public void IsMoving(bool isMoving)
        {
            _isMoving = isMoving;
            OnMove?.Invoke();
        }


        public void SetAvailable(bool isBuyed)
        {
            isBuyed = isBuyed;
        }


        public  void SetUpgrade(UpgradeData upgrade, int level)
        {
            _maxHealth+= upgrade.Health;
            _currentHealth+=_maxHealth;
        }

        public void UIInitialize()
        {
            _currentHealth=_maxHealth;
        }
        
        public void SetPoint(WorkPoint workPoint)
        {
            if (workPoint.IsHaveMedicineBox&&_currentHealth<_maxHealth)
            {
                OpenMedicineBox(workPoint.GetMedicineBox());
            }
        }

        private void OpenMedicineBox(MedicineBox medicineBox)
        {
            AddHealth(((_maxHealth*medicineBox.GetRecoveryRate())/100));
        }

        private void AddHealth(int health)
        {
            _currentHealth+=_maxHealth;
        }
    }
}