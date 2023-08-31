using System.Collections.Generic;
using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Service.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Levels
{
    public class LevelMap:MonoCache
    {
        [SerializeField] private List<Level> _levelGroup;
        private List<Level> _tempLevelGroup;

        [SerializeField] private GameObject container;
        [SerializeField] private TMP_Text _cash;
        private int _selectNumber;
        private GameStateMachine _stateMachine;
        private SaveLoadService _saveLoadService;
        
        public void Initialize(GameStateMachine stateMachine,SaveLoadService saveLoadService)
        {
            _saveLoadService=saveLoadService;
            _tempLevelGroup=new List<Level>();
            _stateMachine=stateMachine;
            foreach (var level in container.transform.GetComponentsInChildren<Level>())
            {
                level.GetComponentInChildren<Button>().onClick.AddListener(() =>OnButtonClick(level));
                _tempLevelGroup.Add(level);
            }

            foreach (var levelGroup in _tempLevelGroup)
            {
                for(int i=0;i<_tempLevelGroup.Count;i++)
                {
                    if(levelGroup.Number==i)
                    {
                        _levelGroup.Add(levelGroup);
                    }
                }
            }
            
            
            _cash.text="$"+_saveLoadService.ReadAmountMoney().ToString();
        }

        private void OnButtonClick(Level level)
        {
            _selectNumber=level.Number;
            
            _saveLoadService.SetLevelData(level);
            EnterLevel();
            
        }
        
        public void EnterLevel()
        {
            Debug.Log("EnterLevel()");
            _stateMachine.Enter<LoadLevelState,string>(ConstantsData.Level); 
            Destroy(gameObject);
        } 
    }
}