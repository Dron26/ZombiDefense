using Services.SaveLoad;

namespace Infrastructure.AIBattle.StateMachines.EnemyAI
{
    public interface IEnemySwitcherState
    {
        public void Enter();
        public void Exit();
        public void Init(EnemyStateMachine enemyStateMachine, SaveLoadService saveLoadService);
        
        public void Disable();
    }
}