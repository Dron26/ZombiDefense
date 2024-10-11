using System;
using System.Collections.Generic;
using Common;
using Data.Settings.Language;
using Infrastructure.Factories.FactoryGame;
using Infrastructure.StateMachine.States;
using Service;
using Service.Ads;
using Service.SaveLoad;
using Services.PauseService;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine: IGameStateMachine
    {
        private readonly Dictionary<Type,IExitebleState> _states;
        private IExitebleState _activeState;
        
        
        public GameStateMachine(SceneLoader sceneLoader, AllServices services, LoadingCurtain loadingCurtain,
            Language language, PauseService pauseService)
        {
            List<string> sceneNames = GetSceneNames();

            _states = new Dictionary<Type, IExitebleState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this,sceneLoader, services,language,pauseService),
                [typeof(LoadLevelState)] = new LoadLevelState(this,sceneLoader, services.Single<IGameFactory>(),sceneNames),
                [typeof(GameLoopState)] = new GameLoopState(this,loadingCurtain),
            };

        }

        public void Enter<TState>() where TState : class, IState
        {
            Debug.Log("Enter<TState>"+typeof(TState));
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState,TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            Debug.Log("Enter<TState,TPayload>"+typeof(TState)+payload);
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitebleState
        {
            Debug.Log("ChangeState"+typeof(TState));
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitebleState => 
            _states[typeof(TState)]as TState;
        
        private List<string> GetSceneNames()
        {
            List<string> names = new() { Constants.Initial, Constants.Menu, Constants.Location };
             return  names;

        }
    }
}