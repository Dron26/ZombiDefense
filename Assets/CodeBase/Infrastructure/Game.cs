using Infrastructure.States;
using Lean.Localization;
using Service;

namespace Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMashine;
        
        public Game(GameBootstrapper corountineRunner ,LoadingCurtain curtain)
        {
            StateMashine = new GameStateMachine(new SceneLoader(corountineRunner),AllServices.Container ,curtain);
        }
    }
}