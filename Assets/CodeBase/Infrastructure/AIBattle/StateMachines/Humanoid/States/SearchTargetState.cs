using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.StateMachines.Humanoid.States;
using Infrastructure.Logic.WeaponManagment;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public class SearchTargetState : State
    {
        private MovementState _movementState;
        private AttackState _attackState;
        private Enemy _enemy;
        private HumanoidWeaponController _humanoidWeaponController;
        private Transform[] _enemyTransforms;
        private bool _isSearhing;
        private PlayerCharacterAnimController _playerCharacterAnimController;
        private Coroutine _coroutine;

        private Coroutine currentTurnCoroutine;
        
        private bool _isTurning;
        public float maxTurnTime = 1f; // максимальное время поворота
        public float _maxTurnAngle = 180.0f; // максимальный угол, при котором персонаж поворачивается
        private float minTurnTime = 0.5f; // минимальное время поворота
        public float _minTurnAngle = 10f; // минимальный угол, при котором персонаж поворачивается
        private float _turnTime = 0.3f;
        private float minDistanceToEnemy = 2.0f; // минимальное расстояние до врага, при котором персонаж перестает поворачиваться

        private float time = 1f;
        private WaitForSeconds timeout;

        private void Awake()
        {
            timeout = new WaitForSeconds(time);
            _humanoidWeaponController = GetComponent<HumanoidWeaponController>();
            _movementState = GetComponent<MovementState>();
            _attackState = GetComponent<AttackState>();
            _playerCharacterAnimController = GetComponent<PlayerCharacterAnimController>();
        }

        protected override void OnEnabled()
        {
          //  Debug.Log("SearchTargetState-OnEnabled()");
           _coroutine=StartCoroutine(Search());
        }

        private IEnumerator Search()
        {
            _isSearhing = true;
            _playerCharacterAnimController.OnIdle();
            float rangeAttack = _humanoidWeaponController.GetRangeAttack();
            
            while (_isSearhing)
            {
                //  Debug.Log("SearchTargetState-while (_isSearhing)");
                int closestEnemyIndex = GetClosestEnemyIndex();
                
                if (closestEnemyIndex != -1)
                {
                    _enemy = SaveLoadService.GetActiveEnemy()[closestEnemyIndex];

                    float _currentRange = Vector3.Distance(transform.position, _enemy.transform.position);
                    
                    if (_currentRange <= rangeAttack && !_isTurning&&_enemy.IsLife())
                    {
                        LookEnemyPosition(_enemy.transform);
                        _isSearhing = false;
                    }
                }
                
                yield return timeout;
            }
            
        }
        
        private void ChangeState()
        {
            _isTurning = false;
         //   Debug.Log("SearchTargetState-ChangeState)");
            if (_enemy.IsLife())
            {
                _attackState.InitEnemy(_enemy);
                PlayerCharactersStateMachine.EnterBehavior<AttackState>();
            }
        }
        
        private void LookEnemyPosition(Transform enemyTransform)
        {
            _turnTime = 0;
        //    Debug.Log("SearchTargetState-LookEnemyPosition)");
            if (currentTurnCoroutine != null)
            {
                StopCoroutine(currentTurnCoroutine);
            }

            Vector3 direction = enemyTransform.position - transform.position;
            float distance = direction.magnitude;

            if (distance <= minDistanceToEnemy)
            {
                ChangeState();
                return;
            }

            float angle = Vector3.Angle(direction, transform.forward);

            _turnTime = Mathf.Lerp(minTurnTime, maxTurnTime, (angle - _minTurnAngle) / (_maxTurnAngle - _minTurnAngle));
            _turnTime = Mathf.Min(_turnTime, maxTurnTime);
            
            if (Vector3.Dot(direction.normalized, transform.forward) < 0)
            {
                // Враг находится за спиной персонажа
                currentTurnCoroutine = StartCoroutine(TurnTowardsEnemy(enemyTransform, _turnTime, true));
                return;
            }
            
            if (angle < _minTurnAngle)
            {
                ChangeState();
                return;
            }

            currentTurnCoroutine = StartCoroutine(TurnTowardsEnemy(enemyTransform, _turnTime, false));
        }

        private IEnumerator TurnTowardsEnemy(Transform enemyTransform, float turnTime, bool shouldShoot)
        {
            _isTurning = true;
            Quaternion targetRotation = Quaternion.LookRotation(enemyTransform.position - transform.position);
            float t = 0.0f;
            Quaternion startRotation = transform.rotation;

            while (t < turnTime)
            {
                t += Time.deltaTime;
                float normalizedTime = t / turnTime;
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, normalizedTime);
                yield return null;
            }
            _isTurning= false;
            transform.rotation = targetRotation;
         //   Debug.Log("SearchTargetState-ChangeState)");
            
            if (enabled)
            {
                ChangeState();
            }
        }
        
        private int GetClosestEnemyIndex()
        {
            int closestEnemyIndex = -1;
            List<Enemy> enemyGroup = SaveLoadService.GetActiveEnemy();
            if (enemyGroup.Count>0)
            {
                foreach (Enemy enemy in enemyGroup)
                {
                    if (!enemy.IsLife())
                    {
                  //      Debug.Log("dfjnsdkfhskdjhfjskdhfjksdhkfjhsdjk");
                    }
                    
                }
                _enemyTransforms = SaveLoadService.GetActiveEnemy()
                    .Select(enemy => enemy.transform)
                    .ToArray();
            
                NativeArray<EnemyPositionData> enemyPositionDataArray = new NativeArray<EnemyPositionData>(_enemyTransforms.Length, Allocator.TempJob);
            
                for (int i = 0; i < _enemyTransforms.Length; i++)
                {
                    enemyPositionDataArray[i] = new EnemyPositionData
                    {
                        soldierPosition = transform.position,
                        enemyPosition = _enemyTransforms[i].position
                    };
                }

                // Create a job and set the size of the result array.
                var job = new GetClosestEnemyJob
                {
                    enemyPositionDataArray = enemyPositionDataArray
                };
            
                job.SetResultArraySize(_enemyTransforms.Length);

                // Schedule the job and wait for it to complete.
                var jobHandle = job.Schedule(_enemyTransforms.Length, 100);
                jobHandle.Complete();

                // Find the index of the closest enemy from the results of the job.
                
                float closestEnemyDistance = float.MaxValue;
                for (int i = 0; i < _enemyTransforms.Length; i++)
                {
                    float distance = job.resultArray[i].distance;
                    if (distance < closestEnemyDistance)
                    {
                        closestEnemyDistance = distance;
                        closestEnemyIndex = job.resultArray[i].enemyIndex;
                    }
                }

                enemyPositionDataArray.Dispose();
                job.resultArray.Dispose();
            }
            return closestEnemyIndex;
        }
        
        private struct EnemyPositionData
        {
            public Vector3 soldierPosition;
            public Vector3 enemyPosition;
        }
        
        private struct GetClosestEnemyJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<EnemyPositionData> enemyPositionDataArray;
            public NativeArray<EnemyDistanceData> resultArray;

            public struct EnemyDistanceData
            {
                public int enemyIndex;
                public float distance;
            }

            public void Execute(int index)
            {
                EnemyPositionData enemyPositionData = enemyPositionDataArray[index];
                float distance = math.distance(enemyPositionData.soldierPosition, enemyPositionData.enemyPosition);

                resultArray[index] = new EnemyDistanceData
                {
                    enemyIndex = index,
                    distance = distance
                };
            }

            public void SetResultArraySize(int size)
            {
                resultArray = new NativeArray<EnemyDistanceData>(size, Allocator.TempJob);
            }
        }

        public override void ExitBehavior()
        {
           // Debug.Log("SAearh ExitBehavior");
                _isTurning= false;
            enabled = false;
        }
        
        protected override void OnDisable()
        {
          //  Debug.Log("SAearh OnDisable");
            
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _isTurning= false;
            _isSearhing = false;
        }
    }
}