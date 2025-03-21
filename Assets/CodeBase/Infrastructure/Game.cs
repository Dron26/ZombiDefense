using Data.Settings.Language;
using Infrastructure.StateMachine;
using Services.PauseService;

namespace Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;

        public Game(GameBootstrapper coroutineRunner, LoadingCurtain curtain, Language language,
            IServiceRegister serviceRegister, IGameFactory gameFactory)
        {
            var sceneLoader = new SceneLoader(coroutineRunner);
            var gameStateMachineFactory = new GameStateMachineFactory(sceneLoader, serviceRegister,
                gameFactory, curtain, language);

            StateMachine = gameStateMachineFactory.Create();
        }
    }
}