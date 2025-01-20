using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Services.SaveLoad;
using UnityEngine;

namespace Services.Audio
{
    public class AudioManager : MonoCache
    {
        private MusicChanger _musicChanger;
        private SoundChanger _soundChanger;
        public bool SoundEnabled => _soundChanger.IsSoundEnabled;
        public bool MusicEnabled => _musicChanger.IsMusicEnabled;

        public bool IsMenuEnabled => _isMenuEnabled;
        private bool _isMenuEnabled = false;
        public Action OnMenuEnabled;

        public void Initialize(SaveLoadService saveLoadService)
        {
            _musicChanger =GetComponent<MusicChanger>();
            _soundChanger=GetComponentInChildren<SoundChanger>();            
            
            _musicChanger.Initialize(saveLoadService, this);
            _soundChanger.Initialize(saveLoadService, this);
        }

        public void SetMenuEnabled(bool value)
        {
            _isMenuEnabled = value;
            OnMenuEnabled?.Invoke();
        }
        

        public void ToggleSound(bool value)
        {
            _soundChanger.ToggleSound(value);
        }

        public void ToggleMusic(bool value)
        {
            _musicChanger.ToggleMusic(value);
        }

        public void SetMusicVolume(float volume)
        {
            _musicChanger.SetMusicVolume(volume);
        }

        public void SetSoundVolume(float volume)
        {
            _soundChanger.SetSoundVolume(volume);
        }

        public AudioSource GetSoundSource()
        {
            return _soundChanger.GetSoundSource();
        }

        public AudioSource GetMusicSource()
        {
            return _musicChanger.GetMusicSource();
        }
    }
}