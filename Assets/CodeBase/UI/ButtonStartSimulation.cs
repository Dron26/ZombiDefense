using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using UnityEngine;

namespace UI
{
    public class ButtonStartSimulation:MonoCache
    {
        private GameStateMachine _stateMachine;
        
        public void ChangeScene()
        {
            _stateMachine = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateMachine>();
            _stateMachine.Enter<LoadLevelState, string>("GameStateMachine");
        }
    }
}