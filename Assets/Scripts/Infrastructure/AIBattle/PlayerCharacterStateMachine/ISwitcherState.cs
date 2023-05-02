using Infrastructure.FactoryWarriors.Humanoids;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine
{
    public interface ISwitcherState
    {
        public void EnterBehavior();
        public void ExitBehavior();
        public void Init(PlayerCharactersStateMachine playerCharactersStateMachine,HumanoidFactory humanoidFactory);
    }
}