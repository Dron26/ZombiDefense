using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Services;
using Services.Analytic;
using Services.Audio;
using Services.PauseService;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsPanel
{
     public class MenuPanel: MonoCache
    {
        [SerializeField] private Button _menu;
        [SerializeField] private Button _continue;
        [SerializeField] private Button _exit;
        [SerializeField] private GameObject _panel;

        
        [SerializeField] private ButtonPanel _buttonPanel;
        [SerializeField]private  SettingPanel _settingPanel;
        [SerializeField]private  GameObject _resursePanel;
        
        [SerializeField]private  GameObject _menuPanel;
        
        private IAudioManager _audioManager;
        
        private IPauseService _pauseService;
        private IAnalyticService _analyticService;

        public Action OnClickExitToMenu;
        
        public void Initialize( )
        {
            _audioManager = AllServices.Container.Single<IAudioManager>();
            _settingPanel.Initialize();
            _buttonPanel.Initialize();
            InitializeButton();
            _pauseService = AllServices.Container.Single<IPauseService>();
            _analyticService = AllServices.Container.Single<IAnalyticService>();
        }

        private void InitializeButton()
        {
            _menu.onClick.AddListener(SwitchState);
            _continue.onClick.AddListener(SwitchState);
            _exit.onClick.AddListener(SwicthScene);
        }

        private void SwitchState()
        {
            _pauseService.ChangePause(!_panel.activeSelf);
            _audioManager.SetMenuEnabled(!_panel.activeSelf);
            _panel.SetActive(!_panel.activeSelf);
            _resursePanel.SetActive(!_resursePanel.activeSelf); 
            _buttonPanel.SwitchPanelState();
            if (_panel.activeSelf) _analyticService.PauseLevel();
            else _analyticService.ResumeLevel();
        }

        private void SwicthScene()
        {
            OnClickExitToMenu?.Invoke();
        }

        public ButtonPanel GetButtonPanel() => _buttonPanel;

        private void OnDestroy()
        {
            _menu.onClick.RemoveListener(SwitchState);
            _continue.onClick.RemoveListener(SwitchState);
            _exit.onClick.RemoveListener(SwicthScene);
        }
    }
}