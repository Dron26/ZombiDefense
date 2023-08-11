using Infrastructure.AssetManagement;
using UnityEngine;

namespace Infrastructure.StateMachine.States
{
    public class BattleState : MonoBehaviour
    { 
        private readonly IGameStateMachine _stateMachine;
        private readonly IAssetsProvider _assetsProvider;

    
        public BattleState(IGameStateMachine stateMachine, IAssetsProvider assetsProvider)
        {
            _stateMachine = stateMachine;
            _assetsProvider = assetsProvider;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
            //Debug.Log("At this stage there is nowhere to get out of this state");
        }
    }
}