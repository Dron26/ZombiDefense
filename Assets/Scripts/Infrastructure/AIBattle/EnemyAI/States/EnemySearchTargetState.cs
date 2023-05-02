using System.Collections.Generic;
using System.Linq;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using UnityEngine;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public class EnemySearchTargetState : EnemyState
    {
        private EnemyMovementState _movementState;
        private EnemyAttackState _attackState;

        private Humanoid _targetHumanoid;

        private void Start()
        {
            _movementState = GetComponent<EnemyMovementState>();
            _attackState = GetComponent<EnemyAttackState>();
        }

        protected override void UpdateCustom()
        {
            if (isActiveAndEnabled == false)
                return;
            
            Search();
        }

        private void Search()
        {
                _targetHumanoid = GetTargetHumanoid();
                if (_targetHumanoid!=null)
                {
                    _targetHumanoid = FindObjectOfType<Humanoid>();
                    _movementState.InitHumanoid(_targetHumanoid);
                    _attackState.InitHumanoid(_targetHumanoid);
                    StateMachine.EnterBehavior<EnemyMovementState>();
                }
            
        }

        private Humanoid GetTargetHumanoid()
        {
            List<Humanoid> aliveHumanoids = Factory.GetAllHumanoids.Where(humanoid =>
                humanoid.IsLife()).ToList();
        
            if (aliveHumanoids.Count > 0)
                return aliveHumanoids[Random.Range(0, aliveHumanoids.Count)];
        
            return null;
        }
    }
}