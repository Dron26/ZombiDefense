using System;
using Characters.Humanoids.AbstractLevel;
using Data.Upgrades;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.StateMachines.Humanoid;
using Infrastructure.AIBattle.StateMachines.Humanoid.States;
using Services.Audio;
using Services.SaveLoad;
using UnityEngine;

namespace Characters.Robots
{
    [RequireComponent(typeof(TurretWeaponController))]
    [RequireComponent(typeof(RobotFXController))]
    [RequireComponent(typeof(TurretStateMachine))]
    [RequireComponent(typeof(RaycastHitChecker))]
    public class Turret : Character
    {
        private RobotFXController _fxController;
        private AudioManager _audioManager;
        public Vector3 StartPosition;
        public Action<Humanoid> OnDataLoad;
        private int _currentHealth;
        private int _maxHealth;
        private bool _isTakeDamagePlay;
        private int _minHealth = 0;
        private bool _isBuyed = false;
        public Action<Turret> OnInitialize;
        public bool IsBuyed => _isBuyed;
        private RaycastHitChecker _raycastHitChecker;
        private TurretWeaponController _turretWeaponController;

        public void SetSaveLoadService()
        {
            _raycastHitChecker = GetComponent<RaycastHitChecker>();
            if (_raycastHitChecker != null)
            {
                _raycastHitChecker.Initialize();
            }
            else
            {
                Debug.LogError("RaycastHitChecker component is missing on Turret.");
            }
        }

        public override void ApplyDamage(int getDamage)
        {
            if (_currentHealth <= 0)
            {
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

        protected override void Die()
        {
            base.Die(); 
            PlayerCharactersStateMachine stateMachine = GetComponent<PlayerCharactersStateMachine>();
            if (stateMachine != null)
            {
                stateMachine.EnterBehavior<DieState>();
            }
            else
            {
                Debug.LogError("PlayerCharactersStateMachine component is missing on Turret.");
            }
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
            _isBuyed = isBuyed; 
        }

        public override void Initialize()
        {
            _turretWeaponController = GetComponent<TurretWeaponController>();
            _fxController = GetComponent<RobotFXController>();
            TurretStateMachine stateMachine = GetComponent<TurretStateMachine>();
            if (stateMachine != null)
            {
                stateMachine.Initialize(_raycastHitChecker, _fxController, _turretWeaponController);
            }
            else
            {
                Debug.LogError("TurretStateMachine component is missing on Turret.");
            }

            _maxHealth = Health;
            _currentHealth = _maxHealth;
            OnInitialize?.Invoke(this);
        }

        public override void SetAudioManager(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        private void AddHealth(int health)
        {
            _currentHealth += health;
        }

        // Удален метод UIInitialize, так как он не используется
    }
}