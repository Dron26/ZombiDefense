using Data.Settings.Language;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Interface;
using Services;
using Services.PauseService;
using Services.SaveLoad;
using Services.Yandex;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoCache, ICoroutineRunner
    {
        [SerializeField] private YandexInitializer _yandexInitializer;
        [SerializeField] private LoadingCurtain _loadingCurtain;
        [SerializeField] private PauseService _pauseService;

        private Game _game;
        private IServiceRegister _serviceRegister;
        private IGameFactory _gameFactory;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            // Регистрация сервисов, без необходимости в вызове Initialize
            RegisterServices();

            _gameFactory = new GameFactory(new AssetProvider());

            AllServices.Container.Single<IUIHandler>().SetCurtain(_loadingCurtain);

            Language language = GetLanguage();
            _game = new Game(this, _loadingCurtain, language, _pauseService, _serviceRegister, _gameFactory);
        }

        public void Start()
        {
            Init();
        }

        private void Init()
        {
            _game.StateMachine.Enter<BootstrapState>();
        }

        public YandexInitializer GetYandexInitializer() => _yandexInitializer;
        public LoadingCurtain GetLoadingCurtain() => _loadingCurtain;
        public GameStateMachine GetStateMachine() => _game.StateMachine;

        private Language GetLanguage()
        {
            return Application.systemLanguage switch
            {
                SystemLanguage.Russian => Language.RU,
                SystemLanguage.Turkish => Language.TR,
                _ => Language.EN
            };
        }

        private void RegisterServices()
        {
            _serviceRegister = new ServiceRegister(_pauseService, new Language(), AllServices.Container);
        }
    }
}
