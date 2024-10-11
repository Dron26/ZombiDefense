using System;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Service;
using Service.Audio;
using Service.SaveLoad;
using UI.Levels;
using UI.SettingsPanel;
using UnityEngine;

namespace UI.GeneralMenu
{
    public class GeneralMenuManager:MonoCache
    {
        [SerializeField] private GameObject _leaderboardPanel;
        
        private SaveLoadService _saveLoadService;
        private GameStateMachine _stateMachine;
        private LoadingCurtain _loadingCurtain  ;
        private GameBootstrapper _gameBootstrapper; 
        [SerializeField] private LocationMap _locationMap;
        [SerializeField]private  SettingPanel _settingPanel;
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private GameObject _locationPanel;
        [SerializeField]private AudioManager _audioManager;
        private LocationManager _locationManager;
        
        public  void Initialize( GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _gameBootstrapper=FindObjectOfType<GameBootstrapper>();
            _saveLoadService = _gameBootstrapper.GetSaveLoad();
            _locationManager=_gameBootstrapper.GetLocationManager();
            _loadingCurtain=_gameBootstrapper.GetLoadingCurtain();
            
            
            
            LoadAudioController();
            _settingPanel.Initialize(_audioManager,_saveLoadService);
            
            _saveLoadService.SetFirstStart();
            
            _locationMap.Initialize( _saveLoadService,_locationManager);
            
            AddListener();

           
           // _locationPanel.SetActive(!isActive);
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