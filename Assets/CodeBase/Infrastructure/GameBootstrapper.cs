using Data.Settings.Language;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
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
        private SaveLoadService _saveLoadService;


        private void Awake()
        {
            DontDestroyOnLoad(this);
            _saveLoadService = GetComponent<SaveLoadService>();
            _saveLoadService.SetGameBootstrapper(this);
        }

        public void Start()
        {
            // _yandexInitializer.Completed += Init;
            Init();
        }

        private void Init()
        {
            Language language = GetLanguage();
            _game = new Game(this, _loadingCurtain, language, _pauseService,_saveLoadService);
            _game.StateMashine.Enter<BootstrapState>();
        }

        public YandexInitializer GetYandexInitializer() =>
            _yandexInitializer;

        public SaveLoadService GetSaveLoad() =>
            _saveLoadService;

        public LoadingCurtain GetLoadingCurtain() =>
            _loadingCurtain;

        public GameStateMachine GetStateMachine() =>
            _game.StateMashine;

        private Language GetLanguage()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Russian:
                    return Language.RU;
                case SystemLanguage.Turkish:
                    return Language.TR;
                default:
                    return Language.EN;
            }
        }
    }
}