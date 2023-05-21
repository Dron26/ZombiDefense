using Enemies.AbstractEntity;
using Infrastructure.AIBattle;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.AssetManagement;
using Service.SaveLoadService;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies.Aliens
{
    public class ErisAlien : Enemy
    {
        private float _maxHealth => MaxHealth;
        private readonly float _minHealth = 0;
        private readonly float _rangeAttack = 1.2f;
        private readonly int _damage = 15;

        private float _health;
        private bool _isLife = true;
        private Animator _animator;
        private EnemyAnimController _enemyAnimController;
        private EnemyFXController _fxController;
        private SaveLoad _saveLoad;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _enemyAnimController = GetComponent<EnemyAnimController>();
            _fxController = GetComponent<EnemyFXController>();

            EnemyDieState enemyDieState = GetComponent<EnemyDieState>();
            enemyDieState.OnRevival += OnRevival;
        }

        private void OnRevival(Enemy enemy)
        {
            Initialize();
        }

        public override int GetPrice()
        {
            throw new System.NotImplementedException();
        }

        public override void SetAttacments()
        {
            throw new System.NotImplementedException();
        }

        public override void SetSaveLoad(SaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
        }

        public override void Initialize()
        {
            _isLife = true;
            _health = _maxHealth;
        }

        public override void ApplyDamage(float getDamage, string weaponName)
        {
            if (_health >= 0)
            {
                if (Level == 4 && getDamage > 30)
                {
                    _animator.SetBool(_enemyAnimController.Walk, false);
                    _animator.SetTrigger(_enemyAnimController.IsHit);
                }

                _fxController.OnHitFX(weaponName);
              //  _animator.SetTrigger(_enemyAnimController.IsHit);
                _health -= Mathf.Clamp(getDamage, _minHealth, _maxHealth);
            }

            if (_health <= 0)
            {
                _animator.SetTrigger(_enemyAnimController.Die);
               // _fxController.OnDieFX();
                _saveLoad.SetInactiveEnemy(this);
                Die();
                _isLife = false;
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