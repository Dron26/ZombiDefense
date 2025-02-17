using Data.Settings.Language;
using Infrastructure.AssetManagement;
using Interface;
using Services;
using Services.Ads;
using Services.Localization;
using Services.PauseService;
using Services.PlayerAuthorization;
using Services.SaveLoad;
using Upgrades.Base;

namespace Infrastructure.StateMachine.States
{
    public class ServiceRegister : IServiceRegister
    {
        public ServiceRegister(LoadingCurtain loadingCurtain,PauseService pauseService, Language language, AllServices services)
        {
            RegisterServices(loadingCurtain,pauseService,language, services);
        }

        public void RegisterServices(LoadingCurtain loadingCurtain,PauseService pauseService, Language language, AllServices services)
        {
            services.RegisterSingle<IAssets>(new AssetProvider());
            services.RegisterSingle<IGameFactory>(new GameFactory(services.Single<IAssets>()));
            services.RegisterSingle<ILocalizationService>(new LocalizationService(language));
            services.RegisterSingle<IAdsService>(new YandexAdsService());
            services.RegisterSingle<ILeaderboardService>(new YandexLeaderboardService());
            services.RegisterSingle<IAuthorization>(new YandexAuthorization());
            services.RegisterSingle<IResourceLoadService>(new ResourceLoaderService());
            services.RegisterSingle<IPauseService>(pauseService);
            services.RegisterSingle<IGameEventBroadcaster>(new GameEventBroadcaster());
            
            var uiHandler = new UIHandler();
            uiHandler.SetCurtain(loadingCurtain);
            services.RegisterSingle<IUIHandler>(uiHandler);
            
            var saveLoadService = new LoadSaveService();
            services.RegisterSingle<ISaveLoadService>(saveLoadService);
            
            var gameData = services.Single<ISaveLoadService>().Load();
            services.Single<ISaveLoadService>().Save();
            var upgradeHandler=new UpgradeHandler(gameData.GameParameters);
            services.RegisterSingle<IUpgradeHandler>(upgradeHandler);
            
            var сurrencyHandler=new CurrencyHandler(gameData.Money);
            services.RegisterSingle<ICurrencyHandler>(сurrencyHandler);
            
            
            
            services.RegisterSingle<IUpgradeManager>(new UpgradeManager(saveLoadService,upgradeHandler, сurrencyHandler)); //UpgradeManager
            
            var achievementsHandler = new AchievementsHandler(gameData.AchievementsData);
            services.RegisterSingle<IAchievementsHandler>(achievementsHandler);
            
            var gameEventBroadcaster = services.Single<IGameEventBroadcaster>();
            services.RegisterSingle<ILocationHandler>(new LocationHandler(gameData));
            services.RegisterSingle<ICharacterHandler>(new CharacterHandler(gameData.Characters));
            services.RegisterSingle<IAudioSettingsHandler>(new AudioSettingsHandler(gameData.AudioData));
            
            services.RegisterSingle<IEnemyHandler>(new EnemyHandler(gameData.EnemyData, achievementsHandler, gameEventBroadcaster));
        }
    }
}
