using Infrastructure.FactoryWarriors.Enemies;
using Infrastructure.WaveManagment;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine
{
    public interface ISwitcherState
    {
        public void EnterBehavior();
        public void ExitBehavior();
        public void Init(PlayerCharactersStateMachine playerCharactersStateMachine,WaveSpawner waveSpawner);
    }
}