using Data.Settings.Language;
using Infrastructure.StateMachine;
using Services;
using Services.PauseService;
using Services.SaveLoad;

namespace Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMashine;
        
        public Game(GameBootstrapper corountineRunner, LoadingCurtain curtain, Language language,
            PauseService pauseService, SaveLoadService saveLoadService)
        {
            StateMashine = new GameStateMachine(new SceneLoader(corountineRunner),AllServices.Container ,curtain,language,pauseService,saveLoadService);
        }
    }
}