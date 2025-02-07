using System;
using System.Collections.Generic;
using Common;
using Data.Settings.Language;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Services.PauseService;

namespace Infrastructure
{
    public class GameStateMachineFactory
    {
        private readonly SceneLoader _sceneLoader;
        private readonly IServiceRegister _serviceRegister;
        private readonly IGameFactory _gameFactory;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly PauseService _pauseService;
        private readonly Language _language;

        public GameStateMachineFactory(SceneLoader sceneLoader, IServiceRegister serviceRegister, 
            IGameFactory gameFactory, LoadingCurtain loadingCurtain , 
            PauseService pauseService, Language language)
        {
            _sceneLoader = sceneLoader;
            _serviceRegister = serviceRegister;
            _gameFactory = gameFactory;
            _loadingCurtain = loadingCurtain;
            _pauseService = pauseService;
            _language = language;
        }

        public GameStateMachine Create()
        {
            var sceneNames = new List<string> { Constants.Initial, Constants.Menu, Constants.Location };

            var gameStateMachine = new GameStateMachine();

            var states = new Dictionary<Type, IExitebleState>
            {
                [typeof(BootstrapState)] = new BootstrapState(gameStateMachine, _sceneLoader, 
                    _serviceRegister, _pauseService, _language, _loadingCurtain),
                [typeof(LoadLevelState)] = new LoadLevelState(gameStateMachine, _sceneLoader, _gameFactory, sceneNames),
                [typeof(GameLoopState)] = new GameLoopState(gameStateMachine, _loadingCurtain)
            };

            gameStateMachine.SetStates(states);
            return gameStateMachine;
        }
    }
}