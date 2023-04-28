using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public abstract class State : MonoCache, ISwitcherState
    {
        protected PlayerCharactersStateMachine PlayerCharactersStateMachine;
        protected Factory Factory;

        public void EnterBehavior() =>
            enabled = true;

        public void ExitBehavior() =>
            enabled = false;

        public void Init(PlayerCharactersStateMachine playerCharactersStateMachine) =>
            PlayerCharactersStateMachine = playerCharactersStateMachine;

        public void InitFactory(Factory factory) => 
            Factory = factory;
    }
}