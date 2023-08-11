using System;
using System.Collections.Generic;
using Data;
using Infrastructure.Factories.FactoryGame;
using Infrastructure.Logic.Inits;
using Service.Ads;
using UI.GeneralMenu;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly List<string> _sceneNames;
        private readonly List<Action> _actions = new();
        private string _nameScene;
        private IAdsService _adsService;
        
        Dictionary<string, Action> _switherGroup = new();

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader ,
            IGameFactory gameFactory,IAdsService adsService, List<string> sceneNames)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _sceneNames = sceneNames;
            _adsService = adsService;
            
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

        private void CreateGeneralMenu()
        {
            
            GameObject generalMenu = _gameFactory.CreateMenu();
            generalMenu.GetComponentInChildren<GeneralMenuManager>().Initialize(_stateMachine);
            _stateMachine.Enter<GameLoopState>();
        }

        private void CreateSceneLevel()
        { 
            
            GameObject level = _gameFactory.CreateLevel();
            level.GetComponentInChildren<SceneInitializer>().Initialize(_stateMachine);
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
            _actions.Add(CreateGeneralMenu);
            _actions.Add(CreateSceneLevel);
        }
        
        private void TryPauseGame()
        {
            
                if (Application.isEditor)
                    return;

                Time.timeScale = ConstantsData.TimeScaleStop;
                SoundInstance.StopRandomMusic(false);
                _adsService.ShowInterstitialAd();
            
        }
    }
}