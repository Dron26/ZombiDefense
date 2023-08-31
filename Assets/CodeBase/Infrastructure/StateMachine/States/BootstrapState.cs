using Data.Settings.Language;
using Infrastructure.AssetManagement;
using Infrastructure.Factories.FactoryGame;
using Service;
using Service.Ads;
using Service.Localization;
using Service.PlayerAuthorization;
using Service.SaveLoad;

namespace Infrastructure.StateMachine.States
{
    public class BootstrapState:IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private  Language _language;

        public BootstrapState(GameStateMachine stateMachine,SceneLoader sceneLoader, AllServices allServices,Language language)
        {
            _stateMachine = stateMachine;   
            _sceneLoader = sceneLoader;
            _services = allServices;
            _language = language;
            
            RegisterServices();
        }

        public void Enter() => _sceneLoader.Load(Initial,onLoaded: EnterLoadLevel);

        public void Exit()
        {
        }

        public void EnterLoadLevel() => _stateMachine.Enter<LoadLevelState,string>("Menu");

        private void RegisterServices()
        {
            _services.RegisterSingle<IAssets>(new AssetProvider());
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>()));
            _services.RegisterSingle<ILocalizationService>((new LocalizationService(_language)));
            _services.RegisterSingle<IAdsService>((new YandexAdsService()));
            _services.RegisterSingle<ILeaderboardService>((new YandexLeaderboardService()));
            _services.RegisterSingle<IAuthorization>((new YandexAuthorization()));
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService());
        }
    }
}