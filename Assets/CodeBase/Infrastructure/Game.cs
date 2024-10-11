using Data.Settings;
using Data.Settings.Language;
using Infrastructure.StateMachine;
using Lean.Localization;
using Service;
using Service.SaveLoad;
using Services.PauseService;

namespace Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMashine;
        
        public Game(GameBootstrapper corountineRunner, LoadingCurtain curtain, Language language,
            PauseService pauseService)
        {
            StateMashine = new GameStateMachine(new SceneLoader(corountineRunner),AllServices.Container ,curtain,language,pauseService);
        }
    }
}