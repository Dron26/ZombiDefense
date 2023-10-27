using System;
using System.Collections.Generic;
using Animation;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Location;
using Infrastructure.Logic.Inits;
using Service.SaveLoad;
using UnityEngine;

namespace Infrastructure.AIBattle.EnemyAI{
    [RequireComponent(typeof(EnemyFXController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(EnemyAnimController))]
    [RequireComponent(typeof(EnemySearchTargetState))]
    [RequireComponent(typeof(EnemyMovementState))]
    [RequireComponent(typeof(EnemyAttackState))]
    [RequireComponent(typeof(EnemyDieState))]
    public class EnemyStateMachine : MonoCache
    {
        private Dictionary<Type, IEnemySwitcherState> _allBehaviors;
        private IEnemySwitcherState _currentBehavior;
        private SceneInitializer _sceneInitializer;
        private SaveLoadService _saveLoadService;


        private void Awake()
        {
            _sceneInitializer=FindObjectOfType<SceneInitializer>();   
            _saveLoadService=_sceneInitializer.GetSaveLoad();
            _allBehaviors = new Dictionary<Type, IEnemySwitcherState>
            {
                [typeof(EnemySearchTargetState)] = GetComponent<EnemySearchTargetState>(),
                [typeof(EnemyMovementState)] = GetComponent<EnemyMovementState>(),
                [typeof(EnemyAttackState)] = GetComponent<EnemyAttackState>(),
                [typeof(EnemyDieState)] = GetComponent<EnemyDieState>(),
            };

            foreach (var behavior in _allBehaviors)
            {
                behavior.Value.Init(this, _saveLoadService);
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

        protected  override void OnDisable()
        {
            foreach (var behavior in _allBehaviors)
            {
                behavior.Value.Disable();
            }
        }
    }
}