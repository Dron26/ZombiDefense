using System;
using Characters.Robots;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Infrastructure.Logic.WaveManagment;
using Service.SaveLoad;
using UI;
using UI.Buttons;
using UI.HUD.StorePanel;
using UI.Levels;
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
        private WaveManager _waveManager;
        public Action OnStartSpawn;
        public Action OnResetLevel;

        public void Init(SaveLoadService saveLoadService, SceneInitializer sceneInitializer, WaveManager waveManager,
            LocationManager locationManager)
        {
            _saveLoadService = saveLoadService;
            _waveManager = waveManager;
            _sceneInitializer = sceneInitializer;
            _store.Initialize(_sceneInitializer, _saveLoadService,_globalTimer);
            _menuPanel.Initialize(_saveLoadService,_globalTimer);
            _resursesCanvas.Initialize(_store.GetWallet());
            _reportPanel.Init(_saveLoadService,_globalTimer,_store, locationManager);
            _saveLoadService.SetRaycasterPanel(GetButtonPanel().GetComponent<GraphicRaycaster>());
            GetComponent<RaycastHitChecker>().Initialize(_saveLoadService);
            _timerDisplay.Initialize(_saveLoadService,_store.GetWallet(),_waveManager);
            AddListener();
        }

        private void StartTimer()
        {
            _saveLoadService.OnSetActiveHumanoid -= StartTimer;
            if (_sceneInitializer.IsStartedTutorial) return;
            
            _timerDisplay.StartTimer();
        }

        protected override void OnDisable()
        {
            _timerDisplay.Disabled();
            RemoveListener();
        }

        private void StartContinueSpawn()
        {
            OnStartSpawn.Invoke();
        }
        
        public Store GetStoreOnPlay() => _store;

        public Store GetStore()
        {
            return _store;
        }
        
        private void OnClickExit()
        {
            OnClickExitToMenu?.Invoke();
        }

        public void SwitchPanelState(bool state)
        {
            _panel.SetActive(state);
        }
        
        public ButtonPanel GetButtonPanel() => _menuPanel.GetButtonPanel();

        public TimerDisplay GetTimerDisplay() => _timerDisplay;

        private void AddListener()
        {
            _sceneInitializer.OnClickContinue+= StartTimer;
            _saveLoadService.OnSetActiveHumanoid += StartTimer;
            _menuPanel.OnClickExitToMenu+= OnClickExit;
            _reportPanel.OnClickExitToMenu+= OnClickExit;
            _reportPanel.OnStayInLication += StartContinueSpawn;
            _reportPanel.OnResetLevel += ResetLevel;
        }

        private void RemoveListener()
        {
            _sceneInitializer.OnClickContinue-= StartTimer;
            _menuPanel.OnClickExitToMenu-= OnClickExit;
            _reportPanel.OnClickExitToMenu-= OnClickExit;
            _reportPanel.OnStayInLication -= StartContinueSpawn;
            _reportPanel.OnResetLevel -= ResetLevel;
        }

        private void ResetLevel()
        {
            OnResetLevel?.Invoke();
        }
    }
}