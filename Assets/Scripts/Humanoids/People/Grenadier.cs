using Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using Infrastructure.WeaponManagment;
using UnityEngine;

namespace Humanoids.People
{
    public class Grenadier:Humanoid
    {
        [SerializeField] private int maxHealth = 60;
        [SerializeField] private int level = 1;
        [SerializeField] private Sprite _sprite;
        private const int Price = 400;
        private const int Damage = 20;

        public int MaxHealth => maxHealth;
        public int Level
        {
            get => level;
            set => level = value;
        }

        private  int _minHealth = 0;
        private  int _maxHealth;
        
        private bool _isLife = true;
        private bool _isTakeDamagePlay;
        private int _currentHealth;

        private PlayerCharacterAnimController _playerCharacterAnimController;
        private Animator _animator;
        private FXController _fxController;
        private WeaponController _weaponController;
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _fxController = GetComponent<FXController>();
            Humanoid _humanoid = GetComponent<Humanoid>();
            _weaponController = _humanoid.GetWeaponController();
            _humanoid.OnLoadData += Initialize;
        }
        public override Sprite GetSprite()
        {
            return _sprite;
        }
        private void Initialize( )
        {
            _maxHealth= maxHealth;
            _currentHealth= _maxHealth;
        }

        public override int GetHealth()
        {
            return _currentHealth;
        }

        public override bool IsLife() => 
            _isLife;

        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            Price;

        
        public override void ApplyDamage(int getDamage)
        {
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
                    // нужно событие в гуманойде  когда принимает урон чтобы все действия остановить
                }
                
                _fxController.OnHitFX();
                _currentHealth -= Mathf.Clamp(getDamage, _minHealth, _maxHealth);
            }
           }

        public override void SetUpgrade(UpgradeInfo upgrade)
        {
            
            _currentHealth=upgrade.Health;
            _weaponController.SetDamage(upgrade.Damage);
            Level=upgrade.Level;
            
            OnLoadData?.Invoke();
        }

        public void TakeDamageEnd()
        {
            _isTakeDamagePlay=false;
        }
        
        public override int GetDamageDone() => 
            (int)Mathf.Round(_maxHealth - _currentHealth);
        
    }
}