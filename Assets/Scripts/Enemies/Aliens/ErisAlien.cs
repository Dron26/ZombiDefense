﻿using Enemies.AbstractEntity;
using Infrastructure.AIBattle;
using Infrastructure.AssetManagement;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies.Aliens
{
    public class ErisAlien : Alien
    {
        private  float _maxHealth  =>MaxHealth;
        private readonly float _minHealth = 0;
        private readonly float _rangeAttack = 1.2f;
        private readonly int _damage = 15;

        private float _health ;
        private bool _isLife = true;
        private Animator _animator;
        private AnimController _animController;
        private FXController _fxController;

        private void Awake()
        {   
            
            _animator = GetComponent<Animator>();
            _animController = GetComponent<AnimController>();
            _fxController = GetComponent<FXController>();
        }

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
            _health = _maxHealth;
        }

        public override void ApplyDamage(float getDamage)
        {
            
            if (_health >= 0)
            {
                if (Level== 4&&getDamage>30)
                {
                    _animator.SetBool(_animController.Walk, false);
                    _animator.SetTrigger(_animController.IsHit);
                }
                
                _fxController.OnHitFX();
              //  _animator.SetTrigger(_animController.IsHit);
                _health -= Mathf.Clamp(getDamage, _minHealth, _maxHealth);
               
            }
            
            if(_health <= 0)
            {
                _animator.SetTrigger(_animController.Die);
                _fxController.OnDieFX();
                _isLife = false;
                Die();
             }
            
            // if (_health > 0)
            // {
            //     //_animator.SetTrigger(_animController.IsHit);
            //     _fxController.OnHitFX();
            //     _health -= Mathf.Clamp(getDamage, _minHealth, _maxHealth);
            // }
            // else
            // {
            //     _animator.SetTrigger(_animController.Die);
            // //    _fxController.OnDieFX();
            //     _isLife = false;
            //     
            //     
            // }
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