using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoad;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public abstract class EnemyState : MonoCache, IEnemySwitcherState
    {
        protected EnemyStateMachine StateMachine;
        protected SaveLoadService saveLoadService;

        public void EnterBehavior() =>
            enabled = true;

        public void ExitBehavior() =>
            enabled = false;

        public void Init(EnemyStateMachine enemyStateMachine, SaveLoadService saveLoadService)
        {
            StateMachine = enemyStateMachine;
            this.saveLoadService = saveLoadService;
        }

        public abstract void Disable();
        public abstract void OnTakeGranadeDamage();
    }
}