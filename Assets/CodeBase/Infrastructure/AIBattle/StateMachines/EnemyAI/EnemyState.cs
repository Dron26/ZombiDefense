using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Services.SaveLoad;

namespace Infrastructure.AIBattle.StateMachines.EnemyAI
{
    public abstract class EnemyState : MonoCache, IEnemySwitcherState
    {
        protected EnemyStateMachine StateMachine;

        public void Init(EnemyStateMachine enemyStateMachine )
        {
            StateMachine = enemyStateMachine ?? throw new ArgumentNullException(nameof(enemyStateMachine));
            OnInitialized();
        }

        public void Enter()
        {
            enabled = true;
            OnEnter();
        }

        public void Exit()
        {
            OnExit(); 
            enabled = false;
        }

        public abstract void Disable();

        protected virtual void OnInitialized() { } 
        protected virtual void OnEnter() { }      
        protected virtual void OnExit() { }        

        protected override void OnDisable()
        {
            Disable();
        }
    }
}