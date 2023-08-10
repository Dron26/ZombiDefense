using Audio;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.Constants;
using Infrastructure.States;
using Service;
using Service.SaveLoadService;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsPanel
{
    public class MenuPanel: MonoCache
    {
        [SerializeField] private Button _power;
        [SerializeField] private Button _continue;
        [SerializeField] private Button _setting;
        [SerializeField] private Button _leaderboard;
        [SerializeField] private Button _exit;
        [SerializeField] private Button _switchButtonNo;
        [SerializeField] private Button _switchButtonYes;
        [SerializeField] private GameObject _panel;

        
        [SerializeField] private LeaderboardPanel _leaderboardPanel;
        [SerializeField] private ButtonPanel _buttonPanel;
        [SerializeField]private  SettingPanel _settingPanel;
        [SerializeField]private  GameObject _exitPanel;
        [SerializeField]private  GameObject _resursePanel;
        
        [SerializeField]private  GameObject _menuPanel;
        [SerializeField]private AudioManager _audioManager;
        
        private GameStateMachine _stateMachine;
        private YandexLeaderboard _yandexLeaderboard;
        private SaveLoadService _saveLoadService;
        private GameBootstrapper _gameBootstrapper;
        
        public void Initialize(SaveLoadService saveLoadService,GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _panel.SetActive(true);
            _gameBootstrapper=FindObjectOfType<GameBootstrapper>();
            _saveLoadService = saveLoadService;
            _yandexLeaderboard = _gameBootstrapper.GetYandexLeaderboard();
            _settingPanel.Initialize(_audioManager,_saveLoadService);
            
            InitializeButton();
            _panel.SetActive(false);
            _exitPanel.SetActive(false);
        }

        private void Start()
        {
            //    _yandexLeaderboard.Initialize(CreateLeaderboard());
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
        
        private void InitializeButton()
        {
            _power.onClick.AddListener(SwitchState);
            _continue.onClick.AddListener(Continue);
            _setting.onClick.AddListener(ShowSettingPanel);
            _leaderboard.onClick.AddListener(ShowLeaderboardPanel);
            _exit.onClick.AddListener(SwitchPanel);
            _switchButtonNo.onClick.AddListener(SwitchPanel);
            _switchButtonYes.onClick.AddListener(SwicthScene);
        }

        private void SwitchState()
        {
            _panel.SetActive(!_panel.activeSelf);
            _buttonPanel.SwitchPanelState();
        }

        private void Continue()
        {
            SwitchState();
        }

        private void ShowSettingPanel()
        {
            
        }

        private void ShowLeaderboardPanel()
        {
            
        }

        private void SwitchPanel()
        {
            _exitPanel.SetActive(!_exitPanel.activeSelf);
            _menuPanel.SetActive(!_menuPanel.activeSelf);
            _resursePanel.SetActive(!_resursePanel.activeSelf); 
        }

        private void SwicthScene()
        {
            _saveLoadService.Save();
            _stateMachine.Enter<LoadLevelState,string>(SceneName.Menu); 
            Destroy(gameObject);
        }
    }
}