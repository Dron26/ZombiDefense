using System;
using System.Collections;
using Characters.Robots;
using Data;
using Infrastructure;
using Infrastructure.AIBattle.EnemyAI;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Infrastructure.Logic.WaveManagment;
using Infrastructure.StateMachine.States;
using Service.SaveLoad;
using UI;
using UI.Buttons;
using UI.HUD.StorePanel;
using UI.Report;
using UI.Resurse;
using UI.SettingsPanel;
using UnityEngine;
using UnityEngine.UI;

namespace Service.Ads
{
    [RequireComponent(typeof(RaycastHitChecker))]
    public class HudPanel:MonoCache
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Store _store;
        [SerializeField] private MenuPanel _menuPanel;
        [SerializeField] private TimerDisplay _timerDisplay;
        [SerializeField] private GlobalTimer _globalTimer;
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
             _store.Initialize(_sceneInitializer, _saveLoadService,_globalTimer);
            _menuPanel.Initialize(_saveLoadService,_globalTimer);
            _menuPanel.OnClickExitToMenu+= OnClickExit;
            _resursesCanvas.Initialize(_store.GetWallet());
            _reportPanel.Initialize(_saveLoadService,_globalTimer,_store);
            _reportPanel.OnClickExitToMenu+= OnClickExit;
            _reportPanel.OnClickContinue += StartContinueSpawn;
            _saveLoadService.OnCompleteLocation+=_reportPanel.ShowReport;
            _saveLoadService.LastHumanoidDie+=_reportPanel.OnLastHumanoidDie;
            _saveLoadService.SetRaycasterPanel(GetButtonPanel().GetComponent<GraphicRaycaster>());
            GetComponent<RaycastHitChecker>().Initialize(_saveLoadService);
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
        
        public Store GetStoreOnPlay() => _store;

        private void OnClickExit()
        {
            OnClickExitToMenu?.Invoke();
        }

        public void SwitchPanelState(bool state)
        {
            _panel.SetActive(state);
        }
        
        public ButtonPanel GetButtonPanel() => _menuPanel.GetButtonPanel();
    }
}