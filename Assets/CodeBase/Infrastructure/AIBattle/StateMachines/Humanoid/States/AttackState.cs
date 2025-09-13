using System.Collections.Generic;
using DG.Tweening;
using Enemies.AbstractEntity;
using Infrastructure.Logic.WeaponManagment;
using Interface;
using Services;
using UnityEngine;

namespace Infrastructure.AIBattle.StateMachines.Humanoid.States
{
    public class AttackState : State
    {
        private readonly WaitForSeconds _waitForSeconds = new(0.1f);
        private Entity _targetEnemy;
        private Transform _targetTransform;
        private float _currentRange;

        private PlayerCharacterAnimController _animController;
        private FXController _fxController;
        private Characters.Humanoids.AbstractLevel.Humanoid _humanoid;
        private HumanoidWeaponController _weaponController;

        private bool _isSpecialWeapon;
        private bool _isAttacking;
        private bool _isReloading;
        private bool _isMove;

        private List<Enemy> _enemiesInRange = new();
        private int _maxAmmo;
        private int _ammoCount;
        private float _damage;
        private float _range;
        private float _maxRange;
        private float[] _radiusList;
        private float[] _damageList;
        private float _accumulationDamage;
        private ItemType _weaponType;

        protected  void Awake()
        {
            _animController = GetComponent<PlayerCharacterAnimController>();
            _fxController = GetComponent<FXController>();
            _humanoid = GetComponent<Characters.Humanoids.AbstractLevel.Humanoid>();
            _weaponController = GetComponent<HumanoidWeaponController>();
            _weaponController.UpdateWeaponData += OnUpdateWeaponData;
        }

        protected override void OnEnabled()
        {
            Debug.Log("OnEnabledAttack()");
            if (!_isAttacking && !_isReloading)
            {
                Attack();
            }
        }

        public void InitEnemy(Entity targetEnemy)
        {
            if (targetEnemy == null) return;
            _targetEnemy = targetEnemy;
            _targetTransform = targetEnemy.transform;
        }

        private void Attack()
        {
            Debug.Log("Attack()");

            if (_targetEnemy == null || !_targetEnemy.IsLife())
            {
                StopFX();
                PlayerCharactersStateMachine.EnterBehavior<SearchTargetState>();
                return;
            }

            _currentRange = Vector3.Distance(transform.position, _targetTransform.position);

            if (_ammoCount <= 0 && !_isReloading)
            {
                Reload();
                return;
            }

            if (!_isAttacking && !_isReloading && _currentRange <= _range)
            {
                _isAttacking = true;
                _accumulationDamage = 0;

                _animController.OnShoot(true);

                float lookTime = (_weaponType == ItemType.Medium || _weaponType == ItemType.Flammer) ? 0.3f : 0.1f;
                transform.DOLookAt(_targetTransform.position, lookTime);

                if (_weaponType == ItemType.Medium || _weaponType == ItemType.Flammer)
                    _fxController.OnAttackFX();
            }
        }

        public void FinishAnimationAttackPlay()
        {
            _isAttacking = false;
            _ammoCount--;

            if (_isSpecialWeapon)
                ApplyDamageToEnemiesInRange();
            else if (_targetEnemy != null && _targetEnemy.IsLife() && _currentRange <= _range)
            {
                _targetEnemy.ApplyDamage(_damage, _weaponType);
            }
            
            if (_ammoCount <= 0 && !_isReloading && !_isMove)
                Reload();
            if (enabled)
            {
                Debug.Log("enabledAttack()");

                Attack();
            }
        }

        private void Reload()
        {
            _isReloading = true;
            _isAttacking = false;

            _animController.OnShoot(false);
            _animController.ReloadWeapon(true);
        }

        public void OnReloadEnd()
        {
            _ammoCount = _maxAmmo;
            _isReloading = false;
            _animController.ReloadWeapon(false);
            if (enabled)
            {
                Attack();
            }
        }

        private void ApplyDamageToEnemiesInRange()
        {
            Vector3 attackDir = _targetTransform.position - transform.position;

            _enemiesInRange = AllServices.Container.Single<ISearchService>()
                .GetEntitiesInRange<Enemy>(transform.position, _maxRange);

            foreach (var enemy in _enemiesInRange)
            {
                if (!enemy.IsLife()) continue;

                Vector3 dirToEnemy = enemy.transform.position - transform.position;
                float angleToEnemy = Vector3.Angle(attackDir, dirToEnemy);

                if (angleToEnemy <= _weaponController.SpreadAngle)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    float damagePercent = CalculateDamagePercent(distance);

                    if (_weaponType != ItemType.Medium)
                        enemy.ApplyDamage(_damage * damagePercent, _weaponType);
                    else
                    {
                        _accumulationDamage += _damage;
                        enemy.ApplyDamage(_accumulationDamage, _weaponType);
                    }
                }
            }

            Attack();
        }

        private float CalculateDamagePercent(float distance)
        {
            for (int i = 0; i < _radiusList.Length; i++)
                if (distance <= _radiusList[i])
                    return _damageList[i];

            return 0;
        }

        private void OnUpdateWeaponData()
        {
            _weaponType = _weaponController.ItemType;
            _maxAmmo = _weaponController.MaxAmmo;
            _ammoCount = _maxAmmo;
            _damage = _weaponController.Damage;
            _range = _weaponController.Range;

            if (_weaponController.SpreadAngle > 0)
            {
                _isSpecialWeapon = true;
                _radiusList = new[]
                {
                    _weaponController.Range * 0.4f,
                    _weaponController.Range * 0.6f,
                    _weaponController.Range
                };
                _damageList = new[] { 1.3f, 1f, 0.5f };
                _maxRange = _weaponType == ItemType.Medium ? _radiusList[2] * 2 : _radiusList[2];
            }
            else
            {
                _isSpecialWeapon = false;
            }
        }

        private void StopFX()
        {
            if (_weaponType == ItemType.Medium || _weaponType == ItemType.Flammer)
                _fxController.OnAttackFXStop();
        }

        protected override void OnDisable()
        {
            Debug.Log("OnDisableAttack()");
            base.OnDisable();
            _isAttacking = false;
            _isReloading = false;
            _targetEnemy = null;
            _targetTransform = null;
            _animController.OnShoot(false);
            if (_weaponController != null)
                _weaponController.UpdateWeaponData -= OnUpdateWeaponData;
        }

        public void StartMove()
        {
            if (_isAttacking || _isReloading)
            {
                _animController.ReloadWeapon(false);
                _isReloading = false;
                _animController.OnShoot(false);
                _isMove = true;
                enabled = false; // временно блокируем state
            }
        }
    }
}
