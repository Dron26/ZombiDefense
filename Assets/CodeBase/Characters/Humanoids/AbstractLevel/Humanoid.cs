using System;
using Data.Upgrades;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.AIBattle.StateMachines.Humanoid;
using Infrastructure.AIBattle.StateMachines.Humanoid.States;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using Services.Audio;
using UnityEngine;

namespace Characters.Humanoids.AbstractLevel
{
    [RequireComponent(typeof(HumanoidWeaponController))]
    [RequireComponent(typeof(PlayerCharactersStateMachine))]
    public abstract class Humanoid : Character
    {
        private PlayerCharacterAnimController _playerCharacterAnimController;
        private FXController _fxController;
        private AudioManager AudioManager;
        public Vector3 StartPosition;
        private Animator _animator;
        public Action OnMove;

        private int _currentHealth;
        private bool _isTakeDamagePlay;
        private int _minHealth = 0;
        private bool _isBuyed = false;
        private bool _isWeaponInitialized = false;
        public bool IsBuyed => _isBuyed;
        public SkinContainer _skinContainer;
        public AudioManager GetAudioManager() => AudioManager;
        
        public override void Initialize()
        {
            _isWeaponInitialized = false;
            _currentHealth = base.CharacterData.Health;
            // _skinContainer = GetComponent<SkinContainer>();
            _skinContainer.SetSkin(CharacterData.Type);
            _animator = GetComponent<Animator>();
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _fxController = GetComponent<FXController>();
            SetController();
        }

        private void SetController()
        {
            string path = "Prefab/Store/Characters/Player/Animation/Controller/"+CharacterData.Type;
            Debug.Log("Loading controller from path: " + path);

            RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(path);
            if (controller == null)
            {
                Debug.LogError("Controller not found at path: " + path);
                return;
            }

            Debug.Log("Controller loaded successfully: " + controller.name);
            _animator.runtimeAnimatorController = controller;
        }

        private void OnWeaponInitialized(Weapon obj)
        {
            _isWeaponInitialized = true;
        }

        public override void SetAudioManager(AudioManager audioManager)
        {
            if (audioManager == null)
                throw new ArgumentNullException(nameof(audioManager));

            AudioManager = audioManager;
        }
        
        
        public override void ApplyDamage(int getDamage)
        {
            
            Debug.Log("_currentHealth");
            Debug.Log(_currentHealth);
            
            Debug.Log("getDamage");
            Debug.Log(getDamage);
            if (_currentHealth < 1)
            {
                _animator.SetTrigger(_playerCharacterAnimController.Die);
                base.Die();
                Die();
            }
            else
            {
                if (!_isTakeDamagePlay)
                {
                    _isTakeDamagePlay = true;
                    _animator.SetTrigger(_playerCharacterAnimController.IsHit);
                }

                //  _fxController.OnHitFX();
                _currentHealth -= Mathf.Clamp(getDamage, _minHealth, _currentHealth);
            }
        }


        protected override  void Die()
        {
            _fxController.OnAttackFXStop();
            PlayerCharactersStateMachine stateMachine = GetComponent<PlayerCharactersStateMachine>();
            stateMachine.EnterBehavior<DieState>();
            RaiseEntityEvent();
        }

        
        private void SetUpgradeFromPoint(int upPrecent) => _currentHealth += ( Health*upPrecent) / 100;

        public void SetPontInfo() { }

        public void IsMoving(bool isMoving)
        {
            base.IsMoving = isMoving;

            if (isMoving)
            {
                OnMove?.Invoke();
            }
        }

        public void SetAvailable(bool isAvailable)
        {
            _isBuyed = isAvailable;
        }

        public void UIInitialize() => _currentHealth = Health;

        protected void SetPoint(WorkPoint workPoint)
        {
            if (workPoint.IsHaveMedicineBox && _currentHealth < Health)
            {
                OpenMedicineBox(workPoint.GetMedicineBox());
            }
        }

        private void OpenMedicineBox(MedicalKit medicalKit)
        {
            if (medicalKit != null)
            {
                AddHealth(((Health * medicalKit.GetRecoveryRate()) / 100));
            }
        }

        private void AddHealth(int health)
        {
            _currentHealth = Mathf.Min(Health, _currentHealth + health);
        }
    }
}