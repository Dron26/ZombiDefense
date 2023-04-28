namespace Infrastructure.AIBattle.EnemyAI
{
    public interface IEnemySwitcherState
    {
        public void EnterBehavior();
        public void ExitBehavior();
        public void Init(EnemyStateMachineWarriors enemyStateMachineWarriors);
    }
}