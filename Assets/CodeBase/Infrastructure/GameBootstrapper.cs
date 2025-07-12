using System.Collections;
using Data.Settings.Language;
using Infrastructure.AssetManagement;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Lean.Localization;
using Services;
using Services.Ads;
using Services.Audio;
using Services.PauseService;
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
            //AnalyticService.Instance.SendEvent(EventName.startSession);
            Init();
            YG2.GameReadyAPI();
            
        }

        private void Init()
        {
            _audioManager=new AudioManager(_musicChanger,_soundChanger);
            _gameFactory = new GameFactory(new AssetProvider());
            
            RegisterServices(_loadingCurtain);
            
            _game = new Game(this, _loadingCurtain, _serviceRegister, _gameFactory);
            _adService = AllServices.Container.Single<IAdService>();
            
            _game.StateMachine.Enter<BootstrapState>();
            SwitchLanguage();
        }

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
        
        private void RegisterServices(LoadingCurtain loadingCurtain)
        {
            _serviceRegister = new ServiceRegister(loadingCurtain, new Language(), AllServices.Container,_audioManager);
        }
    }
}
