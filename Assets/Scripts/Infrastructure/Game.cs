using Infrastructure.Logic;
using Infrastructure.States;
using Lean.Localization;
using Service;

namespace Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMashine;
        
        public Game(GameBootstrapper corountineRunner )
        {
            StateMashine = new GameStateMachine(new SceneLoader(corountineRunner),AllServices.Container );
        }
    }
}