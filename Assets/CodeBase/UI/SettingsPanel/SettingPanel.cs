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
    
        private SaveLoadService _saveLoadService;
        private AudioManager _audioManager;
        private Audio.Audio _audioSettings;
        private  bool _soundEnabled ;
        private bool _musicEnabled ;
        private float _currentVolumeSound ;
        private float _currentVolumeMusic ;
        private bool vibrationEnabled = false;
        
        public void Initialize(AudioManager audioManager,SaveLoadService saveLoadService)
        {
            _audioManager = audioManager;
            _saveLoadService = saveLoadService;

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

            _audioSettings = _saveLoadService.GetAudioSettings();
            _currentVolumeMusic = _audioSettings.CurrentVolumeMusic;
            _currentVolumeSound = _audioSettings.CurrentVolumeSound;
            _musicEnabled = _audioSettings.MusicEnabled;
            _soundEnabled = _audioSettings.SoundEnabled;
            SetSliders();
            SetButtons();
        }
    }
}