using Service.SaveLoad;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine
{
    public interface ISwitcherState
    {
        public void EnterBehavior();
        public void ExitBehavior();
        public void Init(PlayerCharactersStateMachine playerCharactersStateMachine,SaveLoadService saveLoadService);
    }
}