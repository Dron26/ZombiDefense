using System;
using Characters.Humanoids.AbstractLevel;
using Data.Upgrades;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Service.Audio;
using Service.SaveLoad;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Characters.Robots
{
    [RequireComponent(typeof(TurretWeaponController))]
    [RequireComponent(typeof(RobotFXController))]
    [RequireComponent(typeof(TurretStateMachine))]
    [RequireComponent(typeof(RaycastHitChecker))]
    public  class Turret : Character
    {
        private RobotFXController _fxController;
        private AudioManager _audioManager;
        public Vector3 StartPosition;
        public Action<Humanoid> OnDataLoad;
        private int _currentHealth;
        private int _maxHealth;
        private bool _isLife = true;
        private bool _isTakeDamagePlay;
        private int _minHealth = 0;
        private bool _isBuyed = false;
        public Action<Turret> OnInitialize;
        public bool IsBuyed => _isBuyed;
        private RaycastHitChecker _raycastHitChecker;
        private TurretWeaponController _turretWeaponController;
        
        public void Initialize(AudioManager audioManager,SaveLoadService saveLoadService)
        {
            _audioManager=audioManager;
            _raycastHitChecker = GetComponent<RaycastHitChecker>();
            _raycastHitChecker.Initialize(saveLoadService);
            _turretWeaponController= GetComponent<TurretWeaponController>();
            _fxController = GetComponent<RobotFXController>();
            GetComponent<TurretStateMachine>().Initialize(_raycastHitChecker,_fxController,_turretWeaponController);
            _maxHealth = Health;
            _currentHealth=_maxHealth;
            OnInitialize?.Invoke(this);
        }

        public override void ApplyDamage(int getDamage)
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

        public AudioManager GetAudioController()
        {
            return _audioManager;
        }
        
        private void SetUpgradeFromPoint(int upPrecent)
        {
            _currentHealth += (_maxHealth * upPrecent) / 100;
        }

        public void SetAvailable(bool isBuyed)
        {
            isBuyed = isBuyed;
        }
        
        public override void SetUpgrade(UpgradeData upgrade, int level)
        {
            _maxHealth += upgrade.Health;
            _currentHealth = _maxHealth;
        }
        
        private void AddHealth(int health)
        {
            _currentHealth += health;
        }

        public void UIInitialize()
        {
            throw new NotImplementedException();
        }
    }
}