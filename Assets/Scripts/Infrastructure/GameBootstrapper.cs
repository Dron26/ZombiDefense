using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.States;
using Infrastructure.Yandex;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoCache, ICoroutineRunner
    {
        private Game _game;
        [SerializeField]private YandexInitializer _yandexInitializer;
        public void Start()
        {
            _yandexInitializer.Completed += Init;
        }

        private void Init()
        {
            _game = new Game(this);
            _game.StateMashine.Enter<BootstrapState>();
            DontDestroyOnLoad(this);
        }

        private void Destroy()
        {
            _yandexInitializer.Completed -= Init;
        }
    }
}