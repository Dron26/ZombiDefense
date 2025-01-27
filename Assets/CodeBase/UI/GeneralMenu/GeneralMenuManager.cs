using System;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Services;
using Services.Audio;
using Services.SaveLoad;
using UI.Locations;
using UI.SettingsPanel;
using UnityEngine;

namespace UI.GeneralMenu
{
    public class GeneralMenuManager:MonoCache
    {
        [SerializeField] private GameObject _leaderboardPanel;
        
       
        [SerializeField] private LocationMap _locationMap;
        [SerializeField]private  SettingPanel _settingPanel;
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private GameObject _locationPanel;
        [SerializeField]private AudioManager _audioManager;
        
        private SaveLoadService _saveLoadService;
        private GameStateMachine _stateMachine;
        private LoadingCurtain _loadingCurtain  ;
        private GameBootstrapper _gameBootstrapper; 
        
        private LocationManager _locationManager;
        private LocationUIManager _locationUIManager;
        
        public  void Initialize( GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _gameBootstrapper=FindObjectOfType<GameBootstrapper>();
            _loadingCurtain = _gameBootstrapper.GetLoadingCurtain();
            
            _saveLoadService = AllServices.Container.Single<SaveLoadService>();
            
            LoadAudioController();
            _settingPanel.Initialize(_audioManager,_saveLoadService);
            
            _saveLoadService.SetFirstStart();

            AddListener();

            InitializeLocationSystem();
           // _locationPanel.SetActive(!isActive);
        }
        
        private void InitializeLocationSystem()
        {
            // Создаем и инициализируем LocationManager
            _locationManager = new LocationManager();
            _locationManager.Initialize(_stateMachine);

            // Создаем и инициализируем LocationUIManager
            _locationUIManager = new LocationUIManager();
            _locationUIManager.Initialize(_locationManager, HandleLocationSelected);
            
            _locationMap.Initialize( _saveLoadService,_locationManager);
        }
        
        
        private  void  LoadAudioController()
        {
            _audioManager.SetMenuEnabled(true);
            _audioManager.Initialize(_saveLoadService);
        }
        
        private void OnClikedCurtain()
        {
            bool isActive = _saveLoadService.IsExitFromLocation;

            _menuPanel.SetActive(!isActive);
        }

        private void Start()
        {
        //    _yandexLeaderboard.Initialize(CreateLeaderboard());
            
                _loadingCurtain.OnLoaded();
        }

        private void AddListener()
        {
            _loadingCurtain.OnClicked += OnClikedCurtain;
            _locationMap.OnSelectLocation += _locationManager.SetSelectedLocationId;
        }

        private void RemoveListener()
        {
            _loadingCurtain.OnClicked -= OnClikedCurtain;
            _locationMap.OnSelectLocation -= _locationManager.SetSelectedLocationId;
        }

        private void OnDestroy()
        {
            RemoveListener();
        }
    }
}