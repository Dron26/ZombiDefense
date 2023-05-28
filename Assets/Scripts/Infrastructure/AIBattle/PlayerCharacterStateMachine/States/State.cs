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
        protected SaveLoad SaveLoad;

        public void EnterBehavior() =>
            enabled = true;

        public void ExitBehavior()
        {
            enabled = false;
            
        }

        public void Init(PlayerCharactersStateMachine playerCharactersStateMachine, SaveLoad saveLoad  )
        {
            PlayerCharactersStateMachine = playerCharactersStateMachine;
            SaveLoad = saveLoad;
        }
    }
}