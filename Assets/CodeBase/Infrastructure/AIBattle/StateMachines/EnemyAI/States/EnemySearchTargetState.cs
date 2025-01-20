using System.Collections;
using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using Interface;
using Services;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.StateMachines.EnemyAI.States
{
    public class EnemySearchTargetState : EnemyState
    {
        private EnemyMovementState _movementState;
        private EnemyAttackState _attackState;
        private EnemyThrowState _throwState;
        private Character _targetCharacter;
        private Enemy _enemy;
        private NavMeshAgent _agent;
        private bool _isSearching;
        private Coroutine _searchCoroutine;
        private WaitForSeconds _waitForSeconds;

        public override void Disable()
        { }

        protected override void OnInitialized()
        {
            _movementState = StateMachine.MovementState;
            _attackState = StateMachine.AttackState;
            _throwState= StateMachine.ThrowState;
            _agent = StateMachine.Enemy.NavMeshAgent;
            _enemy = StateMachine.Enemy;
            _waitForSeconds = new WaitForSeconds(0.5f);
        }

        protected override void OnEnter()
        {
            _isSearching = false;
            _agent.isStopped = true;
            _searchCoroutine = StartCoroutine(Search());
        }

        protected override void OnExit()
        {
            _isSearching = false;
            _agent.isStopped = false;
            
            if (_searchCoroutine != null)
            {
                StopCoroutine(_searchCoroutine);
                _searchCoroutine = null;
            }
        }

        private IEnumerator Search()
        {
            yield return null;
            
            while (enabled && _enemy.IsLife())
            {
                if (!_isSearching)
                {
                    _isSearching = true;

                    _targetCharacter = AllServices.Container.Single<ISearchService>()
                        .GetClosestEntity<Characters.Humanoids.AbstractLevel.Humanoid>(transform.position);

                    if (_targetCharacter != null)
                    {
                        _movementState.InitCharacter(_targetCharacter);
                        _attackState.InitCharacter(_targetCharacter);
                        _throwState.InitCharacter(_targetCharacter);

                        StateMachine.EnterBehavior<EnemyMovementState>();
                        yield break; 
                    }

                    _isSearching = false;
                }

                yield return _waitForSeconds;
            }
        }
    }
}