using System;
using Characters.Robots;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Logic.Inits;
using Infrastructure.Logic.WaveManagment;
using Infrastructure.StateMachine;
using Interface;
using Services.SaveLoad;
using UI;
using UI.Buttons;
using UI.HUD.StorePanel;
using UI.Report;
using UI.Resurse;
using UI.SettingsPanel;
using UnityEngine;
using UnityEngine.UI;

namespace Services.Ads
{
   [RequireComponent(typeof(RaycastHitChecker))]
    public class HudPanel:MonoCache
    {
        [SerializeField] private Store _store;
        [SerializeField] private MenuPanel _menuPanel;
        [SerializeField] private TimerDisplay _timerDisplay;
        [SerializeField] private ResursesCanvas _resursesCanvas;
        [SerializeField] private ReportPanel _reportPanel;
        
        public Action OnClickExitToMenu;
        private SceneInitializer _sceneInitializer;
        private WaveManager _waveManager;
        public Action OnStartSpawn;
        public Action OnResetLevel;
        private IGameEventBroadcaster _eventBroadcaster;

        public void Init( SceneInitializer sceneInitializer, WaveManager waveManager,
            GlobalTimer globalTimer, GameStateMachine gameStateMachine)
        {
            _waveManager = waveManager;
            _sceneInitializer = sceneInitializer;
            _store.Initialize(_sceneInitializer );
            _menuPanel.Initialize();
            _resursesCanvas.Initialize(_store.GetWallet());
            _reportPanel.Init(_store,gameStateMachine);
            AllServices.Container.Single<IUIHandler>().SetRaycaster(GetButtonPanel().GetComponent<GraphicRaycaster>());
            _eventBroadcaster=AllServices.Container.Single<IGameEventBroadcaster>();
            GetComponent<RaycastHitChecker>().Initialize();
            _timerDisplay.Initialize(_store.GetWallet(),_waveManager);
            AddListener();
        }

        private void ShowSkipButton()
        {
            _eventBroadcaster.OnSetActiveHumanoid -= ShowSkipButton;
            if (_sceneInitializer.IsStartedTutorial) return;
            
            _timerDisplay.ShowSkipButton();
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
        }
        
        public ButtonPanel GetButtonPanel() => _menuPanel.GetButtonPanel();

        public TimerDisplay GetTimerDisplay() => _timerDisplay;

        private void AddListener()
        {
            //_sceneInitializer.OnClickContinue+= StartTimer;
            //_eventBroadcaster.OnLastEnemyRemained += ShowSkipButton;
            //_reportPanel.OnStayInLication += StartContinueSpawn;
            _menuPanel.OnClickExitToMenu+= OnClickExit;
            _reportPanel.OnClickExitToMenu+= OnClickExit;
            _reportPanel.OnResetLevel += ResetLevel;
            _eventBroadcaster.OnSetActiveHumanoid += ShowSkipButton;
        }

        private void RemoveListener()
        {
           // _sceneInitializer.OnClickContinue-= StartTimer;
           // _eventBroadcaster.OnLastEnemyRemained -= ShowSkipButton;
            // _reportPanel.OnStayInLication -= StartContinueSpawn;
            _menuPanel.OnClickExitToMenu-= OnClickExit;
            _reportPanel.OnClickExitToMenu-= OnClickExit;
            _reportPanel.OnResetLevel -= ResetLevel;
            _eventBroadcaster.OnSetActiveHumanoid -= ShowSkipButton;
            
        }

        private void ResetLevel()
        {
            OnResetLevel?.Invoke();
        }
    }
}