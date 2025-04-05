using System;
using System.Collections;
using Data.Upgrades;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.AdditionalEquipment;
using Infrastructure.AIBattle.StateMachines.Humanoid;
using Infrastructure.AIBattle.StateMachines.Humanoid.States;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using Services;
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
        public Vector3 StartPosition;
        public int CurrentHealth=> _currentHealth;
        private Animator _animator;
        public Action OnMove;
        public SkinContainer _skinContainer;
        public bool IsBuyed => _isBuyed;
        private int _level=1;
        private int _currentHealth;
        private int _maxHealth;
        private bool _isTakeDamagePlay;
        private int _minHealth = 0;
        private bool _isBuyed = false;
        private bool _isWeaponInitialized = false;
        private bool _isRegenerating = false;
        private float _defencePercent = 0;
        private float _regeneratePrecent = 0;
        private WaitForSeconds timer;
        private IUpgradeTree _upgradeTree;
        private IAudioManager _audioManager;

        public override void Initialize()
        {
            _isWeaponInitialized = false;
            _currentHealth = base.CharacterData.Health;
            // _skinContainer = GetComponent<SkinContainer>();
            _skinContainer.SetSkin(CharacterData.Type);
            _animator = GetComponent<Animator>();
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _fxController = GetComponent<FXController>();
            _maxHealth= CharacterData.Health;
            timer= new WaitForSeconds(1f);
            _upgradeTree = AllServices.Container.Single<IUpgradeTree>();
            _audioManager=AllServices.Container.Single<IAudioManager>();
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

        public override void ApplyDamage(int damage)
        {
            
            Debug.Log("_currentHealth");
            Debug.Log(_currentHealth);
            Debug.Log("getDamage");
            Debug.Log(damage);
            
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
                
                int currentdamage=(int) Mathf.Round(damage*(100-_defencePercent)/100);
                _currentHealth -= Mathf.Clamp(currentdamage, _minHealth, _currentHealth);

                if (!_isRegenerating&&_currentHealth < _maxHealth)
                {
                    _isRegenerating = true;
                    StartCoroutine(StartRegeneration());
                }
            }
        }

        private IEnumerator StartRegeneration()
        {
            while(_currentHealth < _maxHealth)
            {
                _currentHealth += (int)Mathf.Round(_currentHealth *(_regeneratePrecent/ 100));
                yield return timer;
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

        public override void SetPoint(WorkPoint workPoint)
        {
            if (workPoint.IsHaveMedicineBox && _currentHealth < Health)
            {
                OpenMedicineBox(workPoint.GetMedicineBox());
            }

            _defencePercent = workPoint.DefencePercent;
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

        public void HealthLevelUp(int percent)
        {
            _maxHealth= Mathf.RoundToInt(_maxHealth * (1 + percent / 100));
        }
        private void SetUpgrades()
        {
            _regeneratePrecent=_upgradeTree.GetUpgradeValue(UpgradeGroupType.Health,UpgradeType.RestoreHealth)[0];
            _level = (int)_upgradeTree.GetUpgradeValue(UpgradeGroupType.Supplies, UpgradeType.IncreaseUnitLevel)[0];
        }

        
        
        public int GetLevel()
        {
            return _level;
        }
        
        
        
        public void RestoreHealth()
        {
            _currentHealth=_maxHealth;
        }
    }
}