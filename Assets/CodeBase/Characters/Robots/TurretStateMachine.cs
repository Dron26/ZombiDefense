using System.Collections;
using Data.Upgrades;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.WeaponManagment;
using UnityEngine;

namespace Characters.Robots
{
    
    public class TurretStateMachine:MonoCache
    {
    
        [SerializeField] private int _damage;
        [SerializeField] private float _timeShotsInterval;
        [SerializeField] private GameObject _gun;
        [SerializeField] private GameObject _kk
            ;
        private bool _isTargetSet;
        private bool _isAutoFind;
        private bool _isSelected;
        private bool _isSearch;
        private RobotFXController _fxController;
        private WaitForSeconds _shotsInterval;
        private TurretWeaponController _turretWeaponController;
        private Coroutine currentTurnCoroutine;
        private RaycastHitChecker _raycastHitChecker;
        private TurretGun _turretGun;
        private bool _isTurning;
        public float maxTurnTime = 1f; // максимальное время поворота
        public float _maxTurnAngle = 180.0f; // максимальный угол, при котором персонаж поворачивается
        private float minTurnTime = 0.5f; // минимальное время поворота
        public float _minTurnAngle = 10f; // минимальный угол, при котором персонаж поворачивается
        private float _turnTime = 0.3f;
        private float minDistanceToHit = 2.0f;
        
        public void Initialize(RaycastHitChecker raycastHitChecker, RobotFXController robotFXController,
            TurretWeaponController turretWeaponController)
        {
            _fxController = robotFXController;
            _shotsInterval = new WaitForSeconds(_timeShotsInterval);
            _isAutoFind=false;
            _turretWeaponController = turretWeaponController;
            _turretWeaponController.OnSelected += OnSelectedTurret;
            _raycastHitChecker = raycastHitChecker;
            _turretGun=_turretWeaponController.GetActiveTurretGun(); 
            _turretGun.OnEnter+= OnTriggerEnter;
            _turretGun.OnExit+= OnTriggerExit;
        }

        private void OnSelectedTurret()
        {
            _isSelected = _turretWeaponController.IsSelected;
            
            if (_isSelected&&!_isTargetSet&&!_isSearch)
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
                
            while (!_isTargetSet && _isSelected)
            {
                if ( Input.GetMouseButtonDown(0))
                {
                    if (_raycastHitChecker.CanGetRaycastHit())
                    {
                        LookEnemyPosition(_raycastHitChecker.Point);
                    }
                    else
                    {
                        _isSelected = false;
                    }
                    
                    
                    // 
                    //
                    // if (Physics.Raycast(ray, out RaycastHit hit))
                    // {
                    //
                    //     
                    //
                    //     Debug.Log(gr);
                    //     if (!hit.transform.TryGetComponent(out WorkPoint _))
                    //     {
                    //         LookEnemyPosition(hit.point);
                    //     }
                    // }
                }

                yield return null;
            }
            
            _isSearch = false;
            yield break;

        }
        

        private void LookEnemyPosition(Vector3 hitPosition)
        {
          float   _turnTime = 0;

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
            Quaternion targetRotation = Quaternion.LookRotation( hitPosition - transform.position);
            float t = 0.0f;
            Quaternion startRotation = _gun.transform.rotation;

            while (t < turnTime)
            {
                t += Time.deltaTime;
                float normalizedTime = t / turnTime;
                _gun.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, normalizedTime);
                yield return null;
            }

            _gun.transform.rotation = targetRotation;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Enemy>(out Enemy enemy)&&_isTargetSet==false)
            {
                enemy.OnDeath+= StopAttack;
                StopCoroutine(IdleState());
                StartCoroutine(AttackState(enemy));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            
            if (other.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.OnDeath-= StopAttack;
                StartCoroutine(IdleState());
                StopAttack(enemy);
            }
        }
        private IEnumerator AttackState(Enemy enemy)
        {
            _isTargetSet = true;
            _isSearch = false;
            
            while (enemy.IsLife()&&_isTargetSet)
            {
                if (_isAutoFind)
                {
                    _gun.transform.LookAt(enemy.transform);
                }
                
                _fxController.OnAttackFX();
                enemy.ApplyDamage(_damage, ItemType.Turret);
                _shotsInterval = new WaitForSeconds(_timeShotsInterval);
                yield return _shotsInterval;
            }
        }

        private void StopAttack(Enemy enemy)
        {
            _isTargetSet = false;
            _fxController.OnAttackFXStop();
            StopCoroutine(AttackState(enemy));
        }

        public  void SetUpgrade(UpgradeData upgrade, int level)
        {
            _damage =10;
            _shotsInterval = new WaitForSeconds(0.3f);
            _isAutoFind=false;
        }
    }
}