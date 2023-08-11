using Infrastructure.Location;
using Service.SaveLoad;

namespace Infrastructure.AIBattle.EnemyAI
{
    public interface IEnemySwitcherState
    {
        public void EnterBehavior();
        public void ExitBehavior();
        public void Init(EnemyStateMachine enemyStateMachine, SaveLoadService saveLoadService);
    }
}