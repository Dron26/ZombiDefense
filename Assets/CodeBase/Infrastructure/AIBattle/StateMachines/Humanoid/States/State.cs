using Infrastructure.BaseMonoCache.Code.MonoCache;
using Services.SaveLoad;

namespace Infrastructure.AIBattle.StateMachines.Humanoid.States
{
    public abstract class State : MonoCache, ISwitcherState
    {
        protected PlayerCharactersStateMachine PlayerCharactersStateMachine;
        protected SaveLoadService SaveLoadService;

        public void EnterBehavior() =>
            enabled = true;

        public abstract void ExitBehavior();

        public void Init(PlayerCharactersStateMachine playerCharactersStateMachine, SaveLoadService saveLoadService  )
        {
            PlayerCharactersStateMachine = playerCharactersStateMachine;
            this.SaveLoadService = saveLoadService;
        }
    }
}