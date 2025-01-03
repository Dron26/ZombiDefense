using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using Interface;
using Service;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public class EnemySearchTargetState : EnemyState
    {
        private EnemyMovementState _movementState;
        private EnemyAttackState _attackState;
        private Character _targetCharacter;
        private Enemy _enemy;
        private NavMeshAgent agent;
        private Character[] _humanoidTransforms;
        private bool _isSearhing;
        private void Awake()
        {
            _movementState = GetComponent<EnemyMovementState>();
            _attackState = GetComponent<EnemyAttackState>();
            agent = GetComponent<NavMeshAgent>();
            _enemy = GetComponent<Enemy>();
        }

        public override void OnTakeGranadeDamage()
        {
            agent.speed = 0;
            StateMachine.EnterBehavior<EnemyStunningState>();
            _isSearhing = false;
        }

        protected override void UpdateCustom()
        {
            if (_isSearhing == false&&_enemy.IsLife()) Search();
        }

        private void Search()
        {
            agent.speed = 0;
            _isSearhing = true;

            _targetCharacter= AllServices.Container.Single<ISearchService>().GetClosestEntity<Humanoid>(transform.position);

            if (_targetCharacter != null)
            {
                _movementState.InitCharacter(_targetCharacter);
                _attackState.InitCharacter(_targetCharacter);

                EnemyMovementState _enemyMovement = GetComponent<EnemyMovementState>();
                _enemyMovement.SetTarget(false);
                agent.speed = 1;
                StateMachine.EnterBehavior<EnemyMovementState>();
            }

            _isSearhing = false;
        }

        public override void Disable()
        {}
    }
}