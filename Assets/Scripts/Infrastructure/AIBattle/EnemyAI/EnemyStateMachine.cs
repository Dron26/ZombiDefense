using System;
using System.Collections.Generic;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Infrastructure.Location;
using Observer;
using Service.SaveLoadService;
using UnityEngine;

namespace Infrastructure.AIBattle.EnemyAI{
    [RequireComponent(typeof(FXController))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AnimController))]
    [RequireComponent(typeof(EnemySearchTargetState))]
    [RequireComponent(typeof(EnemyMovementState))]
    [RequireComponent(typeof(EnemyAttackState))]
    [RequireComponent(typeof(EnemyDieState))]
    public class EnemyStateMachine : MonoCache
    {
        private Dictionary<Type, IEnemySwitcherState> _allBehaviors;
        private IEnemySwitcherState _currentBehavior;
        private SceneInitializer _sceneInitializer;
        private SaveLoad _saveLoad;


        private void Awake()
        {
            _sceneInitializer=FindObjectOfType<SceneInitializer>();   
            _saveLoad=_sceneInitializer.GetSaveLoad();
            _allBehaviors = new Dictionary<Type, IEnemySwitcherState>
            {
                [typeof(EnemySearchTargetState)] = GetComponent<EnemySearchTargetState>(),
                [typeof(EnemyMovementState)] = GetComponent<EnemyMovementState>(),
                [typeof(EnemyAttackState)] = GetComponent<EnemyAttackState>(),
                [typeof(EnemyDieState)] = GetComponent<EnemyDieState>(),
            };

            foreach (var behavior in _allBehaviors)
            {
                behavior.Value.Init(this, _saveLoad);
                behavior.Value.ExitBehavior();
            }

        }
        
        private void Start()
        {
            _currentBehavior = _allBehaviors[typeof(EnemySearchTargetState)];
            EnterBehavior<EnemySearchTargetState>();
        }

        public void EnterBehavior<TState>() where TState : IEnemySwitcherState
        {
            var behavior = _allBehaviors[typeof(TState)];
            _currentBehavior.ExitBehavior();
            behavior.EnterBehavior();
            _currentBehavior = behavior;
        }
        
        private void OnDisable()
        {
        }
    }
}