using Audio;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.SaveLoadService;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettingsPanel
{
    public class SettingPanel : MonoCache
    {
        [SerializeField] private Toggle _toggleSound;

        [SerializeField] private Toggle _toggleMusic;
    
        [SerializeField]private Slider soundSlider;
        [SerializeField]private Slider musicSlider;
    
        private SaveLoad _saveLoad;
        private AudioManager _audioManager;
        private AudioSettingsParameters _audioSettingsParameters;
        private bool _isActive=true;
        private  bool _soundEnabled ;
        private bool _musicEnabled ;
        private float _currentVolumeSound ;
        private float _currentVolumeMusic ;
        private bool vibrationEnabled = false;
        
        public void Initialize(AudioManager audioManager,SaveLoad saveLoad)
        {
            _audioManager = audioManager;
            _saveLoad = saveLoad;

            LoadSound();
       
            _toggleSound.onValueChanged.AddListener(SetSound);
            _toggleMusic.onValueChanged.AddListener(SetNusic);
            soundSlider.onValueChanged.AddListener(ChangeSound);
            musicSlider.onValueChanged.AddListener(ChangeMusic);
        }
    
    
        private void SetSliders()
        {
            soundSlider.value = _currentVolumeSound;
            musicSlider.value = _currentVolumeMusic;
        }
        private void SetButtons()
        {
            _toggleSound.isOn = _soundEnabled ;
            _toggleMusic.isOn = _soundEnabled ;
        }
        
        
        public void SetSound(bool value)
        {
            _audioManager.ToggleSound(value);
            _soundEnabled = _audioManager.SoundEnabled;
     //       _toggleSound.isOn = _soundEnabled ;
        
          //  ChangeSliderFill(soundSlider,_soundEnabled);
        }

        public void SetNusic(bool value)
        {
            _audioManager.ToggleMusic(value);
            _musicEnabled = _audioManager.MusicEnabled;
         //   _toggleMusic.isOn = _soundEnabled ;
         
          //  ChangeSliderFill(musicSlider,_musicEnabled);
        }

        public void ChangeSound(float value)
        {
            _audioManager.SetSFXVolume(value);
        }

        public void ChangeMusic(float value )
        {
            _audioManager.SetMusicVolume(value);
        }
    
        private void ChangeSliderFill(Slider slider ,bool isActive)
        {
            GameObject fill = slider.GetComponent<SliderSettings>().Fill;
            fill.SetActive(isActive);
        }

        private void SetPause(bool isActive)
        {
            SetSound(isActive);
            SetNusic(isActive);
        }

    
        public void LoadSound()
        {
            bool isActive = AudioListener.pause;
            
            SetPause(!isActive);

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
            
        }
    }
}