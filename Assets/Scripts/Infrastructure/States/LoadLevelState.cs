using System;
using System.Collections.Generic;
using Infrastructure.FactoryGame;
using Infrastructure.Logic;
using UI;
using UnityEngine;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly List<string> _sceneNames;
        private readonly List<Action> _actions = new();
        private string _nameScene;

        Dictionary<string, Action> _switherGroup = new();

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader ,
            IGameFactory gameFactory, List<string> sceneNames)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _sceneNames = sceneNames;

            FillActionGroup();
            FillSwitcherGroup();
        }

        public void Enter(string sceneName)
        {
            _nameScene = sceneName;
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            
        }
            

        private void OnLoaded()
        {
            foreach (var (key, value) in _switherGroup)
                if (key == _nameScene)
                    value();
        }

        private void CreateMainScene()
        {
            
            GameObject mainScene = _gameFactory.CreateMainScene();
            mainScene.GetComponentInChildren<MainScene>().Initialize(_stateMachine);
            _stateMachine.Enter<GameLoopState>();
        }

        private void CreateSceneSwitcher()
        { 
            
            GameObject battleLevel = _gameFactory.CreateSceneBattle();
            battleLevel.GetComponentInChildren<BattleLevel>().Initialize(_stateMachine);
            _stateMachine.Enter<GameLoopState>();
        }

        private void FillSwitcherGroup()
        {
            for (int i = 0; i < _sceneNames.Count; i++)
            {
                _switherGroup.Add(_sceneNames[i], _actions[i]);
            }
        }

        private void FillActionGroup()
        {
            _actions.Add(null);
            _actions.Add(CreateMainScene);
            _actions.Add(CreateSceneSwitcher);
        }
    }
}