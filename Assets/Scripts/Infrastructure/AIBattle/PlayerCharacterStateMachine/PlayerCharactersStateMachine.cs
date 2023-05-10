using System;
using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.EnemyAI.States;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Infrastructure.FactoryWarriors.Humanoids;
using Infrastructure.Location;
using Infrastructure.WaveManagment;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine
{
    [RequireComponent(typeof(FXController))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(HashAnimator))]
    [RequireComponent(typeof(SearchTargetState))]
    [RequireComponent(typeof(MovementState))]
    [RequireComponent(typeof(AttackState))]
    [RequireComponent(typeof(DieState))]
    
    public class PlayerCharactersStateMachine : MonoCache
    {
        private Dictionary<Type, ISwitcherState> _allBehaviors;
        private ISwitcherState _currentBehavior;
        private SceneInitializer _sceneInitializer;
        private WaveSpawner _waveSpawner;

        private void Awake()
        {
            _sceneInitializer=FindObjectOfType<SceneInitializer>();
            _sceneInitializer.SetInfoCompleted += ChangeState;
            _waveSpawner=_sceneInitializer.GetWaveSpawner();
            
            _allBehaviors = new Dictionary<Type, ISwitcherState>
            {
                [typeof(SearchTargetState)] = GetComponent<SearchTargetState>(),
                [typeof(MovementState)] = GetComponent<MovementState>(),
                [typeof(AttackState)] = GetComponent<AttackState>(),
                [typeof(DieState)] = GetComponent<DieState>(),
            };

            foreach (var behavior in _allBehaviors)
            {
                behavior.Value.Init(this,_waveSpawner);
                behavior.Value.ExitBehavior();
            }
        }

        private void ChangeState()
        {
            _currentBehavior = _allBehaviors[typeof(SearchTargetState)];
            EnterBehavior<SearchTargetState>();
           
        }

        public void EnterBehavior<TState>() where TState : ISwitcherState
        {
            var behavior = _allBehaviors[typeof(TState)];
            _currentBehavior.ExitBehavior();
            behavior.EnterBehavior();
            _currentBehavior = behavior;
        }
        
        
        private void OnDisable()
        {
            _sceneInitializer.SetInfoCompleted -= ChangeState;
        }
    }
}