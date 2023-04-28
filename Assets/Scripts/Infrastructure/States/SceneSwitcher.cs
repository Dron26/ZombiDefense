using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Constants;
using Infrastructure.Logic;
using UnityEngine;

namespace Infrastructure.States
{
    // public class SceneSwitcher:MonoCache
    // {
    //     private GameStateMachine _stateMachine;
    //     [SerializeField] private GameObject _battleLevel;
    //      private LoadingCurtain _loadingCurtain;
    //     
    //     public void Initialize( GameStateMachine playerCharactersStateMachine)
    //     {
    //         GameObject battleLevel = Instantiate(_battleLevel.gameObject);
    //         BattleLevel level = battleLevel.GetComponentInChildren<BattleLevel>();
    //         _stateMachine = playerCharactersStateMachine;
    //         _loadingCurtain = battleLevel.GetComponentInChildren<LoadingCurtain>();
    //         _loadingCurtain.Hide(false);
    //     }
    //     
    //     
    //     public void EnterMenuLevel() => _stateMachine.Enter<LoadLevelState,string>(SceneName.Menu);
    //
    // }
}