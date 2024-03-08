using System;
using System.Collections;
using Data;
using Infrastructure;
using Infrastructure.AIBattle.EnemyAI;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Infrastructure.Logic.WaveManagment;
using Infrastructure.StateMachine.States;
using Service.SaveLoad;
using UI;
using UI.HUD.StorePanel;
using UI.Report;
using UI.Resurse;
using UI.SettingsPanel;
using UnityEngine;

namespace Service.Ads
{
    public class HudPanel:MonoCache
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Store store;
        [SerializeField] private MenuPanel _menuPanel;
        [SerializeField] private TimerDisplay _timerDisplay;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private ResursesCanvas _resursesCanvas;
        [SerializeField] private ReportPanel _reportPanel;
        public Action OnClickExitToMenu;
        private SaveLoadService _saveLoadService;
        private SceneInitializer _sceneInitializer;

        public Action OnClickStartSpawn;
        public Action OnClickContinueStartSpawn;
        public void Init(SaveLoadService saveLoadService,SceneInitializer sceneInitializer)
        {
            _saveLoadService = saveLoadService;
            _sceneInitializer = sceneInitializer;
            _sceneInitializer.OnReadySpawning=StartTimer;
             store.Initialize(_sceneInitializer, _saveLoadService,_timeManager);
            _menuPanel.Initialize(_saveLoadService,_timeManager);
            _menuPanel.OnClickExitToMenu+= OnClickExit;
            _resursesCanvas.Initialize(_saveLoadService);
            _reportPanel.Initialize(_saveLoadService,_timeManager);
            _reportPanel.OnClickExitToMenu+= OnClickExit;
            _reportPanel.OnClickContinue += StartContinueSpawn;
            _saveLoadService.OnCompleteLocation+=_reportPanel.ShowReport;
            _saveLoadService.LastHumanoidDie+=_reportPanel.OnLastHumanoidDie;
        }

        private void StartTimer()
        {
            if (_sceneInitializer.IsStartedTutorial) return;
            _timerDisplay.StartTimer(_saveLoadService);
            _timerDisplay.OnClickStartSpawn += StartSpawn;
        }

        protected override void OnDisable()
        {
            _timerDisplay.OnClickStartSpawn-= StartSpawn;
            _saveLoadService.OnCompleteLocation-=_reportPanel.ShowReport;
        }

        private void StartSpawn()
        {
            OnClickStartSpawn.Invoke();
        }
        
        private void StartContinueSpawn()
        {
            OnClickContinueStartSpawn.Invoke();
        }
        
        public Store GetStoreOnPlay() => store;

        private void OnClickExit()
        {
            OnClickExitToMenu?.Invoke();
        }

        public void SwitchPanelState(bool state)
        {
            _panel.SetActive(state);
        }
    }
}