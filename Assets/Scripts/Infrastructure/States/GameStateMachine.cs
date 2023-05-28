using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Infrastructure.FactoryGame;
using Service;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type,IExitebleState> _states;
        private IExitebleState _activeState;

        public GameStateMachine(SceneLoader sceneLoader , AllServices services)
        {
            List<string> sceneNames = GetSceneNames();

            _states = new Dictionary<Type, IExitebleState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this,sceneLoader, services),
                [typeof(LoadLevelState)] = new LoadLevelState(this,sceneLoader, services.Single<IGameFactory>(),sceneNames),
                [typeof(GameLoopState)] = new GameLoopState(this),
            };

        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState,TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
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
            _states[typeof(TState)]as TState;
        
        private List<string> GetSceneNames()
        {
             // return (from buildSettingsScene in EditorBuildSettings.scenes
             //     where buildSettingsScene.enabled
             //     select buildSettingsScene.path.Substring(buildSettingsScene.path.LastIndexOf(Path.AltDirectorySeparatorChar) + 1)
             //     into name
             //     select name.Substring(0, name.Length - 6)).ToList();
                 Debug.Log("осторожно говнокод");
             List<string> names = new() { "Initial", "GeneralMenu", "SimulationSceneOne" };
             return  names;

        }
    }
}