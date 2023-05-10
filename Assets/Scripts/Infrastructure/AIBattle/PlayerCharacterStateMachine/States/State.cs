using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Infrastructure.FactoryWarriors.Humanoids;
using Infrastructure.WaveManagment;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public abstract class State : MonoCache, ISwitcherState
    {
        protected PlayerCharactersStateMachine PlayerCharactersStateMachine;
        protected WaveSpawner WaveSpawner;

        public void EnterBehavior() =>
            enabled = true;

        public void ExitBehavior() =>
            enabled = false;

        public void Init(PlayerCharactersStateMachine playerCharactersStateMachine, WaveSpawner waveSpawner )
        {
            PlayerCharactersStateMachine = playerCharactersStateMachine;
            WaveSpawner = waveSpawner;
        }
    }
}