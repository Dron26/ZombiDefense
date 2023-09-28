using System;
using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.AIBattle.PlayerCharacterStateMachine.States;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Service.SaveLoad;
using UnityEngine;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine
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
        private SaveLoadService _saveLoadService;
        private Humanoid _humanoid;
        public Action OnStartMove;

        private void Awake()
        {
            if (TryGetComponent(out Humanoid humanoid))
            {
                _humanoid = humanoid;
                _humanoid.OnInitialize += OnHumanoidInitialized;
            }

            _sceneInitializer = FindObjectOfType<SceneInitializer>();
            _saveLoadService = _sceneInitializer.GetSaveLoad();

            _allBehaviors = new Dictionary<Type, ISwitcherState>
            {
                [typeof(SearchTargetState)] = GetComponent<SearchTargetState>(),
                [typeof(MovementState)] = GetComponent<MovementState>(),
                [typeof(AttackState)] = GetComponent<AttackState>(),
                [typeof(DieState)] = GetComponent<DieState>(),
            };

            foreach (var behavior in _allBehaviors)
            {
                behavior.Value.Init(this, _saveLoadService);
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


        public void OnHumanoidInitialized(Humanoid humanoid)
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
            _humanoid.OnInitialize -= OnHumanoidInitialized;
        }
    }
}