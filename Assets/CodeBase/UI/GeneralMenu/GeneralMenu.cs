using System.Collections.Generic;
using Common;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Interface;
using Services;
using Services.Audio;
using Services.SaveLoad;
using UI.Locations;
using UI.SettingsPanel;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;

namespace UI.GeneralMenu
{
      public class GeneralMenu:MonoCache
    {
        [SerializeField] private GameObject _leaderboardPanel;
        [SerializeField] private LocationUIManager _locationUIManager;
        [SerializeField]private  SettingPanel _settingPanel;
        [SerializeField] private GameObject _menuPanel;
        [SerializeField]private AudioManager _audioManager;
        [SerializeField] private LocationManager _locationManager;
        [SerializeField] private Button _play;
        [SerializeField] private Button _upgradeBack;
        [SerializeField] private Button _upgrade;
        [SerializeField] private Button _backUILocotion;
        [SerializeField] private List<UpgradeBranch> _branchContainer;
        [SerializeField] private UpgradeWindow _upgradeWindow;

        private ISaveLoadService _saveLoadService;
        private GameStateMachine _stateMachine;
        private LoadingCurtain _loadingCurtain  ;
        private GameBootstrapper _gameBootstrapper; 
        private IUIHandler _handler;
        
        public  void Initialize( GameStateMachine stateMachine)
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _stateMachine = stateMachine;
            _loadingCurtain =AllServices.Container.Single<IUIHandler>().GetCurtain();
            
            LoadAudioController();
            _settingPanel.Initialize(_audioManager);
            AddListener();

            InitializeLocationSystem();
            // _locationPanel.SetActive(!isActive);
            AllServices.Container.Single<IUpgradeManager>().SetBranch(_branchContainer);
            AllServices.Container.Single<IUpgradeManager>().UpdateBranches();
        }
        
        private void InitializeLocationSystem()
        {
            // Создаем и инициализируем LocationManager
            _locationManager.Initialize(this);

            // Создаем и инициализируем LocationUIManager
            _locationUIManager.Initialize(_saveLoadService,_locationManager);
            _locationUIManager.OnSelectLocation += SwicthScene;
        }
        
        
        private  void  LoadAudioController()
        {
            _audioManager.SetMenuEnabled(true);
            _audioManager.Initialize();
        }
        
        private void OnClikedCurtain()
        {
            bool isActive = AllServices.Container.Single<ILocationHandler>().IsExitFromLocation;

            _menuPanel.SetActive(!isActive);
        }

        private void Start()
        {
            //    _yandexLeaderboard.Initialize(CreateLeaderboard());
            _loadingCurtain.OnLoaded();
        }
        
        private void SwicthScene()
        {
            _saveLoadService.Save();
            _stateMachine.Enter<LoadLevelState, string>(Constants.Location);
            Destroy(gameObject);
        }

        private void AddListener()
        {
            _loadingCurtain.OnClicked += OnClikedCurtain;
            _play.onClick.AddListener(()=>SwitchMenuPanelState(false));
            _backUILocotion.onClick.AddListener(()=>SwitchMenuPanelState(true));
            _upgrade.onClick.AddListener(SwitchPanelsState);
            _upgradeBack.onClick.AddListener(()=>SwitchMenuPanelState(true));
        }

        private void SwitchPanelsState()
        {
            _upgradeWindow.SwitchState();
            SwitchMenuPanelState(false);
        }
        
        private void SwitchMenuPanelState(bool isActive)
        {
            _menuPanel.SetActive(isActive);
            
        }

        private void RemoveListener()
        {
            _loadingCurtain.OnClicked -= OnClikedCurtain;
            _play.onClick.RemoveListener(()=>SwitchMenuPanelState(false));
            _backUILocotion.onClick.RemoveListener(()=>SwitchMenuPanelState(true));
            _upgrade.onClick.AddListener(()=>SwitchMenuPanelState(false));
            _upgradeBack.onClick.AddListener(()=>SwitchMenuPanelState(true));
        }

        private void OnDestroy()
        {
            RemoveListener();
        }
    }
}