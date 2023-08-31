using Agava.WebUtility;
using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Plugins.SoundInstance.Core.Static;
using Service.Audio;
using Service.SaveLoad;
using UnityEngine;

namespace UI.Audio
{
    public class AudioBackgraundChanger:MonoCache
    {
        private AudioData _audioData;
        private float _time; 
        public void Initialize(SaveLoadService saveLoadService)
        {
            
            _audioData = saveLoadService.GetAudioData();
        }

        protected override void OnEnabled() =>
            WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;

        protected override void OnDisabled() =>
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;

        private void OnInBackgroundChange(bool inBackground)
        {
            if (!inBackground)
            {
                _time  = Time.timeScale;
                Time.timeScale = ConstantsData.Zero;
                SoundInstance.PauseMusic();
                SoundInstance.musicVolume = ConstantsData.Zero;
                SoundInstance.GetMusicSource().volume = ConstantsData.Zero;
                
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
                       OnInBackgroundChange(hasFocus);
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