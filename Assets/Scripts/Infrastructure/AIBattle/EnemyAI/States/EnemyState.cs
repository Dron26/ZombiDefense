using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors;
using Infrastructure.FactoryWarriors.Enemies;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public abstract class EnemyState : MonoCache, IEnemySwitcherState
    {
        protected EnemyStateMachineWarriors StateMachine;
        protected EnemyFactory Factory;

        public void EnterBehavior() =>
            enabled = true;

        public void ExitBehavior() =>
            enabled = false;

        public void Init(EnemyStateMachineWarriors enemyStateMachineWarriors) => 
            StateMachine = enemyStateMachineWarriors;

        public void InitFactory(EnemyFactory factory) => 
            Factory = factory;
    }
}