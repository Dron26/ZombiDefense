using System;
using System.Threading.Tasks;
using Audio;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.States;
using Infrastructure.Yandex;
using Service.SaveLoadService;
using Unity.VisualScripting;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoCache, ICoroutineRunner
    {
        [SerializeField]private YandexLeaderboard _yandexLeaderboard; 
        [SerializeField]private YandexAds _yandexAds; 
        [SerializeField]private YandexInitializer _yandexInitializer; 
        
        private Game _game;
        private LoadingCurtain _loadingCurtain;
        private SaveLoadService _saveLoadService;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            Language language = GetLanguage();
            _saveLoadService = GetComponent<SaveLoadService>();
            _loadingCurtain=GetComponentInChildren<LoadingCurtain>();
        }

        public void Start()
        {
            _yandexInitializer.Completed += Init;
        }

        private  void  Init()
        {
            _game = new Game(this,_loadingCurtain);
            _game.StateMashine.Enter<BootstrapState>();
           
        }

        private void Destroy()
        {
            _yandexInitializer.Completed -= Init;
        }
        
        

        public YandexInitializer GetYandexInitializer() => 
            _yandexInitializer;
        public YandexLeaderboard GetYandexLeaderboard() => 
            _yandexLeaderboard;
        public YandexAds GetYandexAds() => 
            _yandexAds;
        public SaveLoadService GetSAaveLoad() => 
            _saveLoadService;

        public LoadingCurtain GetLoadingCurtain() =>
            _loadingCurtain;
    }
}