using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.WeaponManagment;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public class AttackState : State
    {
        private readonly WaitForSeconds _waitForSeconds = new(1f);

        private Enemy _enemy = null;

        private float _currentRange;

        private Animator _animator;
        private HashAnimator _hashAnimator;
        private Coroutine _coroutine;
        private FXController _fxController;
        private Humanoid _humanoid;
        private WeaponController _weaponController;
        private LayerMask _enemyLayerMask;
        private bool _isAttacked;

        private bool _isShotgun;
        //private bool _isAttacked;
        private bool _isAttacking;
        private bool _isReloading;
        private bool isShotgun;
        private Weapon _activeWeapon;

        private int _maxAmmo;
        private float _reloadTime;
        private float _fireRate;
        private float _range;
        private int _ammoCount;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _hashAnimator = GetComponent<HashAnimator>();
            _fxController = GetComponent<FXController>();
            _humanoid = GetComponent<Humanoid>();
            _weaponController = GetComponent<WeaponController>();
            _weaponController.ChangeWeapon += OnWeaponChanged;
        }

        protected override void UpdateCustom()
        {
            
            if (!enabled) 
                return;
            else if (_isAttacking == false && isActiveAndEnabled) Attack();
        }

        public void InitEnemy(Enemy targetEnemy) =>
            _enemy = targetEnemy;

        private async Task Attack()
        {
            while (isActiveAndEnabled & _enemy != null & _enemy.IsLife())
            {
                if (_ammoCount <= 0 && _isReloading == false)
                {
                    Reload();
                }

                if (_isAttacking == false & _isReloading == false)
                {
                    _currentRange = Vector3.Distance(transform.position, _enemy.transform.position);
                    float rangeAttack = _weaponController.GetRangeAttack();

                    if (_currentRange <= rangeAttack)
                    {
                        Fire();
                        _isAttacking = true;
                        return;
                        // _fxController.OnAttackFX();
                    }
                }
                break;
            }

            if (_isAttacking != false) return;
        }


        public void Fire()
        {
            print("Fire");
            _animator.SetBool(_hashAnimator.IsShoot,true);
        }

        private async Task Reload()
        {
            _animator.SetBool(_hashAnimator.IsShoot,false);
            _isAttacking = false;
            _isReloading = true;
            _animator.SetTrigger(_hashAnimator.Reload);
            await Task.Delay(TimeSpan.FromSeconds(_reloadTime));
            _ammoCount = _maxAmmo;
            _isReloading = false;
        }


        public void FinishAnimationAttackPlay()
        {
            _ammoCount--;

            if (_isShotgun)
                ApplyDamageToEnemiesInRange();
            else
                _enemy.ApplyDamage(_weaponController.GetDamage());


            if (!_enemy.IsLife())
            {
                _isAttacking = false;
                _animator.SetBool(_hashAnimator.IsShoot,false);
                PlayerCharactersStateMachine.EnterBehavior<SearchTargetState>();
            }

            if (_ammoCount<=0)
            {
                Reload();
            }
        }
        
        private void ApplyDamageToEnemiesInRange()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _weaponController.GetSpread());
           
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out Enemy enemy))
                {
                    if ( enemy.IsLife())
                    {
                        enemy.ApplyDamage(_weaponController.GetDamage());
                    }
                }
            }
        }

        protected override void OnDisable()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _isAttacking = false;
        }

        public void SetAttacked()
        {
            _isAttacked = false;
        }

   

        private void OnWeaponChanged()
        {
            _activeWeapon = _weaponController.GetActiveWeapon();
            _isShotgun=_activeWeapon.IsShotgun;
            _maxAmmo = _activeWeapon.MaxAmmo;
            _ammoCount = _maxAmmo;
            _reloadTime = _weaponController.ReloadTime;
            _fireRate = _activeWeapon.FireRate;
            _range = _activeWeapon.Range;
            isShotgun=_activeWeapon.IsShotgun;
        }
    }
}