using System;
using Characters.Humanoids.AbstractLevel;
using Data.Upgrades;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.StateMachines.Humanoid;
using Infrastructure.AIBattle.StateMachines.Humanoid.States;
using Infrastructure.AssetManagement;
using Interface;
using Services;
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
        [SerializeField] private bool _isCarTurret;
        private RobotFXController _fxController;
        private IAudioManager _audioManager;
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
        private TurretStateMachine _stateMachine;

        private void Start()
        {
            AllServices.Container.Single<IGameEventBroadcaster>().OnActivatedSpecialTechnique += ActiveTurret;
        }

        public override void Initialize()
        {
            _turretWeaponController = GetComponent<TurretWeaponController>();
            _fxController = GetComponent<RobotFXController>();
            
             _stateMachine = GetComponent<TurretStateMachine>();
            if (_stateMachine != null&&!_isCarTurret)
            {
                _stateMachine.Initialize(_raycastHitChecker, _fxController, _turretWeaponController,_isCarTurret);
            }
            else
            {
                Debug.LogError("TurretStateMachine component is missing on Turret.");
            }

            _audioManager = AllServices.Container.Single<IAudioManager>();
            _maxHealth = Health;
            _currentHealth = _maxHealth;
            OnInitialize?.Invoke(this);
        }

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

        private void SetUpgradeFromPoint(int upPrecent)
        {
            _currentHealth += (_maxHealth * upPrecent) / 100;
        }

        public void SetAvailable(bool isBuyed)
        {
            _isBuyed = isBuyed; 
        }
        

        private void AddHealth(int health)
        {
            _currentHealth += health;
        }
        
        public void ActiveTurret()
        {
            string path = AssetPaths.CharactersData + CharacterType.Turret;
            CharacterData data = Resources.Load<CharacterData>(path);
            SetSaveLoadService();
            Initialize(data);
            _isCarTurret = true;
            _stateMachine.CarTurretActive();
        }
    }
}