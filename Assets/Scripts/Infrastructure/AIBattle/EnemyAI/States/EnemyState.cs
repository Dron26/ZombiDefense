using System.Collections.Generic;
using Humanoids.AbstractLevel;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.FactoryWarriors;
using Infrastructure.FactoryWarriors.Enemies;
using Infrastructure.Location;
using Service.SaveLoadService;

namespace Infrastructure.AIBattle.EnemyAI.States
{
    public abstract class EnemyState : MonoCache, IEnemySwitcherState
    {
        protected EnemyStateMachine StateMachine;
        protected SaveLoad SaveLoad;

        public void EnterBehavior() =>
            enabled = true;

        public void ExitBehavior() =>
            enabled = false;

        public void Init(EnemyStateMachine enemyStateMachine, SaveLoad saveLoad)
        {
            StateMachine = enemyStateMachine;
            SaveLoad = saveLoad;
        }
    }
}