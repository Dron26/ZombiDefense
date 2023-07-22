using System.Threading.Tasks;
using Audio;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Constants;
using Infrastructure.States;
using Service;
using Service.SaveLoadService;
using UI.Levels;
using UI.SettingsPanel;
using UnityEngine;

namespace UI.GeneralMenu
{
    public class GeneralMenuManager:MonoCache
    {
        [SerializeField] private LeaderboardPanel _leaderboardPanel;
        
        private YandexLeaderboard _yandexLeaderboard;
        private SaveLoad _saveLoad;
        private GameStateMachine _stateMachine;
        private LoadingCurtain _loadingCurtain  ;
        private GameBootstrapper _gameBootstrapper; 
        
        [SerializeField]private  SettingPanel _settingPanel;
        [SerializeField] private GameObject _menuPanel;
        [SerializeField]private AudioManager _audioManager;
        [SerializeField]private LevelMap _levelMap;
        private Wallet _wallet;
        
        public async void Initialize( GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _gameBootstrapper=FindObjectOfType<GameBootstrapper>();
            _saveLoad = _gameBootstrapper.GetSAaveLoad();
            _wallet=new Wallet(_saveLoad);
            await LoadAudioControllerAsync();

            _yandexLeaderboard = _gameBootstrapper.GetYandexLeaderboard();
            _loadingCurtain=_gameBootstrapper.GetLoadingCurtain();
            _menuPanel.SetActive(false);
            _loadingCurtain.OnClicked = OnClikedCurtain;
            _settingPanel.Initialize(_audioManager,_saveLoad);
            _levelMap.Initialize(_stateMachine,_saveLoad);
        }
        
        private async Task LoadAudioControllerAsync()
        {
            _audioManager.SetGeneralMenuEnabled(true);
            
            await _audioManager.InitializeAsync(_saveLoad);
            
            Debug.Log("AudioController loaded");
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

        private LeaderboardPanel CreateLeaderboard()
        {
            GameObject panel = Instantiate(_leaderboardPanel.gameObject);
            LeaderboardPanel leaderboardPanel = panel.GetComponentInChildren<LeaderboardPanel>();
            Canvas myCanvas = panel.GetComponent<Canvas>();

            myCanvas.worldCamera = FindObjectOfType<Camera>();
            leaderboardPanel.Initiallize();
            return leaderboardPanel;
        }
    }
}