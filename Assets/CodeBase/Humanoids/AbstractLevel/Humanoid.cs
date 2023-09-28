using System;
using System.Collections.Generic;
using Data;
using Data.Upgrades;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Service.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace Humanoids.AbstractLevel
{
    [RequireComponent(typeof(WeaponController))]
    [RequireComponent(typeof(PlayerCharactersStateMachine))]
    public abstract class Humanoid : MonoCache
    {
        [SerializeField] private int _id;
        [SerializeField] private int _maxHealth;
        [SerializeField] private ParticleSystem _ring;
        [SerializeField] private int _price;
        [SerializeField] private Sprite _sprite;
        private AudioManager _audioManager;

        public Vector3 StartPosition;
        public Action<Humanoid> OnDataLoad;
        public int Price => _price;
        
        private Animator _animator;
        private PlayerCharacterAnimController _playerCharacterAnimController;
        private FXController _fxController;

        public int ID => _id;
        private int _currentHealth;
        private bool _isLife = true;
        private bool _isTakeDamagePlay;
        public int MaxHealth => _maxHealth;

        private  int _minHealth = 0;
        
        private bool _isBuyed = false;
        public bool IsBuyed => _isBuyed;

       // public  int GetLevel();
       // public  int GetHealth();
       // public abstract void Initialize();
         public  Sprite Sprite=>_sprite;
         public  bool IsLife=>_isLife;
       // public  int GetPrice();

        public  int GetMaxHealth()
        {
            return _minHealth;
        }

      

        public Action OnMove;
        public Action <Humanoid>OnInitialize;

        private bool _isSelected;
        private bool _isMoving;
        public bool IsMove => _isMoving;

        public string GetName() => ConstantsData.GetName(_id);


        public void ApplyDamage(int getDamage)
        {
            Debug.Log(_currentHealth);
            if (_currentHealth <= 0)
            {
                _animator.SetTrigger(_playerCharacterAnimController.Die);
                _isLife = false;
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
        
        public void Die()
        {
            PlayerCharactersStateMachine stateMachine = GetComponent<PlayerCharactersStateMachine>();
            stateMachine.EnterBehavior<DieState>();
        }

        public void Initialize(AudioManager audioManager)
        {
            _audioManager = audioManager;
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
                _ring.gameObject.SetActive(true);
            }
            else
            {
                _ring.gameObject.SetActive(false);
            }
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

    }
}