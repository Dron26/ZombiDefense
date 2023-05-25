using System;
using System.Threading.Tasks;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.WeaponManagment;
using UnityEngine;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public class AttackState : State
    {
        private readonly WaitForSeconds _waitForSeconds = new(1f);

        private Enemy _enemy = null;

        private float _currentRange;

        private PlayerCharacterAnimController _playerCharacterAnimController;
        private Coroutine _coroutine;
        private FXController _fxController;
        private Humanoid _humanoid;
        private WeaponController _weaponController;
        private bool _isAttacked;

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

        private void Awake()
        {
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
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
            while (isActiveAndEnabled && _enemy.IsLife())
            {
                
                if (_ammoCount <= 0 && _isReloading == false)
                {
                    Reload();
                }

                if (_isAttacking == false & _isReloading == false)
                {
                    _currentRange = Vector3.Distance(transform.position, _enemy.transform.position);
                    float rangeAttack = _weaponController.GetRangeAttack();

                    if (_currentRange <= rangeAttack & _ammoCount > 0) 
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
            _playerCharacterAnimController.OnShoot(true);
        }

        private async Task Reload()
        {
            _isReloading = true;
            _isAttacking = false;
            
                _isAttacking = false;
                _playerCharacterAnimController.OnShoot(false);
                _playerCharacterAnimController.OnReload();
        }

        
        private void ReloadEnd()
        {
            
            _ammoCount = _maxAmmo;
            _isReloading = false;   
        }

        public async Task FinishAnimationAttackPlay()
        {
            _ammoCount--;

            if (_isShotgun)
                ApplyDamageToEnemiesInRange();
            else
                _enemy.ApplyDamage(_damage, _weaponController.WeaponName);

            if (!_enemy.IsLife())
            { 
                if (_isShotgun)
                {
                    await Task.Delay(TimeSpan.FromSeconds(_fireRate));
                }

                ChangeState();
            }

            if (_ammoCount<=0&_isReloading==false)
            {
                Reload();
            }
            
            return;
        }

        private void ChangeState()
        {

            _isAttacking = false;
            _playerCharacterAnimController.OnShoot(false);
            PlayerCharactersStateMachine.EnterBehavior<SearchTargetState>();
        }

        private void ApplyDamageToEnemiesInRange()
        {
            float angle = _weaponController.GetSpreadAngle();
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
                    
                            enemy.ApplyDamage(_weaponController.GetDamage() * damagePercent, _weaponController.WeaponName); // применяем урон
                        }
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
          //  _reloadTime = _weaponController.ReloadTime;
            _fireRate = _activeWeapon.FireRate;
            _range = _activeWeapon.Range;
            _damage = _weaponController.GetDamage();

            if (_isShotgun)
            {
                _firstRadius = _weaponController.GetSpread();
                _secondRadius = _weaponController.GetSpread() * 0.6f;
                _thirdRadius = _weaponController.GetSpread() * 0.3f;

                _radiusList = new[] { _firstRadius, _secondRadius, _thirdRadius };
                
                _damageList = new[] { 1.3f ,1f,_damage * 0.026f};
                _maxRadius = _radiusList[0];
            }
        }
    }
}