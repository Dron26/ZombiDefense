using System.Collections;
using Animation;
using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.StateMachines.EnemyAI.States
{
    public class EnemyMovementState : EnemyState
    {
        private Character _target;
        private Transform _targetTransform;
        private NavMeshAgent _agent;
        private Animator _animator;
        private EnemyAnimController _enemyAnimController;
        private Enemy _enemy;

        private float _stoppingDistance;
        private float _throwerStoppingDistance;
        private bool _isThrower;
        private bool _isWalking;
        private bool _isTargetSet;

        private const float TrackingProbability = 0.5f;
        private const float CheckDistanceInterval = 0.5f;

        private Coroutine _checkDistanceCoroutine;

        protected override void OnInitialized()
        {
            _animator = StateMachine.Enemy.Animator;
            _enemyAnimController = StateMachine.Enemy.EnemyAnimController;
            _enemy = StateMachine.Enemy;
            _agent = StateMachine.Enemy.NavMeshAgent;

            _agent.speed = _enemy.Data.NavMeshSpeed;
            _isThrower = _enemy.Data.IsThrower;
            _stoppingDistance = _enemy.GetRangeAttack();

            if (_isThrower)
            {
                _throwerStoppingDistance = _enemy.GetThrowerRangeAttack();
            }

            _enemy.OnEntityDeath += OnDeath;
        }

        protected override void OnEnter()
        {
            enabled = true;
            _isWalking = false;
            _isTargetSet = false;

            if (_target != null)
            {
                Move();
            }

            // Запуск корутины проверки расстояния
            _checkDistanceCoroutine = StartCoroutine(CheckDistance());
        }

        protected override void OnExit()
        {
            StopAllCoroutines();
            _agent.isStopped = true;
            _animator.SetBool(_enemyAnimController.Walk, false);
            enabled = false;
            _isWalking= false;
        }

        public void InitCharacter(Character targetCharacter)
        {
            if (_target != targetCharacter)
            {
                _target = targetCharacter;
                _targetTransform= _target.transform;
            }
        }

        private void OnDeath(Entity enemy)
        {
            StateMachine.EnterBehavior<EnemySearchTargetState>();
        }

        private void Move()
        {
            if (_target != null && _target.IsLife())
            {
                if (_agent.isOnNavMesh)
                {
                   // _agent.SetDestination(_character.transform.position);
                    _isTargetSet = true;
                    _agent.isStopped = false;
                    _animator.SetBool(_enemyAnimController.Walk, true);
                }
            }
            else
            {
                StateMachine.EnterBehavior<EnemySearchTargetState>();
            }
        }

        private IEnumerator CheckDistance()
        {
            yield return null;
            
            while (_target.IsLife() && _enemy.IsLife())
            {
                float distance = Vector3.Distance(transform.position, _targetTransform.position);
                _agent.SetDestination(_targetTransform.position);

                if (!_isThrower && distance <= _stoppingDistance)
                {
                    ChangeState<EnemyAttackState>();
                    yield break;
                }

                if (_isThrower)
                {
                    if (distance <= _throwerStoppingDistance && distance > _throwerStoppingDistance / 3)
                    {
                        ChangeState<EnemyAttackState>();
                        yield break;
                    }
                    else if (distance <= _stoppingDistance)
                    {
                        ChangeState<EnemyAttackState>();
                        yield break;
                    }
                }

                if (!_isWalking)
                {
                    _animator.SetBool(_enemyAnimController.Walk, true);
                    _agent.speed = _enemy.Data.NavMeshSpeed;
                    _isWalking = true;
                }

                yield return new WaitForSeconds(CheckDistanceInterval);
            }

            ChangeState<EnemySearchTargetState>();
        }

        private void ChangeState<TState>() where TState : IEnemySwitcherState
        {
            if (isActiveAndEnabled)
            {
                StopAllCoroutines();
                _agent.isStopped = true;
                _animator.SetBool(_enemyAnimController.Walk, false);
                _isTargetSet = false;
                _isWalking = false;
                StateMachine.EnterBehavior<TState>();
            }
        }

        private void OnTargetChangePoint()
        {
            if (gameObject.activeInHierarchy && _agent.isOnNavMesh)
            {
                if (ShouldTrackSoldier())
                {
                    StartCoroutine(CheckSoldierPosition());
                }
                else
                {
                    ChangeState<EnemyAttackState>();
                }
            }
        }

        private IEnumerator CheckSoldierPosition()
        {
            while (_target.IsMove)
            {
                Vector3 soldierPosition = _targetTransform.position;
                if (_agent.isOnNavMesh)
                {
                    _agent.SetDestination(soldierPosition);
                }
                yield return new WaitForSeconds(CheckDistanceInterval);
            }

            _animator.SetBool(_enemyAnimController.Walk, true);
            _isWalking = true;
        }

        private bool ShouldTrackSoldier()
        {
            return Random.value <= TrackingProbability;
        }

        public override void Disable()
        {
            if (_enemy != null)
            {
                _enemy.OnEntityDeath -= OnDeath;
            }
        }
    }
}