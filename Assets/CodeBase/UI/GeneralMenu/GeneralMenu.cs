using System.Collections.Generic;
using Common;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Integration;
using Interface;
using Services;
using Services.Audio;
using Services.SaveLoad;
using UI.Locations;
using UI.SettingsPanel;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;
using YG;
using Services.PlayerAuthorization;

namespace UI.GeneralMenu
{
      public class GeneralMenu:MonoCache
    {
        [SerializeField] private LeaderboardPanel _leaderboardPanel;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private LocationUIManager _locationUIManager;
        [SerializeField]private  SettingPanel _settingPanel;
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private LocationManager _locationManager;
        [SerializeField] private Button _play;
        [SerializeField] private Button _backUpgradePanel;
        [SerializeField] private Button _upgrade;
        [SerializeField] private Button _backUILocation;
        [SerializeField] private List<UpgradeBranch> _branchContainer;
        [SerializeField] private UpgradePanel _upgradePanel;
        [SerializeField] private AudioSource _soundSource;
        [SerializeField] private Camera _upgradeCamera;
        [SerializeField] private Camera _menuCamera;
        [SerializeField] private AuthorizationPanel _authorizationPanel;
        
        private ISaveLoadService _saveLoadService;
        private GameStateMachine _stateMachine;
        private IUIHandler _handler;
        
        public  void Initialize( GameStateMachine stateMachine)
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _stateMachine = stateMachine;
            _upgradeCamera.gameObject.SetActive(false);
            _settingPanel.Initialize();
            AddListener();

            InitializeLocationSystem();
            AllServices.Container.Single<IUpgradeManager>().SetData(_branchContainer,_upgradePanel);
            AllServices.Container.Single<IUpgradeManager>().UpdateBranches();
            _soundSource = AllServices.Container.Single<IAudioManager>().GetSoundSource();
        }
        
        private void InitializeLocationSystem()
        {
            // Создаем и инициализируем LocationManager
            _locationManager.Initialize();

            // Создаем и инициализируем LocationUIManager
            _locationUIManager.Initialize(_saveLoadService,_locationManager);
            _locationUIManager.OnSelectLocation += SwicthScene;
        }

        private void OnClikedCurtain()
        {
            bool isActive = AllServices.Container.Single<ILocationHandler>().IsExitFromLocation;
            if (isActive)
                _locationManager.SwitchPanelState();
            
            SwitchMenuPanelState(!isActive);

        }

        private void Start()
        {
            //    _yandexLeaderboard.Initialize(CreateLeaderboard());
            AllServices.Container.Single<IUIHandler>().GetCurtain().OnLoaded();
        }
        
        private void SwicthScene()
        {
            _saveLoadService.Save();
            _stateMachine.Enter<LoadLevelState, string>(Constants.Location);
//            Destroy(gameObject);
        }

        private void AddListener()
        {
            AllServices.Container.Single<IUIHandler>().GetCurtain().OnClicked += OnClikedCurtain;
            _play.onClick.AddListener(()=>SwitchMenuPanelState(false));
            _backUILocation.onClick.AddListener(()=>SwitchMenuPanelState(true));
            _backUpgradePanel.onClick.AddListener(()=>SwitchUpgradePanelState(false));
            _upgrade.onClick.AddListener(()=>SwitchUpgradePanelState(true));
           // _leaderboardButton.onClick.AddListener(() => _leaderboardPanel.SwitchState(true));;
        }

        private void SwitchMenuPanelState(bool isActive)
        {
            _menuPanel.SetActive(isActive);
        }

        private void SwitchUpgradePanelState(bool isActive)
        {
            _upgradePanel.SwitchState(isActive);
            _upgradeCamera.gameObject.SetActive(isActive);
            _menuCamera.gameObject.SetActive(!isActive);
            SwitchMenuPanelState(!isActive);
        }
        
        private void SwitchAuthorizationPanelState()
        {
            _authorizationPanel.SwithState(true);
        }
        
        private void RemoveListener()
        {
            AllServices.Container.Single<IUIHandler>().GetCurtain().OnClicked -= OnClikedCurtain;
            _play.onClick.RemoveListener(()=>SwitchMenuPanelState(false));
            _backUILocation.onClick.RemoveListener(()=>SwitchMenuPanelState(true));
            _upgrade.onClick.RemoveListener(()=>SwitchUpgradePanelState(false));
            _backUpgradePanel.onClick.RemoveListener(()=>SwitchUpgradePanelState(false));
           // _leaderboardButton.onClick.RemoveListener(() => _leaderboardPanel.SwitchState(true));;
        }

       

        private void OnDestroy()
        {
            RemoveListener();
        }
    }
}