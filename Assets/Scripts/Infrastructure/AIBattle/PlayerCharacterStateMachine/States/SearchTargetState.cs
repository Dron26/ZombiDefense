using System.Collections.Generic;
using System.Linq;
using Enemies.AbstractEntity;
using Humanoids.AbstractLevel;
using UnityEngine;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    [RequireComponent(typeof(MovementState))]
    [RequireComponent(typeof(AttackState))]
    public class SearchTargetState : State
    {
        private MovementState _movementState;
        private AttackState _attackState;

        private Enemy _targetEnemy;
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
            // if (TryGetComponent(out Humanoid _))
            // {
            //     _targetEnemy = GetTargetEnemy();
            //     _movementState.InitEnemy(_targetEnemy);
            //     _attackState.InitEnemy(_targetEnemy);
            //     PlayerCharactersStateMachine.EnterBehavior<MovementState>();
            // }

            if (TryGetComponent(out Enemy _))
            {
                //_targetHumanoid = GetTargetHumanoid();
                _targetHumanoid = FindObjectOfType<Humanoid>();
                    _movementState.InitHumanoid(_targetHumanoid);
                    _attackState.InitHumanoid(_targetHumanoid);
                    PlayerCharactersStateMachine.EnterBehavior<MovementState>();
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

        private Enemy GetTargetEnemy()
        {
            List<Enemy> aliveEnemies = Factory.GetAllEnemies.Where(enemy => 
                enemy.IsLife()).ToList();

            if (aliveEnemies.Count > 0)
                return aliveEnemies[Random.Range(0, aliveEnemies.Count)];

            return null;
        }
    }
}