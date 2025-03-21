using Data.Settings.Language;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Services;
using Services.Audio;
using Services.PauseService;
using Services.Yandex;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoCache, ICoroutineRunner
    {
        [SerializeField] private YandexInitializer _yandexInitializer;
        [SerializeField] private LoadingCurtain _loadingCurtain;
        [SerializeField] private MusicChanger _musicChanger;
        [SerializeField] private SoundChanger _soundChanger;
        
        private Game _game;
        private AudioManager _audioManager;
        private IServiceRegister _serviceRegister;
        private IGameFactory _gameFactory;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            _audioManager=new AudioManager(_musicChanger,_soundChanger);
            _gameFactory = new GameFactory(new AssetProvider());
            
            RegisterServices(_loadingCurtain);


            Language language = GetLanguage();
            _game = new Game(this, _loadingCurtain, language, _serviceRegister, _gameFactory);
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

        private void RegisterServices(LoadingCurtain loadingCurtain)
        {
            _serviceRegister = new ServiceRegister(loadingCurtain, new Language(), AllServices.Container,_audioManager);
        }
    }
}
