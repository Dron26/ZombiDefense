using Services.SaveLoad;

namespace Infrastructure.AIBattle.StateMachines.Humanoid
{
    public interface ISwitcherState
    {
        public void EnterBehavior();
        public void ExitBehavior();
        public void Init(PlayerCharactersStateMachine playerCharactersStateMachine,SaveLoadService saveLoadService);
    }
}