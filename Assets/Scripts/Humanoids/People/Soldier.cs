using Humanoids.AbstractLevel;
using Infrastructure.AIBattle;
using UnityEngine;

namespace Humanoids.People
{
    public class Soldier : Humanoid
    {
        private const int Level = 1;
        private const int Price = 1;
        private const int Damage = 20;
        

        private readonly float _minHealth = 0f;
        private  float _maxHealth;
        
        private bool _isLife = true;
        private bool _isTakeDamagePlay;
        private float _health;

        private PlayerCharacterAnimController _playerCharacterAnimController;
        private Animator _animator;
        private FXController _fxController;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _fxController = GetComponent<FXController>();
            Humanoid _humanoid = GetComponent<Humanoid>();
            _humanoid.OnLoadData += Initialize;
        }

        private void Initialize( )
        {
            _maxHealth= MaxHealth;
            _health= _maxHealth;
        }

        public override float GetHealth()
        {
            return _health;
        }

        public override bool IsLife() => 
            _isLife;

        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            Price;

       

        public override void ApplyDamage(int getDamage)
        {
            if (_health <= 0)
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
                _health -= Mathf.Clamp(getDamage, _minHealth, _maxHealth);
            }
           }

        public void TakeDamageEnd()
        {
            _isTakeDamagePlay=false;
        }
        
        public override int GetDamageDone() => 
            (int)Mathf.Round(_maxHealth - _health);
    }
}