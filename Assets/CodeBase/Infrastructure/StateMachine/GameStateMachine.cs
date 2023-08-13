using System;
using System.Collections.Generic;
using Data.Settings.Language;
using Infrastructure.Factories.FactoryGame;
using Infrastructure.StateMachine.States;
using Service;
using Service.Ads;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine: IGameStateMachine
    {
        private readonly Dictionary<Type,IExitebleState> _states;
        private IExitebleState _activeState;
        private LoadingCurtain _loadingCurtain;
        
        
        public GameStateMachine(SceneLoader sceneLoader, AllServices services, LoadingCurtain loadingCurtain,
             Language language)
        {
            _loadingCurtain=loadingCurtain;
            List<string> sceneNames = GetSceneNames();

            _states = new Dictionary<Type, IExitebleState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this,sceneLoader, services,language),
                [typeof(LoadLevelState)] = new LoadLevelState(this,sceneLoader, services.Single<IGameFactory>(),sceneNames),
                [typeof(GameLoopState)] = new GameLoopState(this),
            };

        }

        public void Enter<TState>() where TState : class, IState
        {
            _loadingCurtain.StartLoading();
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState,TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            _loadingCurtain.StartLoading();
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
            _states[typeof(TState)]as TState;
        
        private List<string> GetSceneNames()
        {
             // return (from buildSettingsScene in EditorBuildSettings.scenes
             //     where buildSettingsScene.enabled
             //     select buildSettingsScene.path.Substring(buildSettingsScene.path.LastIndexOf(Path.AltDirectorySeparatorChar) + 1)
             //     into name
             //     select name.Substring(0, name.Length - 6)).ToList();
                 Debug.Log("осторожно говнокод");
             List<string> names = new() { "Initial", "Menu", "Level" };
             return  names;

        }
    }
}