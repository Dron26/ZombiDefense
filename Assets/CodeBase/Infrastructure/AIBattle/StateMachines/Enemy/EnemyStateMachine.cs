using System;
using System.Collections.Generic;
using Animation;
using Enemies;
using Enemies.AbstractEntity;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.AIBattle.StateMachines.Robots.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Infrastructure.Logic.WeaponManagment;
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
    [RequireComponent(typeof(EnemyStunningState))]
    public class EnemyStateMachine : MonoCache
    {
        private Dictionary<Type, IEnemySwitcherState> _allBehaviors;
        private IEnemySwitcherState _currentBehavior;
        private SceneInitializer _sceneInitializer;
        private SaveLoadService _saveLoadService;
        private Enemy _enemy;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            _enemy.OnTakeGranadeDamage += OnTriggerEnterGranade;
            _enemy.OnEnemyEvent+= OnEnemyEvent;
            _sceneInitializer=FindObjectOfType<SceneInitializer>(); 
            _saveLoadService=_sceneInitializer.GetSaveLoad();
            _allBehaviors = new Dictionary<Type, IEnemySwitcherState>
            {
                [typeof(EnemySearchTargetState)] = GetComponent<EnemySearchTargetState>(),
                [typeof(EnemyMovementState)] = GetComponent<EnemyMovementState>(),
                [typeof(EnemyAttackState)] = GetComponent<EnemyAttackState>(),
                [typeof(EnemyDieState)] = GetComponent<EnemyDieState>(),
                [typeof(EnemyStunningState)] = GetComponent<EnemyStunningState>(),
            };

            foreach (var behavior in _allBehaviors)
            {
                behavior.Value.Init(this, _saveLoadService);
                behavior.Value.ExitBehavior();
            }

        }

        private void OnEnemyEvent(EnemyEventType arg1, ItemType arg2)
        {
            if (arg1==EnemyEventType.Death&&_enemy.Data.Type==EnemyType.Smoker)
            {
                EnterBehavior<EnemyAttackState>();
            }
        }

        private void OnTriggerEnterGranade()
        {
                _currentBehavior.OnTakeGranadeDamage();
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