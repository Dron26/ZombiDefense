using System;
using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Humanoids;
using Infrastructure.Location;
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
    
    public class PlayerCharactersStateMachine : MonoCache
    {
        private Dictionary<Type, ISwitcherState> _allBehaviors;
        private ISwitcherState _currentBehavior;
        private SceneInitializer _sceneInitializer;
        private HumanoidFactory _humanoidFactory;

        private void Awake()
        {
            _sceneInitializer=FindObjectOfType<SceneInitializer>();
            _sceneInitializer.SetInfoCompleted += ChangeState;
            _humanoidFactory=_sceneInitializer.GetHumanoidFactory();
            
            _allBehaviors = new Dictionary<Type, ISwitcherState>
            {
                [typeof(SearchTargetState)] = GetComponent<SearchTargetState>(),
                [typeof(MovementState)] = GetComponent<MovementState>(),
                [typeof(AttackState)] = GetComponent<AttackState>(),
            };

            foreach (var behavior in _allBehaviors)
            {
                behavior.Value.Init(this,_humanoidFactory);
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