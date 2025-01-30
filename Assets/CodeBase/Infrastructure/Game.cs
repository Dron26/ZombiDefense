using Data.Settings.Language;
using Infrastructure.StateMachine;
using Services.PauseService;
using Services.SaveLoad;

namespace Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(GameBootstrapper coroutineRunner, LoadingCurtain curtain, Language language,
            PauseService pauseService, SaveLoadService saveLoadService,
            IServiceRegister serviceRegister, IGameFactory gameFactory)
        {
            var sceneLoader = new SceneLoader(coroutineRunner);
            var gameStateMachineFactory = new GameStateMachineFactory(sceneLoader, serviceRegister,
                gameFactory, curtain, saveLoadService, pauseService, language);

            StateMachine = gameStateMachineFactory.Create();
        }
    }
}