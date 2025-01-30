using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.Audio;

namespace Services.Audio
{
    public class SoundChanger:MonoCache
    {
        public bool IsSoundEnabled => _isSoundEnabled;
        public float _maxVolume = 100.0f;
        public float _minVolume = -80.0f;
        
        private AudioSource _soundSource;
        private AudioData _audioData;
        private AudioMixer _mixerSound;
        private AudioManager _audioManager;
        private bool _isSoundEnabled;
        private float _currentVolumeSound;
        private bool isMenuEnabled => _audioManager.IsMenuEnabled;
        
        public void Initialize( AudioManager audioManager)
        {
            _soundSource=GetComponent<AudioSource>();
            _mixerSound = (AudioMixer)Resources.Load("MixerSound");
            _audioManager=audioManager;
            _audioData =AllServices.Container.Single<IAudioSettingsHandler>().GetAudioData();
            _isSoundEnabled = _audioData.SoundEnabled;
            _currentVolumeSound = _audioData.CurrentVolumeSound;
            
            SetSoundVolume(_currentVolumeSound);
            SetState();
        }

        private void SetState()
        {
            if (_isSoundEnabled == false)
            {
                ToggleSound(false);
            }
        }

        public void ToggleSound(bool value)
        {
            _isSoundEnabled = value;
            _soundSource.mute = !value;
        }

        public void SetSoundVolume(float volume)
        {
            _currentVolumeSound = Mathf.Clamp(volume, _minVolume, _maxVolume);
            _soundSource.volume = _currentVolumeSound;
        }
        
        public AudioSource GetSoundSource()
        {
            return _soundSource;
        }
    }
}