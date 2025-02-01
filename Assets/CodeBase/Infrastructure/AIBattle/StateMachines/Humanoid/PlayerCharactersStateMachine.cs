using System;
using System.Collections.Generic;
using Characters.Humanoids.AbstractLevel;
using Infrastructure.AIBattle.StateMachines.Humanoid.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Services.SaveLoad;
using UnityEngine;

namespace Infrastructure.AIBattle.StateMachines.Humanoid
{
    [RequireComponent(typeof(FXController))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerCharacterAnimController))]
    [RequireComponent(typeof(SearchTargetState))]
    [RequireComponent(typeof(MovementState))]
    [RequireComponent(typeof(AttackState))]
    [RequireComponent(typeof(DieState))]
    public class PlayerCharactersStateMachine : MonoCache
    {
        private Dictionary<Type, ISwitcherState> _allBehaviors;
        private ISwitcherState _currentBehavior;
        private SceneInitializer _sceneInitializer;
        private Character _character;
        public Action OnStartMove;

        private void Awake()
        {
            if (TryGetComponent(out Character character))
            {
                _character = character;
                _character.OnInitialize += OnCharacterInitialized;
            }

            _sceneInitializer = FindObjectOfType<SceneInitializer>();

            _allBehaviors = new Dictionary<Type, ISwitcherState>
            {
                [typeof(SearchTargetState)] = GetComponent<SearchTargetState>(),
                [typeof(MovementState)] = GetComponent<MovementState>(),
                [typeof(AttackState)] = GetComponent<AttackState>(),
                [typeof(DieState)] = GetComponent<DieState>(),
            };

            foreach (var behavior in _allBehaviors)
            {
                behavior.Value.Init(this);
                behavior.Value.ExitBehavior();
            }
        }


        public void EnterBehavior<TState>() where TState : ISwitcherState
        {
            var behavior = _allBehaviors[typeof(TState)];
            _currentBehavior.ExitBehavior();
            behavior.EnterBehavior();
            _currentBehavior = behavior;
        }


        public void OnCharacterInitialized(Character character)
        {
            _currentBehavior = _allBehaviors[typeof(SearchTargetState)];
            EnterBehavior<SearchTargetState>();
        }

        public void NotifySelection(bool isSelected)
        {
            throw new NotImplementedException();
        }

        public void MoveTo()
        {
            OnStartMove?.Invoke();
        }

        protected override void OnDisable()
        {
            _character.OnInitialize -= OnCharacterInitialized;
        }
    }
}