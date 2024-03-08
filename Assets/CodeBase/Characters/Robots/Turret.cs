using System;
using System.Collections;
using Characters.Humanoids.AbstractLevel;
using Data.Upgrades;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.PlayerCharacterStateMachine;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Service.Audio;
using UnityEngine;

namespace Characters.Robots
{
    [RequireComponent(typeof(RobotFXController))]
    public  class Turret : MonoCache
    {
        [SerializeField] private int _id;
        [SerializeField] private int _maxHealth;
        [SerializeField] private ParticleSystem _shotRadius;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private GameObject _gun;

        [SerializeField] private int _damage;
        [SerializeField] private float timeShotsInterval;
        private WaitForSeconds _shotsInterval;
        private AudioManager _audioManager;
        public Vector3 StartPosition;
        public Action<Humanoid> OnDataLoad;
        private RobotFXController _fxController;
        private int _currentHealth;
        private bool _isLife = true;
        private bool _isTakeDamagePlay;
        public int MaxHealth => _maxHealth;
        private int _minHealth = 0;
        private bool _isBuyed = false;
        public Action<Turret> OnInitialize;
        private bool _isSelected;
        
        private bool _isTargetSet;
        private bool _isAutoFind;
        public void Initialize(AudioManager audioManager)
        {
            _audioManager = audioManager;
            _currentHealth = _maxHealth;
            _fxController = GetComponent<RobotFXController>();
            OnInitialize?.Invoke(this);
            _shotsInterval = new WaitForSeconds(timeShotsInterval);
            _isAutoFind=false;
        }

        public void Start()
        {
           // _audioManager = audioManager;
            _currentHealth = _maxHealth;
            _fxController = GetComponent<RobotFXController>();
            OnInitialize?.Invoke(this);
            _shotsInterval = new WaitForSeconds(timeShotsInterval);
        }
    

        private void OnTriggerEnter(Collider other)
        {
            
                
            if (other.TryGetComponent<Enemy>(out Enemy enemy)&&_isTargetSet==false)
            {
                enemy.OnDeath+= StopAttack;
                StartCoroutine(StartAttack(enemy));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            
            if (other.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.OnDeath-= StopAttack;
                StopAttack(enemy);
            }
        }
        private IEnumerator StartAttack(Enemy enemy)
        {
            _isTargetSet = true;
            
            while (enemy.IsLife()&&_isTargetSet)
            {
                if (_isAutoFind)
                {
                    _gun.transform.LookAt(enemy.transform);
                }
                
                _fxController.OnAttackFX();
                enemy.ApplyDamage(_damage, WeaponType.Turret);
                _shotsInterval = new WaitForSeconds(timeShotsInterval);
                yield return _shotsInterval;
            }
        }

        private void StopAttack(Enemy enemy)
        {
            _isTargetSet = false;
            _fxController.OnAttackFXStop();
            StopCoroutine(StartAttack(enemy));
        }

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
            _maxHealth += (_maxHealth * upPrecent) / 100;
        }

        public void SetAvailable(bool isBuyed)
        {
            isBuyed = isBuyed;
        }
        
        public void SetUpgrade(UpgradeData upgrade, int level)
        {
            _maxHealth += upgrade.Health;
            _currentHealth = _maxHealth;
            _damage =10;
            _shotsInterval = new WaitForSeconds(0.3f);
            _isAutoFind=false;
        }
        
        private void AddHealth(int health)
        {
            _currentHealth += _maxHealth;
        }
        
    }
}
