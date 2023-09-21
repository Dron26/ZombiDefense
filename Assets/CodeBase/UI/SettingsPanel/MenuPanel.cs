using System;
using Data;
using Infrastructure;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Service;
using Service.Audio;
using Service.SaveLoad;
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
        [SerializeField] private Button _leaderboardClose;
        [SerializeField] private Button _exit;
        [SerializeField] private Button _switchButtonNo;
        [SerializeField] private Button _switchButtonYes;
        [SerializeField] private GameObject _panel;

        
        [SerializeField] private GameObject _leaderboardWindow;
        [SerializeField] private ButtonPanel _buttonPanel;
        [SerializeField]private  SettingPanel _settingPanel;
        [SerializeField]private  GameObject _exitPanel;
        [SerializeField]private  GameObject _resursePanel;
        
        [SerializeField]private  GameObject _menuPanel;
        [SerializeField]private AudioManager _audioManager;
        
        private GameStateMachine _stateMachine;
        private SaveLoadService _saveLoadService;
        private GameBootstrapper _gameBootstrapper;

        public Action OnClickExitToMenu;
        
        public void Initialize(SaveLoadService saveLoadService)
        {
            _stateMachine = saveLoadService.GetGameBootstrapper().GetStateMachine();
            _panel.SetActive(true);
            _gameBootstrapper=FindObjectOfType<GameBootstrapper>();
            _saveLoadService = saveLoadService;
            _settingPanel.Initialize(_audioManager,_saveLoadService);
            
            InitializeButton();
            _panel.SetActive(false);
            _exitPanel.SetActive(false);
            _leaderboardWindow.SetActive(false);
        }

        private void InitializeButton()
        {
            _power.onClick.AddListener(SwitchState);
            _continue.onClick.AddListener(Continue);
            _setting.onClick.AddListener(ShowSettingPanel);
            _leaderboard.onClick.AddListener(ShowLeaderboardPanel);
            _leaderboardClose.onClick.AddListener(ShowLeaderboardPanel);
            _exit.onClick.AddListener(SwitchPanel);
            _switchButtonNo.onClick.AddListener(SwitchPanel);
            _switchButtonYes.onClick.AddListener(SwicthScene);
        }

        private void SwitchState()
        {
            _audioManager.SetMenuEnabled(!_panel.activeSelf);
            _panel.SetActive(!_panel.activeSelf);
            _resursePanel.SetActive(!_resursePanel.activeSelf); 
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
            _menuPanel.SetActive(!_menuPanel.activeSelf);
            _leaderboardWindow.gameObject.SetActive(!_leaderboardWindow.activeSelf);
        }

        private void SwitchPanel()
        {
            _exitPanel.SetActive(!_exitPanel.activeSelf);
            _menuPanel.SetActive(!_menuPanel.activeSelf);
        }

        private void SwicthScene()
        {
            OnClickExitToMenu?.Invoke();
        }
    }
}