using Infrastructure.FactoryWarriors.Enemies;
using Infrastructure.Location;

namespace Infrastructure.AIBattle.EnemyAI
{
    public interface IEnemySwitcherState
    {
        public void EnterBehavior();
        public void ExitBehavior();
        public void Init(EnemyStateMachine enemyStateMachine, PlayerCharacterInitializer playerCharacterInitializer);
    }
}