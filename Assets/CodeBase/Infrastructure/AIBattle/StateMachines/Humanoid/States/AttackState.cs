using System.Collections.Generic;
using DG.Tweening;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.Logic.WeaponManagment;
using Interface;
using Service;
using UnityEngine;

namespace Infrastructure.AIBattle.StateMachines.Humanoid.States
{
    public class AttackState : State
    {
        private readonly WaitForSeconds _waitForSeconds = new(0.1f);

        private Entity _enemy;
        private Transform _transformEnemy;
        private float _currentRange;

        private PlayerCharacterAnimController _playerCharacterAnimController;
        private FXController _fxController;
        private Characters.Humanoids.AbstractLevel.Humanoid _humanoid;
        private HumanoidWeaponController _humanoidWeaponController;
        private bool _isShotgun;
        private SphereCollider _attackTrigger;
        private bool _isAttacking;
        private bool _isReloading;
        private Weapon _activeWeapon;
        private List<Enemy> _enemiesInRange = new();
        private int _maxAmmo;
        private int _ammoCount;

        private float _damage;
        private float _range;
        private float _maxRadius;
        private float[] _radiusList;
        private float[] _damageList;

        private bool _isTargetSet;

        private void Awake()
        {
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
            _fxController = GetComponent<FXController>();
            _humanoid = GetComponent<Characters.Humanoids.AbstractLevel.Humanoid>();
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
            if (!_isAttacking && !_isReloading)
            {
                Attack();
            }
        }

        public void InitEnemy(Entity targetEnemy)
        {
            Debug.Log("InitEnemy()");
            _enemy = targetEnemy;
            _transformEnemy = _enemy.transform;
            _isTargetSet = true;

            if (!_isAttacking && !_isReloading)
            {
                Attack();
            }
        }

        private void Attack()
        {
            Debug.Log("Attack()");
            if (_enemy.IsLife())
            {
                if (_ammoCount == 0 && !_isReloading)
                {
                    Reload();
                }
                else if (!_isAttacking && !_isReloading)
                {
                    _currentRange = Vector3.Distance(transform.position, _transformEnemy.position);
                    if (_currentRange <= _range && _ammoCount > 0)
                    {
                        _isAttacking = true;
                        _isTargetSet = false;

                        transform.DOLookAt(_transformEnemy.position, 0.1f);
                        _playerCharacterAnimController.OnShoot(true);
                    }
                }
            }
            else
            {
                if (_activeWeapon.ItemType == ItemType.Medium)
                {
                    _fxController.OnAttackFXStop();
                }

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
            {
                _enemy.ApplyDamage(_damage, _humanoidWeaponController.ItemType);
            }

            if (_ammoCount == 0 && !_isReloading)
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
            _enemiesInRange=AllServices.Container.Single<ISearchService>().GetEntitiesInRange<Enemy>(transform.position, _maxRadius);
            foreach (var enemy in _enemiesInRange)
            {
                if (enemy.IsLife())
                {
                    Vector3 directionToEnemy = enemy.transform.position - transform.position;
                    float angleToEnemy = Vector3.Angle(attackDirection, directionToEnemy);

                    if (angleToEnemy <= angle)
                    {
                        float distance = Vector3.Distance(transform.position, enemy.transform.position);
                        float damagePercent = CalculateDamagePercent(distance);

                        enemy.ApplyDamage(_damage * damagePercent, _humanoidWeaponController.ItemType);
                    }
                }
            }
        }

        private float CalculateDamagePercent(float distance)
        {
            for (int i = 0; i < _radiusList.Length; i++)
            {
                if (distance <= _radiusList[i])
                {
                    return _damageList[i];
                }
            }

            return 0;
        }

        private void OnWeaponChanged()
        {
            _activeWeapon = _humanoidWeaponController.GetActiveItemData();
            _maxAmmo = _activeWeapon.MaxAmmo;
            _ammoCount = _maxAmmo;
            _damage = _humanoidWeaponController.Damage;
            _range = _activeWeapon.Range;

            if (_activeWeapon.SpreadAngle > 0)
            {
                _isShotgun = true;
                _radiusList = new[]
                {
                    _humanoidWeaponController.GetSpreadAngle() * 0.4f,
                    _humanoidWeaponController.GetSpreadAngle() * 0.6f, _humanoidWeaponController.GetSpreadAngle()
                };
                _damageList = new[] { 1.3f, 1f, _damage * 0.5f };
                _maxRadius = _radiusList[0];
            }
            else
            {
                _isShotgun = false;
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
            if (_isAttacking || _isReloading)
            {
                _playerCharacterAnimController.ReloadWeapon(false);
                _isReloading = false;
                _playerCharacterAnimController.OnShoot(false);
                OnDisable();
            }
        }
    }
}