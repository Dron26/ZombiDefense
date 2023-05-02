using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors;
using Infrastructure.FactoryWarriors.Enemies;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public abstract class EnemyState : MonoCache, IEnemySwitcherState
    {
        protected EnemyStateMachine StateMachine;
        protected EnemyFactory Factory;

        public void EnterBehavior() =>
            enabled = true;

        public void ExitBehavior() =>
            enabled = false;

        public void Init(EnemyStateMachine enemyStateMachine) => 
            StateMachine = enemyStateMachine;

        public void InitFactory(EnemyFactory factory) => 
            Factory = factory;
        
    }
}