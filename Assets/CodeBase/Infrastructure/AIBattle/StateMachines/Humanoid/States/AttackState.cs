﻿using System;
using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public class AttackState : State
    {
        private readonly WaitForSeconds _waitForSeconds = new(0.1f);

        private Enemy _enemy = null;

        private float _currentRange;

        private PlayerCharacterAnimController _playerCharacterAnimController;
        private FXController _fxController;
        private Humanoid _humanoid;
        private HumanoidWeaponController _humanoidWeaponController;
        private bool _isShotgun;

        //private bool _isAttacked;
        private bool _isAttacking;
        private bool _isReloading;
        private Weapon _activeWeapon;

        private int _maxAmmo;

        //  private float _reloadTime;
        private float _fireRate;
        private float _damage;
        private float _range;
        public int _ammoCount;

        float _firstRadius;
        float _secondRadius;
        float _thirdRadius;
        private float[] _radiusList;
        private float[] _damageList;
        private float _maxRadius;
        private bool _isTargetSet;
        
        private void Awake()
        {
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _fxController = GetComponent<FXController>();
            _humanoid = GetComponent<Humanoid>();
            _humanoidWeaponController = GetComponent<HumanoidWeaponController>();
            _humanoidWeaponController.ChangeWeapon += OnWeaponChanged;
        }

        private void Start()
        { 
            PlayerCharactersStateMachine.OnStartMove += StartMove;
        }

        protected override void OnEnabled()
        {
            Debug.Log("OnEnabled()");
            if (_isAttacking == false & _isReloading == false)
            {
                Attack();
            }
        }

        public void InitEnemy(Enemy targetEnemy)
        {
            Debug.Log("InitEnemy()");
            _enemy = targetEnemy;
            _isTargetSet = true;

            if (_isAttacking == false & _isReloading == false)
            {
                Attack();
            }
        }

        private void Attack()
        {
            Debug.Log("Attack()");
            if (_enemy.IsLife())
            {
                if (_ammoCount == 0 && _isReloading == false)
                {
                    Debug.Log("Reload()");
                    Reload();
                }
                
                if (_isAttacking == false & _isReloading == false)
                {
                    _currentRange = Vector3.Distance(transform.position, _enemy.transform.position);
                    float rangeAttack = _humanoidWeaponController.GetRangeAttack();

                    if (_currentRange <= rangeAttack & _ammoCount > 0)
                    {
                        _isAttacking = true;
                        _isTargetSet = false;

                        _playerCharacterAnimController.OnShoot(true);
                    }
                }
            }
            else
            {
                ChangeState<SearchTargetState>();
            }
        }

        public void FinishAnimationAttackPlay()
        {
            _isAttacking = false;
            _ammoCount--;

            if (_isShotgun)
            {
                ApplyDamageToEnemiesInRange();
            }
            else
                _enemy.ApplyDamage(_damage, _humanoidWeaponController.ItemType);

            if (_ammoCount == 0 & _isReloading == false)
            {
                Reload();
            }
            else
            {
                Attack();
            }

        }

        private void Reload()
        {
            _isReloading = true;
            _isAttacking = false;

            _playerCharacterAnimController.OnShoot(false);
            _playerCharacterAnimController.ReloadWeapon(true);
        }

        public void OnReloadEnd()
        {
            Debug.Log("OnReloadEnd()");
            _ammoCount = _maxAmmo;
            _isReloading = false;
            _playerCharacterAnimController.ReloadWeapon(false);
            Attack();
        }

        private void ChangeState<TState>() where TState : State
        {
            Debug.Log("AttackChangeState()");
            PlayerCharactersStateMachine.EnterBehavior<TState>();
        }

        private void ApplyDamageToEnemiesInRange()
        {
            float angle = _humanoidWeaponController.GetSpreadAngle();
            Vector3 attackDirection = _enemy.transform.position - transform.position;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _maxRadius, LayerMask.GetMask("Enemy"));

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out Enemy enemy))
                {
                    if (enemy.IsLife())
                    {
                        Vector3 directionToEnemy = enemy.transform.position - transform.position;
                        float angleToEnemy = Vector3.Angle(attackDirection, directionToEnemy);

                        // Проверяем, находится ли враг внутри угла атаки
                        if (angleToEnemy <= angle)
                        {
                            float distance = Vector3.Distance(transform.position, enemy.transform.position);
                            float damagePercent = 0;

                            for (int i = 0; i < _radiusList.Length; i++)
                            {
                                if (distance <= _radiusList[i])
                                {
                                    damagePercent = _damageList[i];
                                    break;
                                }
                            }

                            enemy.ApplyDamage(_humanoidWeaponController.Damage * damagePercent,
                                _humanoidWeaponController.ItemType); // применяем урон
                        }
                    }
                }
            }
        }

        private void OnWeaponChanged()
        {
            _activeWeapon = _humanoidWeaponController.GetActiveItemData();
            _isShotgun = _activeWeapon.IsShotgun;
            _maxAmmo = _activeWeapon.MaxAmmo;
            _ammoCount = _maxAmmo;
            //  _reloadTime = _weaponController.ReloadTime;
            _fireRate = _activeWeapon.FireRate;
            _range = _activeWeapon.Range;
            _damage = _humanoidWeaponController.Damage;

            if (_isShotgun)
            {
                _firstRadius = _humanoidWeaponController.GetSpread();
                _secondRadius = _humanoidWeaponController.GetSpread() * 0.6f;
                _thirdRadius = _humanoidWeaponController.GetSpread() * 0.3f;

                _radiusList = new[] { _firstRadius, _secondRadius, _thirdRadius };

                _damageList = new[] { 1.3f, 1f, _damage * 0.026f };
                _maxRadius = _radiusList[0];
            }
        }

        public override void ExitBehavior()
        {
            Debug.Log("ExitBehavior()");
            enabled = false;
        }

        protected override void OnDisable()
        {
            Debug.Log("OnDisable()");
            _isAttacking = false;
            _isTargetSet = false;
            _playerCharacterAnimController.OnShoot(false);
            enabled = false;
        }

        private void StartMove()
        {
            Debug.Log("StartMove()");
            if (_isAttacking == true||_isReloading==true)
            {
                _playerCharacterAnimController.ReloadWeapon(false);
                _isReloading = false;
                _playerCharacterAnimController.OnShoot(false);
                OnDisable();
            }
        }
    }
} 
