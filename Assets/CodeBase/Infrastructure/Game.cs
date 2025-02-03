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
            PauseService pauseService ,
            IServiceRegister serviceRegister, IGameFactory gameFactory)
        {
            var sceneLoader = new SceneLoader(coroutineRunner);
            var gameStateMachineFactory = new GameStateMachineFactory(sceneLoader, serviceRegister,
                gameFactory, curtain, pauseService, language);

            StateMachine = gameStateMachineFactory.Create();
        }
    }
}