using Humanoids.AbstractLevel.SimpleWarriors;
using Infrastructure.AIBattle;
using UnityEngine;

namespace Humanoids.People
{
    public class Knight : PeopleMen
    {
        private const int Level = 3;
        private const int Price = 16;
        private const int Damage = 15;
        
        private const float RangeAttack = 10f;

        private readonly float _minHealth = 0f;
        private readonly float _maxHealth = 40f;

        private bool _isLife = true;
        
        private float _health = 40f;

        private AnimController _animController;
        private Animator _animator;
        private FXController _fxController;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _animController = GetComponent<AnimController>();
            _fxController = GetComponent<FXController>();
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
                _animator.SetTrigger(_animController.Die);
                _fxController.OnDieFX();
                _isLife = false;
            }
            
            _fxController.OnHitFX();
            _animator.SetTrigger(_animController.IsHit);
            _health -= Mathf.Clamp(getDamage, _minHealth, _maxHealth);
        }
        
        public override float GetHealth()
        {
            return _health;
        }
        
        public override int GetDamageDone() => 
            (int)Mathf.Round(_maxHealth - _health);
    }
}