using System.Collections.Generic;
using System.Linq;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using UnityEngine;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    [RequireComponent(typeof(MovementState))]
    [RequireComponent(typeof(AttackState))]
    public class EnemySearchTargetState : EnemyState
    {
        private MovementState _movementState;
        private AttackState _attackState;

       // private Enemy _targetEnemy;
        private Humanoid _targetHumanoid;

        private void Start()
        {
            _movementState = GetComponent<MovementState>();
            _attackState = GetComponent<AttackState>();
        }

        protected override void UpdateCustom()
        {
            if (isActiveAndEnabled == false)
                return;
            
            Search();
        }

        private void Search()
        {
                //_targetHumanoid = GetTargetHumanoid();
                _targetHumanoid = FindObjectOfType<Humanoid>();
                    _movementState.InitHumanoid(_targetHumanoid);
                    _attackState.InitHumanoid(_targetHumanoid);
                    StateMachine.EnterBehavior<EnemyMovementState>();
        }

        // private Humanoid GetTargetHumanoid()
        // {
        //     List<Humanoid> aliveHumanoids = Factory.GetAllHumanoids.Where(humanoid =>
        //         humanoid.IsLife()).ToList();
        //
        //     if (aliveHumanoids.Count > 0)
        //         return aliveHumanoids[Random.Range(0, aliveHumanoids.Count)];
        //
        //     return null;
        // }
    }
}