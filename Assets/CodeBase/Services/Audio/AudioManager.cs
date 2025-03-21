using System;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Services.Audio;
using Services.SaveLoad;
using UnityEngine;

namespace Services.Audio
{
    public class AudioManager : IAudioManager
    {
        private MusicChanger _musicChanger;
        private SoundChanger _soundChanger;

        public bool IsMenuEnabled => _isMenuEnabled;
        private bool _isMenuEnabled = false;
        public Action OnMenuEnabled;

        public AudioManager(MusicChanger musicChanger,SoundChanger soundChanger)
        {
            _musicChanger =musicChanger;
            _soundChanger=soundChanger;
        }

        public bool GetSoundEnabled()=> _soundChanger.IsSoundEnabled;

        public bool GetMusicEnabled()=> _musicChanger.IsMusicEnabled;
        public void SetMenuEnabled(bool value)
        {
            _isMenuEnabled = value;
            OnMenuEnabled?.Invoke();
        }

        public void Initialize()
        {
            _musicChanger.Initialize(this);
            _soundChanger.Initialize(this);
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