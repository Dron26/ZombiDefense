using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services;
using Services.Audio;
using Services.SaveLoad;
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
    
        private IAudioManager _audioManager;
        private AudioData _audioDataSettings;
        private  bool _soundEnabled ;
        private bool _musicEnabled ;
        private float _currentVolumeSound ;
        private float _currentVolumeMusic ;
        private bool vibrationEnabled = false;
        
        public void Initialize()
        {
            _audioManager=AllServices.Container.Single<IAudioManager>();
            LoadSound();
       
            _toggleSound.onValueChanged.AddListener(SetSound);
            _toggleMusic.onValueChanged.AddListener(SetMusic);
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
        
        
        private void SetSound(bool value)
        {
            _audioManager.ToggleSound(value);
            _soundEnabled = _audioManager.GetSoundEnabled();
        }

        private void SetMusic(bool value)
        {
            _audioManager.ToggleMusic(value);
            _musicEnabled = _audioManager.GetMusicEnabled();
        }

        private void ChangeSound(float value)
        {
            _audioManager.SetSoundVolume(value);
        }

        private void ChangeMusic(float value )
        {
            _audioManager.SetMusicVolume(value);
        }

        private void SetPause(bool isActive)
        {
            SetSound(isActive);
            SetMusic(isActive);
        }

    
        public void LoadSound()
        {
            bool isActive = AudioListener.pause;
            
            SetPause(!isActive);

            _audioDataSettings =AllServices.Container.Single<IAudioSettingsHandler>().GetAudioData();
            _currentVolumeMusic = _audioDataSettings.CurrentVolumeMusic;
            _currentVolumeSound = _audioDataSettings.CurrentVolumeSound;
            _musicEnabled = _audioDataSettings.MusicEnabled;
            _soundEnabled = _audioDataSettings.SoundEnabled;
            SetSliders();
            SetButtons();
        }
    }
}