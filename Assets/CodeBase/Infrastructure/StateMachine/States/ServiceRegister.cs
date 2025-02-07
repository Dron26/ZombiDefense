using Data.Settings.Language;
using Infrastructure.AssetManagement;
using Interface;
using Services;
using Services.Ads;
using Services.Localization;
using Services.PauseService;
using Services.PlayerAuthorization;
using Services.SaveLoad;

namespace Infrastructure.StateMachine.States
{
    public class ServiceRegister : IServiceRegister
    {
        public ServiceRegister(PauseService pauseService, Language language, AllServices services)
        {
            RegisterServices(pauseService, language, services);
        }

        public void RegisterServices(PauseService pauseService, Language language, AllServices services)
        {
            services.RegisterSingle<IAssets>(new AssetProvider());
            services.RegisterSingle<IGameFactory>(new GameFactory(services.Single<IAssets>()));
            services.RegisterSingle<ILocalizationService>(new LocalizationService(language));
            services.RegisterSingle<IAdsService>(new YandexAdsService());
            services.RegisterSingle<ILeaderboardService>(new YandexLeaderboardService());
            services.RegisterSingle<IAuthorization>(new YandexAuthorization());
            services.RegisterSingle<IResourceLoadService>(new ResourceLoaderService());
            services.RegisterSingle<IPauseService>(pauseService);
            services.RegisterSingle<IUIHandler>(new UIHandler());
            services.RegisterSingle<IGameEventBroadcaster>(new GameEventBroadcaster());

            var saveLoadService = new LoadSaveService();
            services.RegisterSingle<ISaveLoadService>(saveLoadService);
            
            var gameData = services.Single<ISaveLoadService>().Load();
            
            var upgradeHandler=new UpgradeHandler(gameData.UpgradeInfo);
            services.RegisterSingle<IUpgradeHandler>(upgradeHandler);
           
            
            var achievementsHandler = new AchievementsHandler(gameData.AchievementsData);
            services.RegisterSingle<IAchievementsHandler>(achievementsHandler);
            
            var gameEventBroadcaster = services.Single<IGameEventBroadcaster>();
            services.RegisterSingle<ILocationHandler>(new LocationHandler(gameData));
            services.RegisterSingle<ICharacterHandler>(new CharacterHandler(gameData.Characters));
            services.RegisterSingle<ICurrencyHandler>(new CurrencyHandler(gameData.Money));
            services.RegisterSingle<IAudioSettingsHandler>(new AudioSettingsHandler(gameData.AudioData));
            
            services.RegisterSingle<IEnemyHandler>(new EnemyHandler(gameData.EnemyData, achievementsHandler, gameEventBroadcaster));
            services.RegisterSingle<IUpgradeTree>(new UpgradeTree(saveLoadService, upgradeHandler));
        }
    }
}
