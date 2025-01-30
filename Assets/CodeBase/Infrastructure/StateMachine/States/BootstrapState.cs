using Data;
using Data.Settings.Language;
using Infrastructure.AssetManagement;
using Infrastructure.Factories.FactoryGame;
using Interface;
using Services;
using Services.Ads;
using Services.Localization;
using Services.PauseService;
using Services.PlayerAuthorization;
using Services.SaveLoad;

namespace Infrastructure.StateMachine.States
{
    public class BootstrapState:IState
    {
        private LoadingCurtain _loadingCurtain;
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly PauseService _pauseService;
        private readonly SaveLoadService _saveLoadService;
        private  Language _language;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices allServices,
            Language language, PauseService pauseService, SaveLoadService saveLoadService,
            LoadingCurtain loadingCurtain)
        {
            _loadingCurtain = loadingCurtain;
            _stateMachine = stateMachine;   
            _sceneLoader = sceneLoader;
            _services = allServices;
            _language = language;
            _pauseService=pauseService;
            _saveLoadService = saveLoadService;
            RegisterServices();
        }

        public void Enter() => _sceneLoader.Load(Initial,onLoaded: EnterLoadLevel);

        public void Exit()
        {
        }

        public void EnterLoadLevel() => _stateMachine.Enter<LoadLevelState,string>("Menu");

        private void RegisterServices()
        {
            var UIhandler = new UIHandler();
            UIhandler.SetCurtain(_loadingCurtain);
            
            _services.RegisterSingle<IAssets>(new AssetProvider());
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>()));
            _services.RegisterSingle<ILocalizationService>((new LocalizationService(_language)));
            _services.RegisterSingle<IAdsService>((new YandexAdsService()));
            _services.RegisterSingle<ILeaderboardService>((new YandexLeaderboardService()));
            _services.RegisterSingle<IAuthorization>((new YandexAuthorization()));
            _services.RegisterSingle<SaveLoadService>(_saveLoadService);
            _services.RegisterSingle<IResourceLoadService >(new ResourceLoaderService());
            _services.RegisterSingle<IPauseService>(_pauseService);
            _services.RegisterSingle<ISearchService>((new EntitySearchService()));
            
            _services.RegisterSingle<IDataPersistence>(new DataPersistence());
            _services.RegisterSingle<ICharacterHandler>(new CharacterHandler(_saveLoadService.Characters));
            _services.RegisterSingle<ICurrencyHandler>(new CurrencyHandler(_saveLoadService.Money));
            _services.RegisterSingle<IUIHandler>(new UIHandler());
            _services.RegisterSingle<IAudioSettingsHandler>(new AudioSettingsHandler(_saveLoadService.AudioData));
            _services.RegisterSingle<IAchievementsHandler>(new AchievementsHandler(_saveLoadService.AchievementsData));
            _services.RegisterSingle<IEnemyHandler>(new EnemyHandler(_saveLoadService.Enemies));
            _services.RegisterSingle<ILocationHandler>(new LocationHandler());
            _services.RegisterSingle<IGameEventBroadcaster>(new GameEventBroadcaster());
        }
    }
}