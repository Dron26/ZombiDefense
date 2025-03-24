using System;
using System.Collections;
using Data.Upgrades;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.WeaponManagment;
using Interface;
using Services;
using UnityEngine;

namespace Characters.Robots
{
    public class TurretStateMachine : MonoCache
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _timeShotsInterval;
        [SerializeField] private GameObject _gun;
        [SerializeField] private GameObject _gunBase;
        [SerializeField] private bool _isAutoFind;

        private bool _isCarTurret;
        private bool _isTargetSet;
        private bool _isSelected;
        private bool _isSearch;
        private RobotFXController _fxController;
        private WaitForSeconds _shotsInterval;
        private WaitForSeconds _idleInterval;
        private TurretWeaponController _turretWeaponController;
        private Coroutine currentTurnCoroutine;
        private RaycastHitChecker _raycastHitChecker;
        private TurretGun _turretGun;
        private Enemy _enemy;
        private bool _isTurning;
        public float maxTurnTime = 4f; 
        public float _maxTurnAngle = 180.0f; 
        private float minTurnTime = 1f; 
        public float _minTurnAngle = 10f;
        private float _turnTime = 0.3f;
        private float minDistanceToHit = 2.0f;
        private ISearchService _searchService;
        private IUpgradeTree _upgradeTree;
        private int _rngeAttack = 20;
        private bool _isAttacked;

        public void Initialize(RaycastHitChecker raycastHitChecker, RobotFXController robotFXController,
            TurretWeaponController turretWeaponController, bool isCarTurret)
        {
            _isCarTurret = isCarTurret;
            _fxController = robotFXController;
            _shotsInterval = new WaitForSeconds(_timeShotsInterval);
            _idleInterval = new WaitForSeconds(0.5f);
            _isAutoFind = false;
            _turretWeaponController = turretWeaponController;
            _turretWeaponController.OnSelected += OnSelectedTurret;
            _raycastHitChecker = raycastHitChecker;
            _searchService = AllServices.Container.Single<ISearchService>();
            _upgradeTree=AllServices.Container.Single<IUpgradeTree>();
            SetUpgrades();
        }

        private void OnSelectedTurret()
        {
            _isSelected = _turretWeaponController.IsSelected;

            if (_isSelected && !_isTargetSet && !_isSearch)
            {
                StartCoroutine(IdleState());
            }
            else
            {
                StopCoroutine(IdleState());
            }
        }

        private IEnumerator IdleState()
        {
            _isSearch = true;

            // while (!_isTargetSet && _isSelected)
            // {
            //     if (_isAutoFind)
            //     {
            //         _enemy = _searchService.GetClosestEntity<Enemy>(transform.position);
            //         if (_enemy != null && _enemy.IsLife())
            //         {
            //             float currentRange = Vector3.Distance(transform.position, _enemy.transform.position);
            //
            //             if (currentRange <= _rngeAttack)
            //             {
            //                 LookEnemyPosition(_enemy.transform.position);
            //                 _enemy.OnEnemyEvent += HandleEnemyEvent;
            //                 StopCoroutine(IdleState());
            //             }
            //         }
            //     }
            //     else if (Input.GetMouseButtonDown(0))
            //     {
            //         if (_raycastHitChecker.CanGetRaycastHit())
            //         {
            //             LookEnemyPosition(_raycastHitChecker.Point);
            //         }
            //         else
            //         {
            //             _isSelected = false;
            //         }
            //     }
            //
            //     Debug.Log("_idleInterval");
            //     yield return _idleInterval;
            // }

           
                if (_isAutoFind)
                {
                    while (_isSelected)
                    {
                        if (!_isTargetSet&&!_isTurning)
                        {
                            _enemy = _searchService.GetClosestEntity<Enemy>(transform.position);
                        
                            if (_enemy != null && _enemy.IsLife())
                            {
                                float currentRange = Vector3.Distance(transform.position, _enemy.transform.position);

                                if (currentRange <= _rngeAttack)
                                {
                                    LookEnemyPosition(_enemy.transform.position);
                                }
                            }
                        }
                        
                        
                        Debug.Log("_idleInterval");
                        yield return _idleInterval;
                    }
                }

                while (!_isTargetSet && _isSelected)
                {

                    if (Input.GetMouseButtonDown(0))
                    {

                        if (_raycastHitChecker.CanGetRaycastHit())
                        {
                            LookEnemyPosition(_raycastHitChecker.Point);
                        }
                        else
                        {
                            _isSelected = false;
                        }
                    }

                    Debug.Log("_idleInterval");
                    yield return _idleInterval;

                }

                _isSearch = false;
            Debug.Log("IdleStateFinish");
            yield break;
        }

