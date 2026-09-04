using System.Collections;
using Data.Settings.Language;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Integration;
using Interface;
using Lean.Localization;
using Services;
using Services.Ads;
using Services.Analytic;
using Services.Audio;
using Services.SaveLoad;
using UnityEngine;
using YG;

namespace Infrastructure
{
    public class GameBootstrapper : MonoCache, ICoroutineRunner
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;
        [SerializeField] private MusicChanger _musicChanger;
        [SerializeField] private SoundChanger _soundChanger;
        [SerializeField] private LeanLocalization _leanLocalization;

        private Game _game;
        private AudioManager _audioManager;
        private IServiceRegister _serviceRegister;
        private IGameFactory _gameFactory;
        private IAdService _adService;
        private IGameTimerTracker _gameTimerTracker;
        private IAnalyticService _analyticService;
        private bool _isInitialized;
        private bool _isSentAdditionalMetrics;
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public IEnumerator Start()
        {
            Time.timeScale = 1f;

            while (!YG2.isSDKEnabled)
            {
                yield return null;
            }
            
            YG2.InterstitialAdvShow();
            Init();
            YG2.GameReadyAPI();
            _gameTimerTracker.StartSession();
            _analyticService.StartGame();
        }

        private void Init()
        {
            _audioManager = gameObject.AddComponent<AudioManager>();
            _audioManager.Initialize(_musicChanger, _soundChanger);
            _gameFactory = new GameFactory(new AssetProvider());
            RegisterServices(_loadingCurtain);
            _game = new Game(this, _loadingCurtain, _serviceRegister, _gameFactory);
            _adService = AllServices.Container.Single<IAdService>();
            _game.StateMachine.Enter<BootstrapState>();
            SwitchLanguage();
            _gameTimerTracker = AllServices.Container.Single<IGameTimerTracker>();
            _analyticService = AllServices.Container.Single<IAnalyticService>();
            _isInitialized = true;
            ApplyRemoteConfig();
        }

         private void ApplyRemoteConfig()
        {
            // Логирование для отладки
            Debug.Log("Применение удалённых флагов...");

            // Отключение рекламы
            if (AllServices.Container.Single<ISaveLoadService>().GetGameData().RemoteConfig.DisableAds)
            {
                Debug.Log("Реклама отключена");
                // Логика отключения рекламы, например:
                // AdManager.Instance.DisableAds();
            }
            else
            {
                Debug.Log("Реклама включена");
            }

            // Разблокировка всех улучшений
            if (AllServices.Container.Single<ISaveLoadService>().GetGameData().RemoteConfig.AllUpgradesUnlocked)
            {
                Debug.Log("Все улучшения разблокированы");
                // Логика разблокировки улучшений, например:
                // UpgradeManager.Instance.UnlockAllUpgrades();
            }

            // Разблокировка всех локаций
            if (AllServices.Container.Single<ISaveLoadService>().GetGameData().RemoteConfig.AllLocationsUnlocked)
            {
                Debug.Log("Все локации разблокированы");
                // Логика разблокировки локаций, например:
                // LocationManager.Instance.UnlockAllLocations();
            }

            // Выдача денег
            if (AllServices.Container.Single<ISaveLoadService>().GetGameData().RemoteConfig.GiveMoney)
            {
                Debug.Log($"Добавлено {AllServices.Container.Single<ISaveLoadService>().GetGameData().RemoteConfig.GetMoneyAmount} денег");
                // Логика добавления денег, например:
                // PlayerEconomy.Instance.AddMoney(_remoteConfig.GiveMoneyAmount);
            }

            // Установка сложности
            Debug.Log($"Уровень сложности: {AllServices.Container.Single<ISaveLoadService>().GetGameData().RemoteConfig.Difficulty}");
            // Логика применения сложности, например:
            // GameManager.Instance.SetDifficulty(_remoteConfig.Difficulty);

            // Ежедневные награды
            var dailyRewards = AllServices.Container.Single<ISaveLoadService>().GetGameData().RemoteConfig.DailyRewardValues;
            Debug.Log($"Ежедневные награды: {string.Join(", ", dailyRewards)}");
            
            // вкл метрик
            if (AllServices.Container.Single<ISaveLoadService>().GetGameData().RemoteConfig.IsSentAdditionalMetrics)
            {
                Debug.Log("доп аналитика вкючена");
                _analyticService.ApplicationQuit();
            }
        }

        // Симуляция флагов в Unity Editor
        #if UNITY_EDITOR
        private void OnValidate()
        {
            // Проверяем, настроены ли флаги в YG2 для симуляции
            if (YG2.flags != null && YG2.flags.Length > 0)
            {
                Debug.Log("Симуляция флагов в Unity Editor активирована");
                foreach (var flag in YG2.flags)
                {
                    Debug.Log($"Флаг: {flag.name} = {flag.value}");
                }
            }
        }
        #endif
        
        public LoadingCurtain GetLoadingCurtain() => _loadingCurtain;
        public GameStateMachine GetStateMachine() => _game.StateMachine;

        public void SwitchLanguage()
        {
            LeanLocalization leanLocalization=LeanLocalization.GetOrCreateInstance();
            
            switch (YG2.lang)
            {
                case "ru":
                    leanLocalization.SetCurrentLanguage("Russian");
                    break;
                case "en":
                    leanLocalization.SetCurrentLanguage("English");
                    break;
                case "tr":
                    leanLocalization.SetCurrentLanguage("Turkey");
                    break;
                default:
                    leanLocalization.SetCurrentLanguage("Russian");
                    break;
            }
        }
        private void OnApplicationQuit()
        {
            if (_isInitialized)
            {
                AllServices.Container.Single<IGameEventBroadcaster>().InvokeOnApplicationQuit();
                _gameTimerTracker.SaveSessionDuration();
                _analyticService.ApplicationQuit();
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (_isInitialized)
            {
                if (pause)
                {
                    _gameTimerTracker.SaveSessionDuration();
                    if (_isSentAdditionalMetrics)
                    {
                        _analyticService.PauseGame();
                    }
                }
                else
                if (_isSentAdditionalMetrics)
                {
                    _analyticService.ResumeGame();
                }
            }
        }
        
        
        private void RegisterServices(LoadingCurtain loadingCurtain)
        {
            _serviceRegister = new ServiceRegister(loadingCurtain, new Language(), AllServices.Container,_audioManager);
        }
    }
}
