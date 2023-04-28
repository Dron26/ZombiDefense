using Infrastructure.AssetManagement;
using Infrastructure.FactoryGame;
using Service;

namespace Infrastructure.States
{
    public class BootstrapState:IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        public BootstrapState(GameStateMachine stateMachine,SceneLoader sceneLoader, AllServices allServices)
        {
            _stateMachine = stateMachine;   
            _sceneLoader = sceneLoader;
            _services = allServices;

            RegisterServices();
        }

        public void Enter() => _sceneLoader.Load(Initial,onLoaded: EnterLoadLevel);

        public void Exit()
        {
        }

        public void EnterLoadLevel() => _stateMachine.Enter<LoadLevelState,string>("Main");

        private void RegisterServices()
        {
            _services.RegisrterSingle<IAssets>(new AssetProvider());
            _services.RegisrterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>()));
        }
    }
}