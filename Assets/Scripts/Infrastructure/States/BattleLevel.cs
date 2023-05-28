using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Constants;
using Infrastructure.FactoryWarriors;
using Infrastructure.Logic;
using UI.HUD.LuckySpin;
using UnityEngine;

namespace Infrastructure.States
{
    public class BattleLevel:MonoCache
    {
        [SerializeField] private CanvasLuckySpin _canvasLuckySpin;
        [SerializeField] private CanvasResultBar _canvasResultBar; 
        private GameStateMachine _stateMachine;
        private LoadingCurtainOld _loadingCurtainOld;
        
        public void Initialize( GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _loadingCurtainOld = GetComponentInChildren<LoadingCurtainOld>();
            _loadingCurtainOld.Hide(false);

            _canvasLuckySpin.Initialize(this);
            _canvasResultBar.Initialize(this);
        }
        
        public void EnterMenuLevel() => _stateMachine.Enter<LoadLevelState,string>(SceneName.Menu);

    }
}