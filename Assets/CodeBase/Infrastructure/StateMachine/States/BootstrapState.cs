using Data.Settings.Language;
using Services;
using Services.PauseService;
using Services.SaveLoad;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IServiceRegister _serviceRegister;
        private readonly ISaveLoadService _saveLoadService;
        private readonly PauseService _pauseService;
        private readonly Language _language;
        private readonly LoadingCurtain _loadingCurtain;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, 
            IServiceRegister serviceRegister,
            PauseService pauseService, Language language, LoadingCurtain loadingCurtain)
        {
            _stateMachine = stateMachine;   
            _sceneLoader = sceneLoader;
            _serviceRegister = serviceRegister;
            _pauseService = pauseService;
            _language = language;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter()
        {
            Debug.Log($"Registering LoadingCurtain: {_loadingCurtain != null}");
           // _serviceRegister.RegisterServices(_saveLoadService, _pauseService, _language, _loadingCurtain, AllServices.Container);
            _sceneLoader.Load("Initial", onLoaded: EnterLoadLevel);
        }

        public void Exit() { }

        private void EnterLoadLevel() => _stateMachine.Enter<LoadLevelState, string>("Menu");
    }
}