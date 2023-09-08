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
        
        [SerializeField]private  SettingPanel _settingPanel;
        [SerializeField] private GameObject _menuPanel;
        [SerializeField]private AudioManager _audioManager;
        [SerializeField]private LocationMap _locationMap;
        private MoneyData _moneyData;
        
        public  void Initialize( GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _gameBootstrapper=FindObjectOfType<GameBootstrapper>();
            _saveLoadService = _gameBootstrapper.GetSAaveLoad();
            _moneyData=_saveLoadService.MoneyData;
            
            _loadingCurtain=_gameBootstrapper.GetLoadingCurtain();
            _menuPanel.SetActive(false);
            _saveLoadService.SetCurtain(_loadingCurtain);
            _loadingCurtain.OnClicked = OnClikedCurtain;
            
            LoadAudioController();
            _settingPanel.Initialize(_audioManager,_saveLoadService);
            _locationMap.Initialize(_stateMachine,_saveLoadService);
        }
        
        private  void  LoadAudioController()
        {
            _audioManager.SetMenuEnabled(true);
            _audioManager.Initialize(_saveLoadService);
        }
        
        private void OnClikedCurtain()
        {
            _menuPanel.SetActive(true);
        }

        private void Start()
        {
        //    _yandexLeaderboard.Initialize(CreateLeaderboard());
            
            
            
            
                _loadingCurtain.OnLoaded();
        }
    }
}