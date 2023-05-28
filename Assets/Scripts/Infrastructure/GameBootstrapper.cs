using System;
using System.Threading.Tasks;
using Audio;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.States;
using Infrastructure.Yandex;
using Service.SaveLoadService;
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
        private SaveLoad _saveLoad;
        
        private void Awake()
        {
            _saveLoad = GetComponent<SaveLoad>();
            _loadingCurtain=GetComponentInChildren<LoadingCurtain>();
        }

        public void Start()
        {
            _yandexInitializer.Completed += Init;
        }

        private  void  Init()
        {
            _loadingCurtain.StartLoading();
            _game = new Game(this);
            _game.StateMashine.Enter<BootstrapState>();
            DontDestroyOnLoad(this);
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
        public SaveLoad GetSAaveLoad() => 
            _saveLoad;

        public LoadingCurtain GetLoadingCurtain() =>
            _loadingCurtain;
    }
}