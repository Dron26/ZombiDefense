using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors.Enemies;
using Infrastructure.FactoryWarriors.Humanoids;
using Infrastructure.WaveManagment;
using Service.SaveLoadService;

namespace Infrastructure.AIBattle.PlayerCharacterStateMachine.States
{
    public abstract class State : MonoCache, ISwitcherState
    {
        protected PlayerCharactersStateMachine PlayerCharactersStateMachine;
        protected SaveLoadService saveLoadService;

        public void EnterBehavior() =>
            enabled = true;

        public abstract void ExitBehavior();

        public void Init(PlayerCharactersStateMachine playerCharactersStateMachine, SaveLoadService saveLoadService  )
        {
            PlayerCharactersStateMachine = playerCharactersStateMachine;
            this.saveLoadService = saveLoadService;
        }
    }
}