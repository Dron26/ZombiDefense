using System;
using System.Collections.Generic;
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
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class ServiceRegister : IServiceRegister
    {
        
       
        public ServiceRegister( PauseService pauseService, Language language, AllServices services,SaveLoadService saveLoadService)
        {
            RegisterServices(pauseService, language, services,saveLoadService);
        }

        public void RegisterServices(PauseService pauseService, Language language, AllServices services,SaveLoadService saveLoadService)
        {
            services.RegisterSingle<IAssets>(new AssetProvider());
            services.RegisterSingle<IGameFactory>(new GameFactory(services.Single<IAssets>()));
            services.RegisterSingle<ILocalizationService>((new LocalizationService(language)));
            services.RegisterSingle<IAdsService>((new YandexAdsService()));
            services.RegisterSingle<ILeaderboardService>((new YandexLeaderboardService()));
            services.RegisterSingle<IAuthorization>((new YandexAuthorization()));
            services.RegisterSingle<ISaveLoadService>(saveLoadService);
            services.RegisterSingle<IResourceLoadService >(new ResourceLoaderService());
            services.RegisterSingle<IPauseService>(pauseService);
            services.RegisterSingle<IDataPersistence>(new DataPersistence());
            services.RegisterSingle<ICharacterHandler>(new CharacterHandler(saveLoadService.GameData.Characters));
            services.RegisterSingle<ICurrencyHandler>(new CurrencyHandler(saveLoadService.GameData.Money));
            services.RegisterSingle<IUIHandler>( new UIHandler());
            services.RegisterSingle<IAudioSettingsHandler>(new AudioSettingsHandler(saveLoadService.GameData.AudioData));
            services.RegisterSingle<IAchievementsHandler>(new AchievementsHandler(saveLoadService.GameData.AchievementsData));
            services.RegisterSingle<ILocationHandler>(new LocationHandler());
            services.RegisterSingle<IGameEventBroadcaster>(new GameEventBroadcaster());
            services.RegisterSingle<IEnemyHandler>(new EnemyHandler(saveLoadService.GameData.EnemyData, services.Single<IAchievementsHandler>(),services.Single<IGameEventBroadcaster>()));
            
            
        }


       
    }
}