using System.Collections.Generic;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Constants;
using Infrastructure.States;
using Service.SaveLoadService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Levels
{
    public class LevelMap:MonoCache
    {
        [SerializeField] private List<LevelPoint> _levelGroup;

        [SerializeField] private GameObject container;
        [SerializeField] private TMP_Text _cash;
        private int _selectNumber;
        private GameStateMachine _stateMachine;
        private SaveLoad _saveLoad;
        
        public void Initialize(GameStateMachine stateMachine,SaveLoad saveLoad)
        {
            _saveLoad=saveLoad;
            
            _stateMachine=stateMachine;
            foreach (var level in container.transform.GetComponentsInChildren<LevelPoint>())
            {
                level.GetComponentInChildren<Button>().onClick.AddListener(() =>OnButtonClick(level));
                _levelGroup.Add(level);
            }
            
            _cash.text="$"+_saveLoad.ReadAmountMoney().ToString();
        }

        private void OnButtonClick(LevelPoint level)
        {
            _selectNumber=level.Number;
            _saveLoad.SetLevel(level.GetWaveDataInfo());
            EnterLevel();
            
        }
        
        public void EnterLevel()
        {
            _stateMachine.Enter<LoadLevelState,string>(SceneName.Level); 
            Destroy(gameObject);
        } 
    }
}