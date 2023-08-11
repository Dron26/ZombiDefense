using Data.Settings;
using Data.Settings.Language;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine.States;
using Service.SaveLoad;
using Service.Yandex;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure
{
    public class GameBootstrapper : MonoCache, ICoroutineRunner
    {
        [SerializeField]private YandexLeaderboard _yandexLeaderboard; 
        [SerializeField]private YandexInitializer _yandexInitializer; 
        
        private Game _game;
        private LoadingCurtain _loadingCurtain;
        private SaveLoadService _saveLoadService;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            _saveLoadService = GetComponent<SaveLoadService>();
            _loadingCurtain=GetComponentInChildren<LoadingCurtain>();
        }

        public void Start()
        {
            _yandexInitializer.Completed += Init;
        }

        private  void  Init()
        {
            Language language = GetLanguage();
            _game = new Game(this,_loadingCurtain,language);
            _game.StateMashine.Enter<BootstrapState>();
           
        }
        
        public YandexInitializer GetYandexInitializer() => 
            _yandexInitializer;
        public YandexLeaderboard GetYandexLeaderboard() => 
            _yandexLeaderboard;
        public SaveLoadService GetSAaveLoad() => 
            _saveLoadService;

        public LoadingCurtain GetLoadingCurtain() =>
            _loadingCurtain;


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