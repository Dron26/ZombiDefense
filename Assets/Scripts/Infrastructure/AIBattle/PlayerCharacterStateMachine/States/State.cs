using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors;
using Infrastructure.FactoryWarriors.Humanoids;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public abstract class State : MonoCache, ISwitcherState
    {
        protected PlayerCharactersStateMachine PlayerCharactersStateMachine;
        protected HumanoidFactory HumanoidFactory;

        public void EnterBehavior() =>
            enabled = true;

        public void ExitBehavior() =>
            enabled = false;

        public void Init(PlayerCharactersStateMachine playerCharactersStateMachine, HumanoidFactory humanoidFactory)
        {
            PlayerCharactersStateMachine = playerCharactersStateMachine;
            HumanoidFactory = humanoidFactory;
        }
    }
}