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

        private Entity _enemy;
        private Transform _transformEnemy;
        private float _currentRange;

        private PlayerCharacterAnimController _playerCharacterAnimController;
        private FXController _fxController;
        private Characters.Humanoids.AbstractLevel.Humanoid _humanoid;
        private HumanoidWeaponController _humanoidWeaponController;
        private bool _isSpecialWeapon;
        private SphereCollider _attackTrigger;
        private bool _isAttacking;
        private bool _isReloading;
        private Weapon _activeWeapon;
        private List<Enemy> _enemiesInRange = new();
        private int _maxAmmo;
        private int _ammoCount;
        private float _damage;
        private float _range;
        private float _maxRange;
        private float[] _radiusList;
        private float[] _damageList;
        private float _accumulationDamage;
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
                        
                        _playerCharacterAnimController.OnShoot(true);
                        
                        if (_activeWeapon.ItemType == ItemType.Medium||_activeWeapon.ItemType == ItemType.Flammer)
                        {
                            _fxController.OnAttackFX();
                        }
                    }
                }
            }
            else
            {
                if (_activeWeapon.ItemType == ItemType.Medium||_activeWeapon.ItemType == ItemType.Flammer)
                {
                    _fxController.OnAttackFXStop();
                }

                ChangeState<SearchTargetState>();
            }
            
            transform.DOLookAt(_transformEnemy.position, 0.1f);
        }

        public void FinishAnimationAttackPlay()
        {
            _isAttacking = false;
            _ammoCount--;

            if (_isSpecialWeapon)
            {
                ApplyDamageToEnemiesInRange();
            }
            else
            {
                _enemy.ApplyDamage(_damage, _activeWeapon.ItemType);
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
            _accumulationDamage = 0;
            Debug.Log("AttackChangeState()");
            PlayerCharactersStateMachine.EnterBehavior<TState>();
        }

        private void ApplyDamageToEnemiesInRange()
        {
                float angle = _activeWeapon.SpreadAngle;
                Vector3 attackDirection = _enemy.transform.position - transform.position;
                _enemiesInRange = AllServices.Container.Single<ISearchService>()
                    .GetEntitiesInRange<Enemies.AbstractEntity.Enemy>(transform.position, _maxRange);

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

                            if (_activeWeapon.ItemType != ItemType.Medium)
                            {
                                enemy.ApplyDamage(_damage * damagePercent, _activeWeapon.ItemType);
                            }
                            else
                            {
                                _accumulationDamage += _damage;
                                enemy.ApplyDamage(_accumulationDamage, _activeWeapon.ItemType);
                                Debug.Log(_accumulationDamage);
                            }

                        }
                    }
                }

                Attack();
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
            _damage = _activeWeapon.Damage;
            _range = _activeWeapon.Range;

            if (_activeWeapon.SpreadAngle > 0)
            {
                _isSpecialWeapon = true;
                _radiusList = new[]
                {
                    _activeWeapon.Range * 0.4f,
                    _activeWeapon.Range * 0.6f, _activeWeapon.Range
                };
                _damageList = new[] { 1.3f, 1f,  0.5f };

                if (_activeWeapon.ItemType == ItemType.Medium)
                {
                    _maxRange = _radiusList[2]*2;
                }
                else
                {
                    _maxRange = _radiusList[2];
                }
            }
            else
            {
                _isSpecialWeapon = false;
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