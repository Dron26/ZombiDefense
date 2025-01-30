//using Agava.WebUtility;

using Common;
using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Interface;
using Plugins.SoundInstance.Core.Static;
using Services;
using Services.SaveLoad;
using UnityEngine;

namespace UI.Audio
{
    public class AudioBackgraundChanger:MonoCache
    {
        private AudioData _audioData;
        private float _time; 
        public void Initialize( )
        {
            _audioData =AllServices.Container.Single<IAudioSettingsHandler>().GetAudioData();
        }

        // protected override void OnEnabled() =>
        //  WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;

        // protected override void OnDisabled() =>
        //  WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;

        private void OnInBackgroundChange(bool inBackground)
        {
            if (!inBackground)
            {
                _time  = Time.timeScale;
                Time.timeScale = Constants.Zero;
                SoundInstance.PauseMusic();
                SoundInstance.musicVolume = Constants.Zero;
                SoundInstance.GetMusicSource().volume = Constants.Zero;
                
            }
            else
            {
                Time.timeScale = _time;
                SoundInstance.ResumeMusic();
                SoundInstance.musicVolume = _audioData.CurrentVolumeMusic;
                SoundInstance.GetMusicSource().volume = _audioData.CurrentVolumeMusic;
               
            }
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            //  OnInBackgroundChange(hasFocus);
        }
        
        public void SetPauseWhenAds(bool isPaused)
        {
            //SetPaused(isPaused);
            //  _isPlayAds = true;
        }

        public void ClosePauseAds(bool isPaused)
        {
            // _isPlayAds = false;
            // SetPaused(isPaused);
        }

    }
}