
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Infrastructure.AIBattle.StateMachines.Robots.States
{
    public class AttackState:State
    {
        private readonly WaitForSeconds _waitForSeconds = new(0.1f);
        private Enemy _enemy = null;
        private float _currentRange;
        private RobotFXController _fxController;
        private Characters.Robots.Turret _turret;
        private WeaponController _weaponController;
        private bool _isShotgun;
        private bool _isAttacking;
        private bool _isReloading;
        private Weapon _activeWeapon;
        private int _maxAmmo;
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
            _fxController = GetComponent<RobotFXController>();
            _turret = GetComponent<Characters.Robots.Turret>();
            _weaponController = GetComponent<WeaponController>();
            _weaponController.ChangeWeapon += OnWeaponChanged;
        }

        private void Start()
        { 
            PlayerCharactersStateMachine.OnStartMove += StartMove;
        }

        protected override void OnEnabled()
        {
            if (_isAttacking == false & _isReloading == false)
            {
                Attack();
            }
        }

        public void InitEnemy(Enemy targetEnemy)
        {
            _enemy = targetEnemy;
            _isTargetSet = true;

            if (_isAttacking == false & _isReloading == false)
            {
                Attack();
            }
        }

        private void Attack()
        {
            if (_enemy.IsLife())
            {
                if (_ammoCount == 0 && _isReloading == false)
                {
                    Reload();
                }
                if (_isAttacking == false & _isReloading == false)
                {
                    _currentRange = Vector3.Distance(transform.position, _enemy.transform.position);
                    float rangeAttack = _weaponController.GetRangeAttack();

                    if (_currentRange <= rangeAttack & _ammoCount > 0)
                    {
                        _isAttacking = true;
                        _isTargetSet = false;
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
                _enemy.ApplyDamage(_damage, _weaponController.WeaponWeaponType);

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
        }

        public void OnReloadEnd()
        {
            _ammoCount = _maxAmmo;
            _isReloading = false;
            Attack();
        }

        private void ChangeState<TState>() where TState : State
        {
            PlayerCharactersStateMachine.EnterBehavior<TState>();
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

                            enemy.ApplyDamage(_weaponController.GetDamage() * damagePercent,
                                _weaponController.WeaponWeaponType); // применяем урон
                        }
                    }
                }
            }
        }

        private void OnWeaponChanged()
        {
            _activeWeapon = _weaponController.GetActiveWeapon();
            _isShotgun = _activeWeapon.IsShotgun;
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

                _damageList = new[] { 1.3f, 1f, _damage * 0.026f };
                _maxRadius = _radiusList[0];
            }
        }

        public override void ExitBehavior()
        {
            enabled = false;
        }

        protected override void OnDisable()
        {
            _isAttacking = false;
            _isTargetSet = false;
            enabled = false;
        }

        private void StartMove()
        {
            if (_isAttacking == true||_isReloading==true)
            {
                _isReloading = false;
                OnDisable();
            }
        }
    }
}