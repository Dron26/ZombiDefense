using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Services.SaveLoad;

namespace Infrastructure.AIBattle.StateMachines.EnemyAI
{
    public abstract class EnemyState : MonoCache, IEnemySwitcherState
    {
        protected EnemyStateMachine StateMachine;
        protected SaveLoadService SaveLoadService;

        public void Init(EnemyStateMachine enemyStateMachine, SaveLoadService saveLoadService)
        {
            StateMachine = enemyStateMachine ?? throw new ArgumentNullException(nameof(enemyStateMachine));
            SaveLoadService = saveLoadService ?? throw new ArgumentNullException(nameof(saveLoadService));
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