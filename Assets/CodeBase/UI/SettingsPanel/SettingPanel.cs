using Infrastructure.BaseMonoCache.Code.MonoCache;
using Service.Audio;
using Service.SaveLoad;
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
        private AudioManager _audioPlayer;
        private AudioData _audioDataSettings;
        private  bool _soundEnabled ;
        private bool _musicEnabled ;
        private float _currentVolumeSound ;
        private float _currentVolumeMusic ;
        private bool vibrationEnabled = false;
        
        public void Initialize(AudioManager audioManager,SaveLoadService saveLoadService)
        {
            _audioPlayer = audioManager;
            _saveLoadService = saveLoadService;

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
            _audioPlayer.ToggleSound(value);
            _soundEnabled = _audioPlayer.SoundEnabled;
        }

        private void SetMusic(bool value)
        {
            _audioPlayer.ToggleMusic(value);
            _musicEnabled = _audioPlayer.MusicEnabled;
        }

        private void ChangeSound(float value)
        {
            _audioPlayer.SetSoundVolume(value);
        }

        private void ChangeMusic(float value )
        {
            _audioPlayer.SetMusicVolume(value);
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

            _audioDataSettings = _saveLoadService.GetAudioData();
            _currentVolumeMusic = _audioDataSettings.CurrentVolumeMusic;
            _currentVolumeSound = _audioDataSettings.CurrentVolumeSound;
           _musicEnabled = _audioDataSettings.MusicEnabled;
            _soundEnabled = _audioDataSettings.SoundEnabled;
            SetSliders();
            SetButtons();
        }
    }
}