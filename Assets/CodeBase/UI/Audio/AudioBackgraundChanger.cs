using Agava.WebUtility;
using Data;
using Infrastructure.BaseMonoCache.Code.MonoCache;
using Plugins.SoundInstance.Core.Static;
using Service.Audio;
using Service.SaveLoad;

namespace UI.Audio
{
    public class AudioBackgraundChanger:MonoCache
    {
        private AudioData _audioData;
        private bool _isPlayAds;
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
            if (inBackground)
            {
                SoundInstance.PauseMusic();
                SoundInstance.musicVolume = ConstantsData.Zero;
                SoundInstance.GetMusicSource().volume = ConstantsData.Zero;
            }
            else
            {
                SoundInstance.ResumeMusic();
                SoundInstance.musicVolume = _audioData.CurrentVolumeMusic;
                SoundInstance.GetMusicSource().volume = _audioData.CurrentVolumeMusic;
            }
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