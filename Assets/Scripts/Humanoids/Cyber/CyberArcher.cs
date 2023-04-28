﻿using Humanoids.AbstractLevel.SimpleWarriors;
using Infrastructure.AIBattle;
using UnityEngine;

namespace Humanoids.Cyber
{
    public class CyberArcher : CyberMen
    {
        private const int Level = 6;
        private const int Price = 256;
        private const int Damage = 15;
        
        private const float RangeAttack = 10f;
        
        private readonly float _minHealth = 0f;
        private readonly float _maxHealth = 100f;
        
        private bool _isLife = true;

        private float _health = 100f;
        private int _totalReceivedDamage;
        private Animator _animator;
        private HashAnimator _hashAnimator;
        private FXController _fxController;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _hashAnimator = GetComponent<HashAnimator>();
            _fxController = GetComponent<FXController>();
        }
        
        public override float GetRangeAttack() =>
            RangeAttack;

        public override bool IsLife() => 
            _isLife;

        public override int GetLevel() => 
            Level;

        public override int GetPrice() =>
            Price;

        public override int GetDamage()
        {
            _totalReceivedDamage += Damage;
            return Damage;
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

        public override int GetDamageDone() => 
            (int)Mathf.Round(_maxHealth - _health);

        public override int DamageReceived() =>
            _totalReceivedDamage;

        public override int TotalPoints() => 
            _totalReceivedDamage + (int)Mathf.Round(_maxHealth - _health);
    }
}