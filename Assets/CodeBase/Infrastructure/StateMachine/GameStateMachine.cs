using System;
using System.Collections.Generic;
using Common;
using Data.Settings.Language;
using Infrastructure.Factories.FactoryGame;
using Infrastructure.StateMachine.States;
using Services;
using Services.PauseService;
using Services.SaveLoad;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private Dictionary<Type, IExitebleState> _states;
        private IExitebleState _activeState;

        public void SetStates(Dictionary<Type, IExitebleState> states)
        {
            _states = states ?? throw new ArgumentNullException(nameof(states));
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitebleState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitebleState =>
            _states[typeof(TState)] as TState;
    }
}