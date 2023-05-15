using Audio;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsPanel
{
    public class SettingPanel : MonoCache
    {
        [SerializeField] private Button buttonSound;
        [SerializeField] private Image _soundImage;
        [SerializeField] private Sprite _soundOn;
        [SerializeField] private Sprite _soundOff;

        [SerializeField] private Button buttonMusic;
        [SerializeField] private Image _musicImage;
        [SerializeField] private Sprite _musicOn;
        [SerializeField] private Sprite _musicOff;
        [SerializeField] private GameObject _settingPanel;
        [SerializeField] private Button _settingButton;
        [SerializeField] private Button _closeButton;
    
        [SerializeField]private Slider soundSlider;
        [SerializeField]private Slider musicSlider;
    
        private SaveLoad _saveLoad;
        private AudioController _audioController;
        private AudioSettingsParameters _audioSettingsParameters;
        private bool _isActive=true;
        private  bool _soundEnabled ;
        private bool _musicEnabled ;
        private float _currentVolumeSound ;
        private float _currentVolumeMusic ;
        private bool vibrationEnabled = false;
        
        public void Initialize(AudioController audioController,SaveLoad saveLoad)
        {
            _settingPanel.SetActive(!_isActive);
            _audioController = audioController;
            _saveLoad = saveLoad;

            LoadSound();
       
            _settingButton.onClick.AddListener(ChangeStatePanel);
            _closeButton.onClick.AddListener(ChangeStatePanel);
            buttonSound.onClick.AddListener(SetSound);
            buttonMusic.onClick.AddListener(SetNusic);
            ChangeStatePanel();
            
        }
    
    
        private void SetSliders()
        {
            soundSlider.value = _currentVolumeSound;
            musicSlider.value = _currentVolumeMusic;
        }
        private void SetButtons()
        {
            _soundImage.sprite = _soundEnabled ? _soundOn : _soundOff;
            _musicImage.sprite = _musicEnabled ? _musicOn : _musicOff;
        }

        private void ChangeStatePanel()
        {
            _isActive = !_isActive;
            _settingPanel.SetActive(_isActive);
        
        }
        
        public void SetSound()
        {
            _audioController.ToggleSound();
            _soundEnabled = _audioController.SoundEnabled;
            _soundImage.sprite = _soundEnabled ? _soundOn : _soundOff;
        
            ChangeSliderFill(soundSlider,_soundEnabled);
        }

        public void SetNusic()
        {
            _audioController.ToggleMusic();
            _musicEnabled = _audioController.MusicEnabled;
            _musicImage.sprite = _musicEnabled ? _musicOn : _musicOff;
            ChangeSliderFill(musicSlider,_musicEnabled);
        }

        public void ChangeSound( )
        {
            _audioController.SetSFXVolume(soundSlider.value);
        }

        public void ChangeMusic( )
        {
            _audioController.SetMusicVolume(musicSlider.value);
        }
    
        private void ChangeSliderFill(Slider slider ,bool isActive)
        {
            GameObject fill = slider.GetComponent<SliderSettings>().Fill;
            fill.SetActive(isActive);
        }

        private void SetPause()
        {
            SetSound();
            SetNusic();
        }

    
        public void LoadSound()
        {
            if ( AudioListener.pause==true) SetPause();

            _audioSettingsParameters = _saveLoad.GetAudioSettings();
            _currentVolumeMusic = _audioSettingsParameters.CurrentVolumeMusic;
            _currentVolumeSound = _audioSettingsParameters.CurrentVolumeSound;
            _musicEnabled = _audioSettingsParameters.MusicEnabled;
            _soundEnabled = _audioSettingsParameters.SoundEnabled;
            SetSliders();
            SetButtons();
        }

        private void OnDisable()
        {
            _settingButton.onClick.RemoveListener(ChangeStatePanel);
            _closeButton.onClick.RemoveListener(ChangeStatePanel);
        }
    
    
    
    
    }
}