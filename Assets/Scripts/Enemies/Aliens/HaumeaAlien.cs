using Enemies.AbstractEntity;
using Infrastructure.AIBattle;
using UnityEngine;

namespace Enemies.Aliens
{
    public class HaumeaAlien : Alien
    {
        private readonly float _maxHealth = 60f;
        private readonly float _minHealth = 0;
        
        private readonly float _rangeAttack = 1.2f;
        private readonly int _damage = 15;
        
        private float _health = 60f;
        private bool _isLife = true;

        private Animator _animator;
        private HashAnimator _hashAnimator;
        private FXController _fxController;
        private int Level=3;
        public override int GetLevel() => 
            Level;
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _hashAnimator = GetComponent<HashAnimator>();
            _fxController = GetComponent<FXController>();
        }
        
        public override void ApplyDamage(int getDamage)
        {
            if (_health <= 0)
            {
                _animator.SetTrigger(_hashAnimator.Die);
                _fxController.OnDieFX();
                _isLife = false;
            }
            
            _fxController.OnHitFX();
            _animator.SetTrigger(_hashAnimator.IsHit);
            _health -= Mathf.Clamp(getDamage, _minHealth, _maxHealth);
        }


        public override float GetRangeAttack() =>
            _rangeAttack;

        public override int GetDamage() => 
            _damage;

        public override float GetHealth() =>
            _health;

        public override bool IsLife() => 
            _isLife;
        
        public override int GetPrice()
        {
            throw new System.NotImplementedException();
        }
        
        public override void SetAttacments()
        {
            throw new System.NotImplementedException();
        }

        public override void Initialize()
        {
            throw new System.NotImplementedException();
        }
    }
}