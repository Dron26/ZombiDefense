using Enemies.AbstractEntity;
using Infrastructure.AIBattle;
using UnityEngine;

namespace Enemies.Aliens
{
    public class CeresAlien : Alien
    {
        private readonly float _maxHealth = 70f;
        private readonly float _minHealth = 0;
        
        private readonly float _rangeAttack = 1.2f;
        private readonly int _damage = 15;
        
        private float _health = 70f;
        private bool _isLife = true;

        private Animator _animator;
        private HashAnimator _hashAnimator;
        private FXController _fxController;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _hashAnimator = GetComponent<HashAnimator>();
            _fxController = GetComponent<FXController>();
        }

        public override int GetPrice()
        {
            throw new System.NotImplementedException();
        }

        public override void ApplyDamage(int getDamage)
        {
            if (_health >= 0)
            {
                _fxController.OnHitFX();
                _animator.SetTrigger(_hashAnimator.IsHit);
                _health -= Mathf.Clamp(getDamage, _minHealth, _maxHealth);
               
            }
            
            if(_health <= 0)
            {
                _animator.SetTrigger(_hashAnimator.Die);
                _fxController.OnDieFX();
                _isLife = false;
            }
        }

        public override void SetAttacments()
        {
            throw new System.NotImplementedException();
        }

        public override void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public override int GetLevel()
        {
            throw new System.NotImplementedException();
        }

        public override float GetRangeAttack() =>
            _rangeAttack;

        public override int GetDamage() => 
            _damage;

        public override float GetHealth() =>
            _health;

        public override bool IsLife() => 
            _isLife;
    }
}