        private void LookEnemyPosition(Vector3 hitPosition)
        {
            float _turnTime = 1f;

            if (currentTurnCoroutine != null)
            {
                StopCoroutine(currentTurnCoroutine);
            }

            Vector3 direction = hitPosition - _gun.transform.position;
            float distance = direction.magnitude;

            if (distance <= minDistanceToHit)
            {
                return;
            }

            float angle = Vector3.Angle(direction, transform.forward);

            _turnTime = Mathf.Lerp(minTurnTime, maxTurnTime, (angle - _minTurnAngle) / (_maxTurnAngle - _minTurnAngle));
            _turnTime = Mathf.Min(_turnTime, maxTurnTime);

            if (Vector3.Dot(direction.normalized, _gun.transform.forward) < 0)
            {
                currentTurnCoroutine = StartCoroutine(TurnTowardsHit(hitPosition, _turnTime, true));
                return;
            }

            if (angle < _minTurnAngle)
            {
                return;
            }

            currentTurnCoroutine = StartCoroutine(TurnTowardsHit(hitPosition, _turnTime, false));
        }

        private IEnumerator TurnTowardsHit(Vector3 hitPosition, float turnTime, bool shouldShoot)
        {
            _isTurning = true;

            // Направление к цели для gunBase (горизонтальное вращение)
            Vector3 directionBase = hitPosition - transform.position;
            directionBase.y = 0; // Игнорируем вертикальную составляющую
            Quaternion targetRotationBase = Quaternion.LookRotation(directionBase);
     
            float elapsedTime = 0f;
            Quaternion startRotationBase = _gunBase.transform.rotation;
            Quaternion startRotationGun = _gun.transform.rotation;

            while (elapsedTime  < turnTime)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(elapsedTime / turnTime);

                // Плавно поворачиваем gunBase только по оси Y
                _gunBase.transform.rotation = Quaternion.Lerp(startRotationBase, targetRotationBase, normalizedTime);
                _gunBase.transform.rotation = Quaternion.Euler(0, _gunBase.transform.rotation.eulerAngles.y, 0);

              
                yield return null;
            }

            // Устанавливаем финальные повороты
            _gunBase.transform.rotation = targetRotationBase;
            _gun.transform.LookAt(hitPosition);

            _isTurning = false;

            if (_isAutoFind && !_isTargetSet)
            {
                StopCoroutine(currentTurnCoroutine);
                StartCoroutine(AttackState());
            }
        }

        private IEnumerator AttackState()
        {
            _isTargetSet = true;
            _isSearch = false;
            
            while (_enemy!=null&&_enemy.IsLife() && _isTargetSet)
            {
                

                _fxController.OnAttackFX();
                _enemy.ApplyDamage(_damage, ItemType.Turret);
                _shotsInterval = new WaitForSeconds(_timeShotsInterval);
                
                if (_isTurning ==false)
                {
                    LookEnemyPosition(_enemy.transform.position);
                }
                
                yield return _shotsInterval;
            }
            
            StopAttack();
        }

        private void StopAttack()
        {
            Debug.Log("StopAttack()");
            _isTargetSet = false;
            _fxController.OnAttackFXStop();
            StopCoroutine(AttackState());
            //StartCoroutine(IdleState());
        }

        private void SetUpgrades()
        {
            int i=0;
            UpdateUpgradeValue(UpgradeGroupType.Turrets, UpgradeType.AddTurretAutoAim, value => i = value);
            if (i != 0)
            {
                _isAutoFind=(int)Mathf.Round(_upgradeTree.GetUpgradeValue(UpgradeGroupType.Turrets, UpgradeType.AddTurretAutoAim)[0])>0;
            };

        }

        private void UpdateUpgradeValue(UpgradeGroupType groupType, UpgradeType type, Action<int> setValue)
        {
            var upgrades = _upgradeTree.GetUpgradeValue(groupType, type);
            if (upgrades != null && upgrades.Count > 0)
            {
                setValue((int)Mathf.Round(upgrades[0]));
            }
        }
        
        
      
        public void CarTurretActive()
        {
            _isSelected = true;
            _isTargetSet = false;
            _isAutoFind = true;
            StartCoroutine(IdleState());
        }
    }
}