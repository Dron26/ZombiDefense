using Data.Settings.Language;
using Infrastructure.AssetManagement;
using Interface;
using Services;
using Services.Ads;
using Services.Audio;
using Services.Localization;
using Services.PauseService;
using Services.PlayerAuthorization;
using Services.SaveLoad;
using Upgrades.Base;

namespace Infrastructure.StateMachine.States
{
    public class ServiceRegister : IServiceRegister
    {
        public ServiceRegister(LoadingCurtain loadingCurtain, Language language, AllServices services,
            AudioManager audioManager)
        {
            RegisterServices(loadingCurtain,language, services,audioManager);
        }

        public void RegisterServices(LoadingCurtain loadingCurtain, Language language, AllServices services,
            AudioManager audioManager)
        {
            var saveLoadService = new LoadSaveService();
            services.RegisterSingle<ISaveLoadService>(saveLoadService);
            
            services.RegisterSingle<IGameEventBroadcaster>(new GameEventBroadcaster());
            services.RegisterSingle<IAssets>(new AssetProvider());
            services.RegisterSingle<IGameFactory>(new GameFactory(services.Single<IAssets>()));
            services.RegisterSingle<ILocalizationService>(new LocalizationService(language));
            services.RegisterSingle<IAdsService>(new YandexAdsService());
            services.RegisterSingle<ILeaderboardService>(new YandexLeaderboardService());
            services.RegisterSingle<IAuthorization>(new YandexAuthorization());
            services.RegisterSingle<IResourceLoadService>(new ResourceLoaderService());
            services.RegisterSingle<IPauseService>(new PauseService());
            services.RegisterSingle<ISearchService>(new EntitySearchService());
            services.RegisterSingle<IAudioManager>(audioManager);
            
            var uiHandler = new UIHandler();
            uiHandler.SetCurtain(loadingCurtain);
            services.RegisterSingle<IUIHandler>(uiHandler);
            
            var gameData = services.Single<ISaveLoadService>().Load();
            
            var upgradeHandler=new UpgradeHandler(gameData.GameParameters);
            services.RegisterSingle<IUpgradeHandler>(upgradeHandler);
            
            var сurrencyHandler=new CurrencyHandler(gameData.Money);
            services.RegisterSingle<ICurrencyHandler>(сurrencyHandler);


            
            var upgradeManager = new UpgradeManager( сurrencyHandler);
            services.RegisterSingle<IUpgradeManager>(upgradeManager); 
            //UpgradeManager
            var upgradeTree = new UpgradeTree(saveLoadService, upgradeHandler);
            services.RegisterSingle<IUpgradeTree>(upgradeTree);
            services.Single<IUpgradeManager>().SetTree();
            var achievementsHandler = new AchievementsHandler(gameData.AchievementsData);
            services.RegisterSingle<IAchievementsHandler>(achievementsHandler);
            
            var gameEventBroadcaster = services.Single<IGameEventBroadcaster>();
            services.RegisterSingle<ILocationHandler>(new LocationHandler(gameData));
            services.RegisterSingle<ICharacterHandler>(new CharacterHandler());
            services.RegisterSingle<IAudioSettingsHandler>(new AudioSettingsHandler(gameData.AudioData));
            services.RegisterSingle<IEnemyHandler>(new EnemyHandler( gameEventBroadcaster));
            services.Single<IAudioManager>().Initialize();
            services.Single<ISaveLoadService>().Save();
        }
    }
}
