using System;
using System.Collections;
using Data;
using Infrastructure;
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
    public class WindowBase:MonoCache
    {
        [SerializeField] private Store store;
        [SerializeField] private MenuPanel _menuPanel;
        [SerializeField] private TimerDisplay _timerDisplay;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private ResursesCanvas _resursesCanvas;
        [SerializeField] private ReportPanel _reportPanel;
        private SaveLoadService _saveLoadService;
        private SceneInitializer _sceneInitializer;

        public Action OnClickStartSpawn;
        public Action OnClickContinueStartSpawn;
        public void Init(SaveLoadService saveLoadService,SceneInitializer sceneInitializer)
        {
            _saveLoadService = saveLoadService;
            _sceneInitializer = sceneInitializer;
             store.Initialize(_sceneInitializer, _saveLoadService,_timeManager);
            _menuPanel.Initialize(_saveLoadService,_timeManager);
            _menuPanel.OnClickExitToMenu+= SwicthScene;
            _resursesCanvas.Initialize(_saveLoadService);
            _reportPanel.Initialize(_saveLoadService,_timeManager);
            _reportPanel.OnClickExitToMenu+= SwicthScene;
            _reportPanel.OnClickContinue += StartContinueSpawn;
            _saveLoadService.OnCompleteLocation+=_reportPanel.ShowReport;
            _saveLoadService.LastHumanoidDie+=_reportPanel.OnLastHumanoidDie;
            StartTimer();
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
        
        private void SwicthScene()
        {
            _saveLoadService.Save();
            _saveLoadService.GetGameBootstrapper().GetStateMachine().Enter<LoadLevelState,string>(ConstantsData.Menu); 
            Destroy(gameObject);
        }
    }
}