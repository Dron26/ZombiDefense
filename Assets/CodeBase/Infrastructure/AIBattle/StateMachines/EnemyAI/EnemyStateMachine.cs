using System;
using System.Collections.Generic;
using Animation;
using Characters.Humanoids.AbstractLevel;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.StateMachines.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.AIBattle.StateMachines.EnemyAI
{
    [RequireComponent(typeof(Enemy))]
    [RequireComponent(typeof(EnemySearchTargetState))]
    [RequireComponent(typeof(EnemyMovementState))]
    [RequireComponent(typeof(EnemyAttackState))]
    [RequireComponent(typeof(EnemyDieState))]
    [RequireComponent(typeof(EnemyThrowState))]
    public class EnemyStateMachine : MonoCache
    {
        private Dictionary<Type, IEnemySwitcherState> _states;
        private IEnemySwitcherState _currentState;
        private SaveLoadService _saveLoadService;
        private Enemy _enemy;
        private NavMeshAgent _agent;
        private Animator _animator;
        private EnemyAnimController _enemyAnimController;
        
        public Enemy Enemy=>_enemy;
        
        public EnemyMovementState MovementState;
        public EnemyAttackState AttackState;
        public EnemyThrowState ThrowState;
        
        public void Initialize(Enemy enemy)
        {
            _enemy = enemy;
            _saveLoadService = _enemy.SaveLoadService;
            InitializeStates();
        }

        private void InitializeStates()
        {
            MovementState = GetComponent<EnemyMovementState>();
            AttackState = GetComponent<EnemyAttackState>();
            ThrowState=GetComponent<EnemyThrowState>();
                
            _states = new Dictionary<Type, IEnemySwitcherState>
            {
                [typeof(EnemySearchTargetState)] = GetComponent<EnemySearchTargetState>(),
                [typeof(EnemyMovementState)] = MovementState,
                [typeof(EnemyAttackState)] = AttackState,
                [typeof(EnemyDieState)] = GetComponent<EnemyDieState>(),
                [typeof(EnemyThrowState)] = ThrowState,
            };

            foreach (var state in _states.Values)
            {
                state.Init(this, _saveLoadService);
                state.Exit(); // Изначально все состояния выключены
            }
        }
        private void Start()
        {
            EnterBehavior<EnemySearchTargetState>();
        }

        public void EnterBehavior<TState>() where TState : IEnemySwitcherState
        {
            if (_states.TryGetValue(typeof(TState), out var newState))
            {
                _currentState?.Exit(); 
                newState.Enter();      
                _currentState = newState;
            }
            else
            {
                Debug.LogError($"State {typeof(TState)} not found!");
            }
        }

        protected override void OnDisable()
        {
            _currentState?.Exit();
            _enemy.OnInitialized-=  InitializeStates;
        }
    }
}