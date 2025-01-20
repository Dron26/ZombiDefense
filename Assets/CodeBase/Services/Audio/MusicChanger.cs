using System;
using System.Reflection;
using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Plugins.SoundInstance.Core.Static;
using Services.SaveLoad;
using UI.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace Services.Audio
{
    public class MusicChanger:MonoCache
    {
        public bool IsMusicEnabled=>_isMusicEnabled;
        public float _maxVolume = 100.0f;
        public float _minVolume = -80.0f;
        
        private AudioSource _musicSource;
        private SaveLoadService _saveLoadService;
        private AudioData _audioData;
        private AudioMixer _mixerMusic;
        private float _currentVolumeMusic;
        private bool _isMusicEnabled=false;
        
        private AudioBackgraundChanger _audioBackgraundChanger;

        Type audioClipAddressesType = typeof(MusicClipAddresses);
        FieldInfo[] _musicClips =new FieldInfo[]{};
        private bool isMenuEnabled => _audioManager.IsMenuEnabled;
        private AudioManager _audioManager;
        
        public void Initialize(SaveLoadService saveLoadService,AudioManager audioManager)
        {
            _musicSource=GetComponent<AudioSource>();
            _mixerMusic = (AudioMixer)Resources.Load("MixerMusic");
            _audioManager = audioManager;
            _saveLoadService = saveLoadService;
            _audioData = _saveLoadService.GetAudioData();
            _isMusicEnabled = _audioData.MusicEnabled;
            _currentVolumeMusic = _audioData.CurrentVolumeMusic;
            _musicClips  = audioClipAddressesType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            
            _audioBackgraundChanger=GetComponent<AudioBackgraundChanger>();
            _audioBackgraundChanger.Initialize(saveLoadService);
            
            
            
            SetMusicVolume(_currentVolumeMusic);
            SetBackgroundMusics();
            AddListener();
        }

        private void SetBackgroundMusics()
        {
            if (isMenuEnabled&&_isMusicEnabled)
            {
                SoundInstance.StopRandomMusic();
                SoundInstance.StartMenuMusic(MusicClipAddresses.Menu);
            }
            else if (!_isMusicEnabled)
            {
                SoundInstance.PauseMusic();
            }
            else
            {
                 SoundInstance.StopMusic();
                 SoundInstance.StartRandomMusic();
            }
        }

        public void ToggleMusic(bool value)
        {
            if (_isMusicEnabled!=value)
            {
                SetBackgroundMusics();
            }
        }

        public void SetMusicVolume(float volume)
        {
            _currentVolumeMusic = Mathf.Clamp(volume, _minVolume, _maxVolume);
            _musicSource.volume = _currentVolumeMusic;
        }

        public AudioSource GetMusicSource()
        {
            return _musicSource;
        }

        private void SetLoadingMusic()
        {
            SoundInstance.StopRandomMusic();
            SoundInstance.StartLoadingMusic(MusicClipAddresses.Loading);
        }
        
        private void AddListener()
        {
            _audioManager.OnMenuEnabled+=SetBackgroundMusics;
            _saveLoadService.GetCurtain().OnStartLoading+=SetLoadingMusic;
            _saveLoadService.GetCurtain().OnClicked+=SetBackgroundMusics;
        }

        private void RemoveListener()
        {
            _audioManager.OnMenuEnabled-=SetBackgroundMusics;
            _saveLoadService.GetCurtain().OnStartLoading-=SetLoadingMusic;
            _saveLoadService.GetCurtain().OnClicked-=SetBackgroundMusics;
        }

        private void OnDestroy()
        {
            RemoveListener();
        }
    }
}